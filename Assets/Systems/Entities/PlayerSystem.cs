using UnityEngine;
using FYFY;

public class PlayerSystem : FSystem {
    private readonly Family _players = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

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
            // Nothing yet
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

        GameObjectManager.bind(go);
    }

    private GameObject CreateMacrophageCard(GameObject parent)
    {
        GameObject go = Resources.Load<GameObject>("Test/UI_Macrophage_Card");
        GameObject card = Object.Instantiate(go);
        card.transform.SetParent(parent.transform);
        return card;
    }
}