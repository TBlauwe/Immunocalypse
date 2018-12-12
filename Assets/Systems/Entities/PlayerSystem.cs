using UnityEngine;
using FYFY;
using System.IO;
using System.Collections.Generic;

public class PlayerSystem : FSystem {
    private readonly Family _players = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    private string exportedFileName = "save";

    private readonly string levelsFolder = "/Resources/saves/";

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
        public int prefabID;
        public CardDescription(Card sourceObject) : base(sourceObject)
        {
            Origin origin = sourceObject.GetComponent<Origin>();
            if (origin != null)
            {
                this.prefabID = origin.sourceObject.GetInstanceID();
            }
        }



        public GameObject getGameObject()
        {
            /*GameObject gameObject = GameObject.Get;
            GameObjectManager.bind(gameObject);
            return gameObject;*/
            return null;
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
            //Debug.Log(name);
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
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _players)
        {
            if(Input.GetKeyDown("s"))
            {
                Player playerComponent = go.GetComponent<Player>();
                string filePath = Application.dataPath + levelsFolder + this.exportedFileName + ".json";
                File.WriteAllText(filePath, JsonUtility.ToJson(new PlayerDescription(playerComponent)));
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