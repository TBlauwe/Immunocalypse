using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Factory))]
[CanEditMultipleObjects]
public class FactoryEditor : Editor
{
    private Factory factory;
    private bool expandSettings;
    private bool expandEntries;

    private void OnEnable()
    {
        factory = serializedObject.targetObject as Factory;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("This editor is designed to help you creating you own factory", MessageType.Info);

        EditorGUILayout.Space();

        expandSettings = EditorGUILayout.Foldout(expandSettings, "Factory Settings");
        if (expandSettings) _drawSettings();

        EditorGUILayout.Space();

        expandEntries = EditorGUILayout.Foldout(expandEntries, "Entries Settings");

        if (expandEntries)
        {
            EditorGUILayout.BeginVertical();
            {
                if (!EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    for (int i = 0; i < factory.entries.Count; ++i)
                    {
                        _drawEditEntry(i);
                    }
                    EditorGUILayout.BeginHorizontal();
                    {
                        _addButton();
                        _clearButton();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    for (int i = 0; i < factory.entries.Count; ++i)
                    {
                        _drawPlayEntry(i);
                    }
                    _resetAllButton();
                }
                
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
        }

    private void _addButton()
    {
        if (GUILayout.Button("Add")) {factory.entries.Add(new FactoryEntry());}
    }

    private void _clearButton()
    {
        if (GUILayout.Button("Clear")) { factory.entries.Clear(); }
    }

    private void _removeButton(int index)
    {
        if (GUILayout.Button("X")) { factory.entries.RemoveAt(index); }
    }

    private void _resetButton(int index)
    {
        if (GUILayout.Button("Reset")) { factory.entries[index].nb = factory.entries[index].originalNb; }
    }

    private void _resetAllButton()
    {
        if (GUILayout.Button("Reset All"))
        {
            foreach (FactoryEntry entry in factory.entries)
            {
                entry.nb = entry.originalNb;
            }
        }
    }

    private void _drawSettings()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Production rate");
                factory.rate = EditorGUILayout.FloatField(factory.rate);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Paused");
                factory.paused = EditorGUILayout.Toggle(factory.paused);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Allow mixing spawning");
                factory.useRandomSpawning = EditorGUILayout.Toggle(factory.useRandomSpawning);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void _drawEditEntry(int index)
    {
        SerializedProperty prop = serializedObject.FindProperty("entries").GetArrayElementAtIndex(index);
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Entry n°" + index);
                _removeButton(index);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(prop.FindPropertyRelative("prefab"));

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Nb");
                factory.entries[index].nb = factory.entries[index].originalNb = EditorGUILayout.IntField(
                    factory.entries[index].originalNb
                );
            }
            EditorGUILayout.EndHorizontal();
            
        }
        EditorGUILayout.EndVertical();
    }

    private void _drawPlayEntry(int index)
    {
        SerializedProperty prop = serializedObject.FindProperty("entries").GetArrayElementAtIndex(index);
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Entry n°" + index);
                _resetButton(index);
            }

            GUI.enabled = false;
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("prefab"));

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Remaining");
                    EditorGUILayout.IntField(factory.entries[index].nb);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUI.enabled = true;

        }
        EditorGUILayout.EndVertical();
    }
}