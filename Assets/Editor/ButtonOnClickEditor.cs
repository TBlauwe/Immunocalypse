using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ButtonOnClick))]
public class ButtonOnClickEditor : Editor {

    private ReorderableList functionNames;

    private void OnEnable() {
        functionNames = new ReorderableList(serializedObject, serializedObject.FindProperty("functionNames"), true, true, true, true);

        functionNames.drawElementCallback = 
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = functionNames.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };

        functionNames.drawHeaderCallback = (Rect rect) => {
	        EditorGUI.LabelField(rect, "Functions to call on click");
        };

        functionNames.onSelectCallback = (ReorderableList l) =>{
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("functionNames").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        functionNames.onCanRemoveCallback = (ReorderableList l) => {
	        return l.count > 1;
        };

	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();

        EditorGUILayout.HelpBox("Editor for defining which system's function should be called when clicking this button." +
            "Function's name should exactly matches the one in this list", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

		functionNames.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
