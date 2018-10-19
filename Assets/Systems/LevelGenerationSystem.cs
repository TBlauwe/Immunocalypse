using UnityEngine;
using System.Collections.Generic;
using FYFY;

public enum GENERATEUR
{
    HEXAGON
}

public class LevelGenerationSystem : FSystem {

    public Family _levelGO; 

    public LevelGenerationSystem()
    {
        Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelGeneration)));

        GameObject level = _levelGO.First();
        LevelGeneration levelGeneration = level.GetComponent<LevelGeneration>();

        switch (levelGeneration.gen)
        {
            case GENERATEUR.HEXAGON:
                hexIslandGeneration(level, levelGeneration);
                break;
        }
        level.transform.position = Vector3.zero - getCenter(level); // Center the grid
        setupPetriBox(level, levelGeneration);
    }

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
	}

    private void setupPetriBox(GameObject level, LevelGeneration levelGeneration)
    {
        levelGeneration.petriBox = Object.Instantiate<GameObject>(levelGeneration.petriPrefab);

		levelGeneration.petriBox.transform.SetParent(level.transform, true);
        levelGeneration.petriBox.name = "Petri Box";

        float treshold = (Mathf.FloorToInt(getRadius(levelGeneration) / 5) + 1);
        levelGeneration.petriBox.transform.localScale = Vector3.one * treshold;
        levelGeneration.petriBox.transform.localPosition = new Vector3( levelGeneration.petriBox.transform.localPosition.x,
                                                                        0,
                                                                        levelGeneration.petriBox.transform.localPosition.z) + Vector3.up * (-0.06f * treshold);
    }

    private void hexIslandGeneration(GameObject level, LevelGeneration levelGeneration)
    {
        int radius = levelGeneration.radius;
        foreach(Hex hex in Hex.Spiral(new Hex(0, 0, 0), radius))
        {
            spawnCell(level, levelGeneration, hex);
        }

        radius++;

        foreach(Hex hex in Hex.SpiralAt(new Hex(0, 0, 0), radius, levelGeneration.safeZoneRings))
        {
            // createCell(level, levelGeneration, hex);
        }

        radius += levelGeneration.safeZoneRings;

        List<Hex> possibleHexes = Hex.SpiralAt(new Hex(0, 0, 0), radius, levelGeneration.reservedFactoryRing);
        IListExtensions.Shuffle<Hex>(possibleHexes);

        while(levelGeneration.factoryPrefabList.Count > 0 && possibleHexes.Count > 0)
        {
            Hex randomHex = IListExtensions.Pop<Hex>(possibleHexes);
            
            int lastIndex = levelGeneration.factoryPrefabList.Count - 1;

            GameObject factory = levelGeneration.factoryPrefabList[lastIndex];
            int number = levelGeneration.factoryNumberList[lastIndex];

            number--;
            if(number == 0){
                levelGeneration.factoryPrefabList.RemoveAt(lastIndex);
                levelGeneration.factoryNumberList.RemoveAt(lastIndex);
            }
            else
            {
                levelGeneration.factoryNumberList[lastIndex] = number;
            }

            if (factory != null) { spawnFactory(level, levelGeneration, factory, randomHex); }
        }
    }

	private void spawnCell(GameObject level, LevelGeneration levelGeneration, Hex hex) {
        GameObject cell = Object.Instantiate<GameObject>(levelGeneration.cellPrefab);

        Pair<double, double> coord = Hex.ToCoordinate(hex, levelGeneration.gap, getOuterRadius(levelGeneration.cellPrefab), getInnerRadius(levelGeneration.cellPrefab));

        Vector3 position;
        position.x = (float) coord.a;
        position.y = 0f;
        position.z = (float) coord.b;

        cell.transform.SetParent(level.transform, false);
        cell.transform.localPosition = position;
        cell.transform.eulerAngles = new Vector3(cell.transform.eulerAngles.x, UnityEngine.Random.Range(0.0f, 359.99f), cell.transform.eulerAngles.z);

        cell.name = "Cell [" + Hex.ToIndex(hex).a + "][" + Hex.ToIndex(hex).b + "]";

        GameObjectManager.bind(cell);
	}

	private void spawnFactory(GameObject level, LevelGeneration levelGeneration, GameObject factory, Hex hex) {
        GameObject cell = Object.Instantiate<GameObject>(factory);

        Pair<double, double> coord = Hex.ToCoordinate(hex, levelGeneration.gap, getOuterRadius(levelGeneration.cellPrefab), getInnerRadius(levelGeneration.cellPrefab));

        Vector3 position;
        position.x = (float) coord.a;
        position.y = 0f;
        position.z = (float) coord.b;

        cell.transform.SetParent(level.transform, false);
        cell.transform.localPosition = position;
        cell.transform.eulerAngles = new Vector3(cell.transform.eulerAngles.x, UnityEngine.Random.Range(0.0f, 359.99f), cell.transform.eulerAngles.z);

        cell.name = "Factory Cell [" + Hex.ToIndex(hex).a + "][" + Hex.ToIndex(hex).b + "]";

        GameObjectManager.bind(cell);
	}

    public float getOuterRadius(GameObject go)
    {
        Vector3 size = go.GetComponent<Renderer>().bounds.size/2;
        return Mathf.Max(size.x, size.z);
    }

    public float getInnerRadius(GameObject go)
    {
        return getOuterRadius(go) * 0.866025404f;
    }

    public int getRadius(LevelGeneration levelGeneration)
    {
        return levelGeneration.radius + levelGeneration.safeZoneRings + levelGeneration.reservedFactoryRing;
    }

    public Vector3 getCenter(GameObject go)
    {
        Bounds bounds = new Bounds();
        foreach(Transform child in go.transform)
        {
            bounds.Encapsulate(child.position);
        }
        return bounds.center;
    }

    public Vector3 getBoxSize(LevelGeneration levelGeneration)
    {
        return levelGeneration.petriBox.transform.lossyScale;
    }
}