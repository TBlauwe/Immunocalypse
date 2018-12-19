using UnityEngine;
using System.Collections.Generic;
using FYFY;

public class LevelGenerationSystem : FSystem {

    public Family _levelGO;

    private GameObject level;
    private LevelSettings levelSettings;

    public LevelGenerationSystem()
    {
        // ===== VARIABLES SETUP =====
        Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelSettings)));

        level = _levelGO.First();
        levelSettings = level.GetComponent<LevelSettings>();

        // ===== GRID SETUP =====
        switch (levelSettings.gridType)
        {
            case GRID_TYPE.HEXAGON:
                hexIslandGeneration();
                break;
            case GRID_TYPE.GRID:
                gridGeneration();
                break;
        }

        // ===== ARENA SETUP =====
        setupArena();
    }

    private void setupArena()
    {
        GameObject arena = Object.Instantiate<GameObject>(levelSettings.arenaPrefab);

		arena.transform.SetParent(level.transform, true);
        arena.name = "Petri Box";

        float mult = 0.0f;
        switch (levelSettings.gridType)
        {
            case GRID_TYPE.HEXAGON:
                mult = getTotalRadius();
                break;
            case GRID_TYPE.GRID:
                mult = (levelSettings.width - getTotalHeight() > 0) ? levelSettings.width : getTotalHeight();
                mult /= 1.75f;
                break;
        }
        Debug.Log(mult);
        float treshold = Mathf.Clamp(mult * Mathf.Max(levelSettings.cellSizeX, levelSettings.cellSizeZ), 1, 100);
        arena.transform.localScale = Vector3.one * treshold;

        float x = arena.transform.localPosition.x;
        float y = 0;
        float z = arena.transform.localPosition.z;

        if(levelSettings.gridType == GRID_TYPE.GRID)
        {
            x += (levelSettings.width * levelSettings.cellSizeX) / 4;
            z += (levelSettings.width * levelSettings.cellSizeZ) / 3;
        }
        arena.transform.localPosition = new Vector3(x, y, z) +
                                            Vector3.up * (-0.06f * treshold);

        levelSettings.size = arena.GetComponent<Renderer>().bounds.size;
        levelSettings.center = arena.transform.position;  
    }

    private void hexIslandGeneration()
    {
        int radius = levelSettings.radius;

        // ===== STEP I =====
        // Place cell's island based on settings 
        List<Hex> possibleHexes = Hex.Spiral(new Hex(0, 0, 0), radius);
        IListExtensions.Shuffle<Hex>(possibleHexes);

        for(int i=0; i < levelSettings.islandsNumber && possibleHexes.Count > 0; i++)
        {
            Hex randomHex = IListExtensions.Pop<Hex>(possibleHexes);
            spawnGameObjectAt(levelSettings.cellPrefab, randomHex, levelSettings.cellPrefab.name);

            List<Hex> neighbours = Hex.Spiral(randomHex, Random.Range(levelSettings.islandMinSize, levelSettings.islandMaxSize+1));
            foreach(Hex neighbour in neighbours)
            {
                if (possibleHexes.Contains(neighbour))
                {
                    spawnGameObjectAt(levelSettings.cellPrefab, neighbour, levelSettings.cellPrefab.name);
                    possibleHexes.Remove(neighbour);
                }
            }
        }
        radius++;

        // ===== OPTIONAL STEP =====
        // Loop cycling through no cell's land
        /**
        foreach(Hex hex in Hex.SpiralAt(new Hex(0, 0, 0), radius, levelSettings.safeZoneRings))
        {
        }
        **/

        // ===== STEP II =====
        // Place factories based on settings 
        radius += levelSettings.numberOffSafeZoneLayers;

        possibleHexes = Hex.SpiralAt(new Hex(0, 0, 0), radius, levelSettings.numberOfFactoriesLayers);
        IListExtensions.Shuffle<Hex>(possibleHexes);

        while(levelSettings.factoryPrefabList.Count > 0 && possibleHexes.Count > 0)
        {
            Hex randomHex = IListExtensions.Pop<Hex>(possibleHexes);
            GameObject factory = getNextFactory();
            
            if (factory != null) { spawnGameObjectAt(factory, randomHex, factory.name); }
        }
    }

    private void gridGeneration()
    {
        int width = levelSettings.width;
        int height = levelSettings.cellLayerDefense + levelSettings.numberOffSafeZoneLayers + levelSettings.numberOfFactoriesLayers;
        foreach(Hex hex in Hex.Rectangle(width, height))
        {
        }

        int h_offset = Mathf.FloorToInt((height * 1.0f) / 2.0f);
        int h_counter = 0;
        for(int h=h_offset; h > -height + h_offset; h--)
        {
            h_counter++;
            int w_offset = Mathf.FloorToInt((h * 1.0f) / 2.0f);
            for(int w=-w_offset; w < width - w_offset; w++)
            {
                Hex hex = new Hex(h, w, -h - w);
                if(h_counter <= levelSettings.cellLayerDefense) // CELL LAND
                    spawnGameObjectAt(levelSettings.cellPrefab, hex, "cell");
                else if(h_counter <= levelSettings.cellLayerDefense + levelSettings.numberOffSafeZoneLayers) {
                    // NO CELL'S LANDS
                }
                else if(h_counter <= getTotalHeight()) // FACTORY LAND
                {
                    GameObject factory = getNextFactory();
                    if (factory != null) { spawnGameObjectAt(factory, hex, factory.name); }
                }
            }
        }
    }

    // Returns next factory from levelSettings.factoryPrefabList. Each time a factory is returned, his counter
    // is decremented until it reaches zero where it will be removed from the list
    // If no factory remains, null is returned

    private GameObject getNextFactory()
    {
        int idx = levelSettings.factoryPrefabList.Count - 1;
        
        if(idx < 0) { return null; }

        GameObject factory = levelSettings.factoryPrefabList[idx];
        int        counter = levelSettings.factoryNumberList[idx];

        counter--;
        if(counter == 0){
            levelSettings.factoryPrefabList.RemoveAt(idx);
            levelSettings.factoryNumberList.RemoveAt(idx);
        }else{
            levelSettings.factoryNumberList[idx] = counter;
        }

        return factory;
    }

    // Spawn a prefab at hex location with the specified name.
    // bUseHexCoordinate true = Append hex coordinates to name.
    private void spawnGameObjectAt(GameObject prefab, Hex hex, string name="Object", bool bUseHexCoordinate=true, bool randomizeYRotation=true)
    {
        GameObject go = Object.Instantiate<GameObject>(prefab);

        Pair<double, double> coord = Hex.ToCoordinate(  hex,
                                                        getOuterRadius(),
                                                        getInnerRadius());

        Vector3 position = new Vector3((float)coord.a, 0f, (float) coord.b);

        go.transform.SetParent(level.transform, false);
        go.transform.localPosition = position;
        go.transform.eulerAngles = new Vector3( go.transform.eulerAngles.x,
                                                (randomizeYRotation ? UnityEngine.Random.Range(0.0f, 359.99f) : go.transform.eulerAngles.y),
                                                go.transform.eulerAngles.z);

        go.name = name + (bUseHexCoordinate ? "[" + Hex.ToIndex(hex).a + "][" + Hex.ToIndex(hex).b + "]" : "");

        GameObjectManager.bind(go);
    }

    public float getOuterRadius()
    {
        return levelSettings.cellSizeX / 2;
    }

    public float getInnerRadius()
    {
        return getOuterRadius() * 0.866025404f;
    }

    public int getTotalRadius()
    {
        return levelSettings.radius + levelSettings.numberOffSafeZoneLayers + levelSettings.numberOfFactoriesLayers;
    }

    public int getTotalHeight()
    {
        return levelSettings.cellLayerDefense + levelSettings.numberOffSafeZoneLayers + levelSettings.numberOfFactoriesLayers;
    }
}