#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor {

    private bool _foldout = false;
    private string _replaceWith = "mixamorig";
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Color defaultColor = GUI.contentColor;
        GUI.contentColor = Color.cyan;
        if (_foldout = EditorGUILayout.BeginFoldoutHeaderGroup(_foldout, "Rigs corrections"))
        {
            GUI.contentColor = defaultColor;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefix");
            _replaceWith = EditorGUILayout.TextField(_replaceWith);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Rename"))
            {
                Transform[] children = ((Character)target).GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    Match match = Regex.Match(child.name, "^.*:");
                    if (match.Success)
                        child.name = $"{_replaceWith}:{child.name[match.Value.Length..]}";
                }
            }
        }
        GUI.contentColor = defaultColor;
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
#endif