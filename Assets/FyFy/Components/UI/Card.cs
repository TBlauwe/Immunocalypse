using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour {
    public string       title="Card's name";    // Name of the card
    public int          counter=1;              // Number of entities to add in the pool
    public GameObject   entityPrefab;           // Entity to spawn when using this card
}