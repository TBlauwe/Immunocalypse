using UnityEngine;
using FYFY;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;
using System.Collections.Generic;
using FYFY_plugins.TriggerManager;

/// <summary>
///     This System is designed to manage cards in the User Interface.
/// </summary>
public class UICardSystem : FSystem {

    // All the available cards in the current scene (and active)
    private readonly Family _cards = FamilyManager.getFamily(new AllOfComponents(typeof(Card)));

    public UICardSystem()
    {
        _cards.addEntryCallback(InitializeCard);

        foreach (GameObject go in _cards)
        {
            InitializeCard(go);
        }
    }

    /// <summary>
    ///     Use this method for (optionally) initialize your card with the provided text and image.
    /// </summary>
    /// <param name="go">The GameObject having the Card component</param>
    private void InitializeCard(GameObject go)
    {
        Card card = go.GetComponent<Card>();

        // Initialize card if not done yet
        TMPro.TextMeshProUGUI Text = go.transform.Find("Title").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Text.text = card.title;

        TMPro.TextMeshProUGUI Counter = go.transform.Find("Counter").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        Counter.text = card.counter.ToString();
    }
}