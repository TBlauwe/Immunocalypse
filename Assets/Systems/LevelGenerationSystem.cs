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
        }

        // ===== ARENA SETUP =====
        setupArena();
    }

    private void setupArena()
    {
        GameObject arena = Object.Instantiate<GameObject>(levelSettings.arenaPrefab);

		arena.transform.SetParent(level.transform, true);
        arena.name = "Petri Box";

        float treshold = Mathf.Clamp(getTotalRadius() * Mathf.Max(levelSettings.cellSizeX, levelSettings.cellSizeZ), 1, 100);
        arena.transform.localScale = Vector3.one * treshold;
        arena.transform.localPosition = new Vector3(arena.transform.localPosition.x, 0, arena.transform.localPosition.z) +
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

            List<Hex> neighbours = Hex.Spiral(randomHex, Random.Range(levelSettings.islandMinSize, levelSettings.islandMaxSize));
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
}