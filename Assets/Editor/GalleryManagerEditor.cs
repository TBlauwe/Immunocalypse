using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GalleryManager))]
public class GalleryManagerEditor : Editor {

    private ReorderableList list;

    private void OnEnable() {
		list = new ReorderableList(serializedObject, 
        		serializedObject.FindProperty("galleryModels"), 
        		true, true, true, true);

        list.drawElementCallback = 
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Type"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("GalleryModelGO"), GUIContent.none);
                element.FindPropertyRelative("Order").intValue = index;
            };

        list.drawHeaderCallback = (Rect rect) => {
	        EditorGUI.LabelField(rect, "Gallery Models Order");
        };

        list.onSelectCallback = (ReorderableList l) =>{
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("GalleryModelGO").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        list.onCanRemoveCallback = (ReorderableList l) => {
	        return l.count > 1;
        };
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();

        EditorGUILayout.HelpBox("Editor for defining an order for gallery models", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

		list.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
