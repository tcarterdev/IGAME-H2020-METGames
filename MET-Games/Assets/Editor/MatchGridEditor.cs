using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MatchGrid))]
public class MatchGridEditor : Editor
{
    SerializedProperty isGridSquare;

    SerializedProperty gridX;
    SerializedProperty gridY;

    SerializedProperty gridOffset;

    SerializedProperty cardPrefab;

    private void OnEnable()
    {
        gridX = serializedObject.FindProperty("gridX");
        gridY = serializedObject.FindProperty("gridY");
        gridOffset = serializedObject.FindProperty("gridOffset");

        isGridSquare = serializedObject.FindProperty("isGridSquare");

        cardPrefab = serializedObject.FindProperty("cardPrefab");
    }

    public override void OnInspectorGUI()
    {
        MatchGrid matchGrid = (MatchGrid)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(isGridSquare);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(gridX);

        if (!matchGrid.GetIsGridSquare())
        {
            EditorGUILayout.PropertyField(gridY);
        }

        EditorGUILayout.PropertyField(gridOffset);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(cardPrefab);

        serializedObject.ApplyModifiedProperties();
    }
}
