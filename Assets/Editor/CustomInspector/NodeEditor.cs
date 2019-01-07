using UnityEditor;
using UnityEngine;

public class WaypointEditor : EditorWindow
{
    private Node FirstNode;
    private Node SecondNode;

    [MenuItem("Game/Waypoints")]
    public static void ShowWindow()
    {
        GetWindow<WaypointEditor>(false, "Waypoints", true);
    }

    void OnGUI()
    {
        FirstNode = (Node)EditorGUILayout.ObjectField(new GUIContent("First node"), FirstNode, typeof(Node), true);
        SecondNode = (Node)EditorGUILayout.ObjectField(new GUIContent("Second node"), SecondNode, typeof(Node), true);

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create link"))
            {
                SerializedProperty f_connections = new SerializedObject(FirstNode).FindProperty("connections");
                AddIfNotPresent(f_connections, SecondNode);

                SerializedProperty s_connections = new SerializedObject(SecondNode).FindProperty("connections");
                AddIfNotPresent(s_connections, FirstNode);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void OnDrawGizmos() // ???? not working
    {
        SerializedProperty f_connections = new SerializedObject(FirstNode).FindProperty("connections");
        DisplayConnections(f_connections, FirstNode);

        SerializedProperty s_connections = new SerializedObject(SecondNode).FindProperty("connections");
        DisplayConnections(s_connections, SecondNode);
    }

    private void AddIfNotPresent(SerializedProperty list, Object elem)
    {
        int i = 0;
        while (i < list.arraySize && !list.GetArrayElementAtIndex(i).objectReferenceValue.Equals(elem)) ++i;
        if (i == list.arraySize)
        {
            list.arraySize++;
            list.GetArrayElementAtIndex(i).objectReferenceValue = elem;
        }
        list.serializedObject.ApplyModifiedProperties();
    }

    private void DisplayConnections(SerializedProperty neighbours, Node node)
    {
        for (int i = 0; i < neighbours.arraySize; ++i)
        {
            Node other = (Node)neighbours.GetArrayElementAtIndex(i).objectReferenceValue;
            Gizmos.DrawLine(node.transform.position, other.transform.position);
        }
    }
}
/*
[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private ReorderableList connections;
    private Node go;

    private void OnEnable()
    {
        go = (Node)serializedObject.targetObject;
        connections = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("connections"),
            true, true, true, true
        );

        connections.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = connections.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                element,
                GUIContent.none
            );
        };

        connections.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Connections");
        };

        connections.onRemoveCallback = (ReorderableList l) =>
        {
            int index = connections.index;
            Node node = (Node)connections.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue;

            if (node != null && node.connections.Contains(go))
            {
                node.connections.Remove(go);
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        };
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("Drag and drop waypoints to create link between them", MessageType.Info);
        connections.DoLayoutList();

        serializedObject.ApplyModifiedProperties();        
    }

    private void ConnectNodesIfRequired(SerializedProperty property)
    {
        SerializedProperty connections = property.FindPropertyRelative("connections");

        for (int i = 0; i < connections.arraySize; ++i)
        {
            SerializedProperty prop = connections.GetArrayElementAtIndex(i);
            if (prop != null && prop.objectReferenceValue != null)
            {
                Node n = GameObject.Find(prop.objectReferenceValue.name).GetComponent<Node>();
                if (n != null && !n.connections.Contains(go))
                {
                    n.connections.Add(go);
                    Debug.Log(n.connections[n.connections.Count - 1]);
                }
                SerializedObject s_node = new SerializedObject(prop.objectReferenceValue);
                s_node.Update();

                SerializedProperty n_connections = s_node.FindProperty("connections");
                bool found = false;
                int j = 0;
                while (!found && j < n_connections.arraySize)
                {
                    if (n_connections.GetArrayElementAtIndex(j).objectReferenceValue.Equals(go))
                    {
                        found = true;
                    }
                    j++;
                }
                if (!found)
                {
                    n_connections.arraySize++;
                    n_connections.GetArrayElementAtIndex(n_connections.arraySize - 1).objectReferenceValue = go;
                }

                s_node.ApplyModifiedProperties();
            }
        }
    }
}*/