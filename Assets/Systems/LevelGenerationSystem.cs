using UnityEngine;
using System.Collections.Generic;
using FYFY;

public enum GENERATEUR
{
    RANDOM,
    ISLAND
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
            case GENERATEUR.RANDOM:
                randomGeneration(level, levelGeneration);
                break;
            case GENERATEUR.ISLAND:
                squareIslandGeneration(level, levelGeneration);
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

        float treshold = (Mathf.FloorToInt(levelGeneration.radius / 5) + 1);
        levelGeneration.petriBox.transform.localScale = Vector3.one * treshold;
        levelGeneration.petriBox.transform.localPosition = new Vector3( levelGeneration.petriBox.transform.localPosition.x,
                                                                        0,
                                                                        levelGeneration.petriBox.transform.localPosition.z) + Vector3.up * (-0.06f * treshold);
    }

    private void randomGeneration(GameObject level, LevelGeneration levelGeneration)
    {
        int radius = levelGeneration.radius;

		levelGeneration.cells = new GameObject[radius, radius];

        float totalCells = radius * radius;
        int wantedCells = Mathf.FloorToInt(levelGeneration.gridFill * totalCells);

        float proba = wantedCells / totalCells; 

        for(int h = 0; h < radius; h++)
        {
            for(int w = 0; w < radius; w++)
            {
                if (UnityEngine.Random.Range(0.0f,1.0f) <= proba)
                {
                    createCell(level, levelGeneration, h, w);
                    wantedCells--;
                }
                totalCells--;
                proba = wantedCells / totalCells; 
            }
        }
    }

    private void hexIslandGeneration(GameObject level, LevelGeneration levelGeneration)
    {
        // Possible indexes 
        List<Pair<int>> indexes = new List<Pair<int>>();

        Pair<int> idx = null;
        List<Pair<int>> neighboursIdx = null;
        int size = 0;   // Island's size (between specified min-max)

        for(int i = 0; i < levelGeneration.islandsNumber; i++)
        {
            if(indexes.Count > 0)
            {
                idx = indexes[UnityEngine.Random.Range(0, indexes.Count)];
                size = UnityEngine.Random.Range(levelGeneration.islandMinSize, levelGeneration.islandMaxSize+1);

                indexes.Remove(idx);

                neighboursIdx = getCellNeighboursAtDistance(levelGeneration, idx.a, idx.b, size);

                createCell(level, levelGeneration, idx.a, idx.b);
                foreach(Pair<int> neighbourIdx in neighboursIdx)
                {
                    if (indexes.Contains(neighbourIdx))
                    {
                        createCell(level, levelGeneration, neighbourIdx.a, neighbourIdx.b);
                        indexes.Remove(neighbourIdx);
                    }
                }
            }
        }
    }

    private void squareIslandGeneration(GameObject level, LevelGeneration levelGeneration)
    {
        int radius = levelGeneration.radius;

		levelGeneration.cells = new GameObject[radius, radius];

        // Possible indexes 
        List<Pair<int>> indexes = new List<Pair<int>>();
        for (int h = 0; h < radius; h++)
        {
            for (int w = 0; w < radius; w++)
            {
                indexes.Add(new Pair<int>(h, w));
            }
        }

        Pair<int> idx = null;
        List<Pair<int>> neighboursIdx = null;
        int size = 0;   // Island's size (between specified min-max)

        for(int i = 0; i < levelGeneration.islandsNumber; i++)
        {
            if(indexes.Count > 0)
            {
                idx = indexes[UnityEngine.Random.Range(0, indexes.Count)];
                size = UnityEngine.Random.Range(levelGeneration.islandMinSize, levelGeneration.islandMaxSize+1);

                indexes.Remove(idx);

                neighboursIdx = getCellNeighboursAtDistance(levelGeneration, idx.a, idx.b, size);

                createCell(level, levelGeneration, idx.a, idx.b);
                foreach(Pair<int> neighbourIdx in neighboursIdx)
                {
                    if (indexes.Contains(neighbourIdx))
                    {
                        createCell(level, levelGeneration, neighbourIdx.a, neighbourIdx.b);
                        indexes.Remove(neighbourIdx);
                    }
                }
            }
        }
    }
	
	private void createCell (GameObject level, LevelGeneration levelGeneration, int h, int w) {
		GameObject cell = Object.Instantiate<GameObject>(levelGeneration.cellPrefab);

		Vector3 position;
		position.x = (levelGeneration.gap * h) * getOuterRadius(cell) * 1.5f;
		position.y = 0f;
		position.z = ((levelGeneration.gap * w) + h * 0.5f - h / 2) * getInnerRadius(cell) * 2.0f;

		cell.transform.SetParent(level.transform, false);
		cell.transform.localPosition = position;
        cell.transform.eulerAngles = new Vector3(cell.transform.eulerAngles.x, UnityEngine.Random.Range(0.0f, 359.99f), cell.transform.eulerAngles.z);
        cell.name = "Cell [" + h + "][" + w + "]";

        levelGeneration.cells[h, w] = cell;
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

    private List<Pair<int>> getCellNeighbours(LevelGeneration levelGeneration, int x, int y)
    {
        int radius = levelGeneration.radius;

        List<Pair<int>> neighbours = new List<Pair<int>>();

        if (y - 1 >= 0){ neighbours.Add(new Pair<int>(x, y - 1)); }
        if (y + 1 >= 0){ neighbours.Add(new Pair<int>(x, y + 1)); }
        if (x - 1 >= 0){ neighbours.Add(new Pair<int>(x - 1, y)); }
        if (x + 1 >= 0){ neighbours.Add(new Pair<int>(x + 1, y)); }

        if(x % 2 == 0)
        {
            if (x - 1 > 0 && y - 1 >= 0){ neighbours.Add(new Pair<int>(x - 1, y - 1)); }
            if (y - 1 >= 0 && x + 1 <= radius - 1){ neighbours.Add(new Pair<int>(x + 1, y - 1)); }
        }
        else
        {
            if (x + 1 <= radius - 1 && y + 1 <= radius - 1){ neighbours.Add(new Pair<int>(x + 1, y + 1)); }
            if (x - 1 > 0 && y + 1 <= radius - 1){ neighbours.Add(new Pair<int>(x - 1, y + 1)); }
        }

        return neighbours;
    }

    private List<Pair<int>> getCellNeighboursAtDistance(LevelGeneration levelGeneration, int x, int y, int distance)
    {
        List<Pair<int>> neighbours = new List<Pair<int>>();
        List<Pair<int>> unvisitedCells = new List<Pair<int>>{ new Pair<int>(x, y) };

        for (int i=0; i < distance; i++)
        {
            int counter = unvisitedCells.Count;
            if(counter > 0)
            {
                for(int j = 0; j < counter; j++)
                {
                    Pair<int> idx = unvisitedCells[0];
                    if (!neighbours.Contains(idx))
                    {
                        neighbours.Add(idx);
                    }

                    List<Pair<int>> cellNeighbours = getCellNeighbours(levelGeneration, idx.a, idx.b);
                    foreach(Pair<int> neighbourIdx in cellNeighbours)
                    {
                        if (!neighbours.Contains(neighbourIdx) && 
                            !unvisitedCells.Contains(neighbourIdx))
                        {
                            unvisitedCells.Add(neighbourIdx);
                        }
                    }

                    unvisitedCells.RemoveAt(0);
                }
            }
        }

        return neighbours;
    }
}