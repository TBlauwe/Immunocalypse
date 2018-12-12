using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(YellowPage))]
class YellowPageEditor : Editor
{
    private YellowPage yellowPage;

    private void OnEnable()
    {
        yellowPage = (YellowPage)target;
        yellowPage.items = new ADictionary<string, GameObject>();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Place here the objects you want to identify during the game with a name", MessageType.Info);
        EditorGUILayout.Space();

        IEnumerable<string> dictionarykeys = yellowPage.items.Keys.AsEnumerable();

        List<string> keys = new List<string>(dictionarykeys);

        foreach(string key in keys)
        {
            GameObject gameObject;
            yellowPage.items.TryGetValue(key, out gameObject);
            EditorGUILayout.BeginHorizontal();
            string newKey = EditorGUILayout.TextField(key);
            GameObject newGameObject = EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true) as GameObject;
            yellowPage.items.Remove(key);
            if (!GUILayout.Button("X"))
            {
                yellowPage.items.Add(newKey, newGameObject);
            }
            EditorGUILayout.EndHorizontal();
        }

        if(GUILayout.Button("Add Entry"))
        {
            yellowPage.items.Add(""+yellowPage.items.Count, null);
        }
    }
}
