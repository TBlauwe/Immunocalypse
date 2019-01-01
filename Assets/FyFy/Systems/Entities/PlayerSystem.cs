using UnityEngine;
using FYFY;
using System.IO;
using System.Collections.Generic;

public class PlayerSystem : FSystem {
    private readonly Family _players = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    private readonly Family _yellowPageFamily = FamilyManager.getFamily(new AllOfComponents(typeof(YellowPageComponent)));

    private string exportedFileName = "save";

    private readonly string levelsFolder = "/Resources/saves/";

    private static YellowPageComponent yellowPage;

    [System.Serializable]
    private abstract class Jsonable
    {
        public string toJSON()
        {
            string result = JsonUtility.ToJson(this);
            Debug.Log(result);
            return result;
        }
    }

    [System.Serializable]
    private abstract class Description<T> : Jsonable
    {
        public Description(T sourceObject)
        {

        }

        public Description(GameObject gameObject) : this(gameObject.GetComponent<T>())
        {

        }

    }

    [System.Serializable]
    private class CardDescription : Description<Card>
    {
        public string prefabID;
        public CardDescription(Card sourceObject) : base(sourceObject)
        {
            Origin origin = sourceObject.GetComponent<Origin>();
            if (origin != null)
            {
                this.prefabID = origin.sourceObjectKey;
            }
        }

        public GameObject getGameObject(GameObject parent)
        {
            GameObject prefab = YellowPageUtils.GetSourceObject(yellowPage, this.prefabID);
            GameObject result = GameObject.Instantiate(prefab, parent.transform);
            return result;
        }

        public static List<CardDescription> fromList(List<GameObject> list)
        {
            List<CardDescription> resultat = new List<CardDescription>();
            foreach (GameObject gameObject in list)
            {
                Card component = gameObject.GetComponent<Card>();
                if (component != null)
                {
                    resultat.Add(new CardDescription(component));
                }
            }
            return resultat;
        }

        public static List<GameObject> getGameObjects(List<CardDescription> descriptions, GameObject parent)
        {
            List<GameObject> resultat = new List<GameObject>();
            foreach (CardDescription description in descriptions)
            {
                resultat.Add(description.getGameObject(parent));
            }
            return resultat;
        }
    }


    [System.Serializable]
    private class PlayerDescription : Description<Player>
    {
        public List<CardDescription> globalDeck;

        public PlayerDescription(Player sourceObject) : base(sourceObject)
        {
            this.globalDeck = CardDescription.fromList(sourceObject.globalDeck);
        }

        public GameObject getObject()
        {
            string name = "Player" + ((int)(Random.value * 100));
            GameObject go = new GameObject(name);
            Player player = go.AddComponent<Player>();
            player.globalDeck = CardDescription.getGameObjects(this.globalDeck, go);
            GameObjectManager.bind(go);
            return go;
        }


    }

    public PlayerSystem()
    {
        if (_players.Count == 0)
        {
            Debug.LogWarning("No Player in scene, creating one from scratch");
            CreatePlayer();
        }

        foreach(GameObject go in _yellowPageFamily)
        {
            yellowPage = go.GetComponent<YellowPageComponent>();
            break;
        }
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _players)
        {
            if(Input.GetKeyDown("s"))
            {
                Player playerComponent = go.GetComponent<Player>();
                string filePath = Application.dataPath + levelsFolder + this.exportedFileName + ".json";
                string json = JsonUtility.ToJson(new PlayerDescription(playerComponent));
                Debug.Log(json);
                File.WriteAllText(filePath, json);
            }
            else if(Input.GetKeyDown("l"))
            {
                string path = Application.dataPath + levelsFolder + this.exportedFileName + ".json";
                PlayerDescription playerDescription = JsonUtility.FromJson<PlayerDescription>(File.ReadAllText(path));
                Player playerComponent = go.GetComponent<Player>();
                foreach (GameObject gameObject in playerComponent.globalDeck)
                {
                    GameObjectManager.unbind(gameObject);
                    GameObject.Destroy(gameObject);
                }
                foreach(GameObject gameObject in playerComponent.levelDeck)
                {
                    GameObjectManager.unbind(gameObject);
                    GameObject.Destroy(gameObject);
                }
                playerComponent.globalDeck.Clear();
                playerComponent.levelDeck.Clear();
                playerComponent.globalDeck = CardDescription.getGameObjects(playerDescription.globalDeck, go);
            }
        }
	}

    private void CreatePlayer()
    {
        GameObject go = new GameObject("Player");
        Player player = go.AddComponent<Player>();
        go.AddComponent<DontDestroyOnLoad>();
        GameObjectManager.bind(go);
    }
}
