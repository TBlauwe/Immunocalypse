using UnityEngine;

public class Movable : MonoBehaviour {
    public static int LAYER_1 = 1;
    public static int LAYER_2 = 2;
    public static int LAYER_3 = 3;
    public static int LAYER_4 = 4;
    public static int LAYER_5 = 5;
    public static int LAYER_6 = 6;
    public static int LAYER_7 = 7;
    public static int LAYER_8 = 8;

    public int layer_mask = 0;
    public float range = 5;
    public float safetyDistance = 2;
    public bool is_moving;
    public float velocity = 2;
    public GameObject target;
}