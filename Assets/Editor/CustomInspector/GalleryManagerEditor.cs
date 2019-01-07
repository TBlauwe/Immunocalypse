using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GalleryManager))]
public class GalleryManagerEditor : Editor {

    private ReorderableList galleryModels;
    private ReorderableList cameraSpots;

    private void OnEnable() {
		galleryModels = new ReorderableList(serializedObject, 
        		serializedObject.FindProperty("galleryModels"), 
        		true, true, true, true);

		cameraSpots = new ReorderableList(serializedObject, 
        		serializedObject.FindProperty("cameraSpots"), 
        		true, true, true, true);

        galleryModels.drawElementCallback = 
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = galleryModels.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("GalleryModelGO"), GUIContent.none);
                element.FindPropertyRelative("Order").intValue = index;
            };

        galleryModels.drawHeaderCallback = (Rect rect) => {
	        EditorGUI.LabelField(rect, "Gallery Models Order");
        };

        galleryModels.onSelectCallback = (ReorderableList l) =>{
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("GalleryModelGO").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        galleryModels.onCanRemoveCallback = (ReorderableList l) => {
	        return l.count > 1;
        };

        cameraSpots.drawElementCallback = 
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = cameraSpots.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.ObjectField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };

        cameraSpots.drawHeaderCallback = (Rect rect) => {
	        EditorGUI.LabelField(rect, "Overview camera spots");
        };

        cameraSpots.onSelectCallback = (ReorderableList l) =>{
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("GameObject").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        cameraSpots.onCanRemoveCallback = (ReorderableList l) => {
	        return l.count > 1;
        };
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();

        EditorGUILayout.HelpBox("Editor for defining an order for gallery models", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

		galleryModels.DoLayoutList();

        EditorGUILayout.HelpBox("Editor for defining an order for overview camera spots", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

		cameraSpots.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
