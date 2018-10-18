using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SubjectToForces))]
public class SubjectToForcesEditor : Editor {

    private SubjectToForces component;          // Reference to the component
    private readonly GUIStyle IN_LABEL_STYLE = new GUIStyle();      // Style for input labels

    private void OnEnable()
    {
        component = serializedObject.targetObject as SubjectToForces;
        IN_LABEL_STYLE.alignment = TextAnchor.MiddleRight;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("This editor is designed to help you applying the right forces to your entity. Forces are defined with the Lenard-Jones potential function.", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Forces Settings", EditorStyles.boldLabel);
        
        for (int i = 0; i < component.appliedForces.Count; i++)
        {
            _drawForceSpecEntry(i);
        }

        EditorGUILayout.BeginHorizontal();
        {
            _addForceSpecButton();
            _clearForceSpecButton();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

    }

    // ======================================   
    // ========== Editor's Button  ==========   
    // ======================================   

    private void _addForceSpecButton()
    {
        if (GUILayout.Button("Add")) { component.appliedForces.Add(new ForceSpec()); }
    }

    private void _clearForceSpecButton()
    {
        if (GUILayout.Button("Clear")) { component.appliedForces.Clear(); }
    }

    private void _removeForceSpecButton(int index)
    {
        if (GUILayout.Button("X")) { component.appliedForces.RemoveAt(index); }
    }

    // ===============================
    // ========== Painters  ==========   
    // ===============================

    private void _drawForceSpecEntry(int index)
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel("Force Specification " + (index + 1).ToString());
                
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Mask", IN_LABEL_STYLE, GUILayout.MinWidth(35.0f));
                component.appliedForces[index].Mask = EditorGUILayout.IntField(
                    component.appliedForces[index].Mask,
                    GUILayout.MinWidth(25.0f)
                );
            
                EditorGUILayout.LabelField("A", IN_LABEL_STYLE, GUILayout.MinWidth(12.0f));
                component.appliedForces[index].A = EditorGUILayout.FloatField(
                   component.appliedForces[index].A,
                   GUILayout.MinWidth(30.0f)
                );
            
                EditorGUILayout.LabelField("B", IN_LABEL_STYLE, GUILayout.MinWidth(12.0f));
                component.appliedForces[index].B = EditorGUILayout.FloatField(
                    component.appliedForces[index].B,
                    GUILayout.MinWidth(30.0f)
                );
            
                EditorGUILayout.LabelField("n", IN_LABEL_STYLE, GUILayout.MinWidth(10.0f));
                component.appliedForces[index].N = EditorGUILayout.FloatField(
                    component.appliedForces[index].N,
                    GUILayout.MinWidth(25.0f)
                );
            
                EditorGUILayout.LabelField("m", IN_LABEL_STYLE, GUILayout.MinWidth(10.0f));
                component.appliedForces[index].M = EditorGUILayout.FloatField(
                    component.appliedForces[index].M,
                    GUILayout.MinWidth(25.0f)
                );

                _removeForceSpecButton(index);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}
