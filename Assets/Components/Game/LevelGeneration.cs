using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public int radius = 8;      // Taille de la grille en X et Y

    public float gap = 1.1f;    // Espace entre les hexagons

    public GENERATEUR gen = GENERATEUR.RANDOM;  // Type de génération

    // ===== RANDOM SPECIFIC =====
    public float gridFill = 0.5f;   // Pourcentage de remplissage de la grille

    // ===== ISLAND SPECIFIC =====
    public int islandsNumber = 1;   // Nombre d'ilôts à générer

    public int islandMinSize = 1;   // Taille minimale d'un ilôt
    public int islandMaxSize = 2;   // Taille maximale d'un ilôt

    // ===== HEX SPECIFIC =====
    public int reservedFactoryRing = 1;
    public int safeZoneRings = 2;

    // ===== GENERALS =====
    public GameObject cellPrefab;   // Référence du prefab d'une cellule
    public GameObject[,] cells;     // Référence des GamesObjects cellule

    public GameObject petriPrefab;  // Référence du prefab de la boîte de petri
    public GameObject petriBox;     // Référence du GameObject de la boîte de petri 

    // ===== GAMEPLAY SPECIFIC =====
    public List<Pair<GameObject, int>> factoryList = new List<Pair<GameObject, int>>();
}