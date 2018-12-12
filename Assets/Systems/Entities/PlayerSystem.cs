using UnityEngine;
using FYFY;
using System.IO;
using System.Collections.Generic;

public class PlayerSystem : FSystem {
    private readonly Family _players = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    private readonly Family _yellowPageFamily = FamilyManager.getFamily(new AllOfComponents(typeof(YellowPage)));

    private string exportedFileName = "save";

    private readonly string levelsFolder = "/Resources/saves/";

    private static YellowPage yellowPage;

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
                this.prefabID = yellowPage.items.findKey(origin.sourceObject);
            }
        }

        public GameObject getGameObject()
        {
            GameObject prefab;
            yellowPage.items.TryGetValue(this.prefabID, out prefab);
            GameObject result = GameObject.Instantiate(prefab);
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

        public static List<GameObject> getGameObjects(List<CardDescription> descriptions)
        {
            List<GameObject> resultat = new List<GameObject>();
            foreach (CardDescription description in descriptions)
            {
                resultat.Add(description.getGameObject());
            }
            return resultat;
        }
    }


    [System.Serializable]
    private class PlayerDescription : Description<Player>
    {
        public List<CardDescription> globalDeck;

        public List<CardDescription> levelDeck;

        public PlayerDescription(Player sourceObject) : base(sourceObject)
        {
            this.globalDeck = CardDescription.fromList(sourceObject.globalDeck);
            this.levelDeck = CardDescription.fromList(sourceObject.levelDeck);
        }

        public GameObject getObject()
        {
            string name = "Player" + ((int)(Random.value * 100));
            GameObject go = new GameObject("Player");
            Player player = go.AddComponent<Player>();
            player.globalDeck = CardDescription.getGameObjects(this.globalDeck);
            player.levelDeck = CardDescription.getGameObjects(this.levelDeck);
            GameObjectManager.bind(go);
            return go;
        }


    }

    public PlayerSystem()
    {
        _players.addEntryCallback(DontDestroyCallback);
        if (_players.Count == 0)
        {
            CreatePlayer();
        }

        foreach(GameObject go in _yellowPageFamily)
        {
            yellowPage = go.GetComponent<YellowPage>();
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
                GameObject newPlayer = JsonUtility.FromJson<PlayerDescription>(File.ReadAllText(path)).getObject();
                GameObjectManager.unbind(go);
                GameObject.Destroy(go);
            }
        }
	}


    // A call back function telling that all player objects shouldn't be destroyed
    private void DontDestroyCallback(GameObject player)
    {
        GameObjectManager.dontDestroyOnLoadAndRebind(player);
    }

    private void CreatePlayer()
    {
        // Create the player
        GameObject go = new GameObject("Player");
        Player player = go.AddComponent<Player>();

        // Add some cards
        player.globalDeck.Add(CreateMacrophageCard(go));
        player.globalDeck.Add(CreateBCellCard(go));
        // player.levelDeck.Add(CreateMacrophageCard(go));

        GameObjectManager.bind(go);
    }

    private GameObject CreateMacrophageCard(GameObject parent)
    {
        GameObject go = Resources.Load<GameObject>("Cards/MacrophageCard");
        GameObject card = Object.Instantiate(go);
        card.transform.SetParent(parent.transform);
        return card;
    }

    private GameObject CreateBCellCard(GameObject parent)
    {
        GameObject go = Resources.Load<GameObject>("Cards/BCellCard");
        GameObject card = Object.Instantiate(go);
        card.transform.SetParent(parent.transform);
        return card;
    }
}
