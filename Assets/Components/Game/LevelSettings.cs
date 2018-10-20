using System;
using System.Collections.Generic;
using UnityEngine;

public enum GRID_TYPE
{
    HEXAGON
}

public enum ARENA_TYPE 
{
    PETRI
}

public class LevelSettings : MonoBehaviour {
    // ==================
    // ===== HIDDEN =====
    // ==================
    // Used to store data for interaction with other systems

    public Vector3 center;  // Used to store level's center after generation
    public Vector3 size;    // Used to store level's size after generation

    // ====================
    // ===== EDITABLE =====
    // ====================

    // ===== GRID SETTINGS =====
    public int radius   = 8;      // Combien de lignes ?

    public float cellSizeX  = 0.3f;    // Taille en X d'une cellule 
    public float cellSizeZ  = 0.3f;    // Taille en Z d'une cellule 

    public GRID_TYPE gridType   = GRID_TYPE.HEXAGON;  // Type de génération pour la grille

    // ===== ARENA SETTINGS =====
    public ARENA_TYPE arenaType = ARENA_TYPE.PETRI;  // Type de génération pour la grille

    // ===== ISLANDS SETTINGS =====
    public int islandsNumber = 1;   // Nombre d'ilôts à générer

    public int islandMinSize = 1;   // Taille minimale d'un ilôt
    public int islandMaxSize = 2;   // Taille maximale d'un ilôt

    // ===== RINGS SETTINGS =====
    public int numberOfFactoriesLayers  = 1; // Nombre de lignes qui seront utilisées pour placer les spawners
    public int numberOffSafeZoneLayers  = 2; // Nombre de lignes qui seront utilisées pour espacer les spawners et les lignes à défendre

    // ===== GENERALS =====
    public GameObject cellPrefab;   // Référence du prefab d'une cellule
    public GameObject arenaPrefab;  // Référence du prefab de la boîte de petri

    // ===== GAMEPLAY SPECIFIC =====
    public List<GameObject> factoryPrefabList = new List<GameObject>(); // Liste des prefabs qui seront utilisées pour spawner des factories
    public List<int>        factoryNumberList = new List<int>();        // Nombre de fois qu'un prefab doit être utilisées (associé au prefab du même indice)
}