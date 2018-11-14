using UnityEngine;

[RequireComponent(typeof(Dragable))]
public class Card : MonoBehaviour {
    public GameObject   entityPrefab;
    public bool         initialized;
    public string       title;
    public Sprite       image;
    public bool inGame = false;
}