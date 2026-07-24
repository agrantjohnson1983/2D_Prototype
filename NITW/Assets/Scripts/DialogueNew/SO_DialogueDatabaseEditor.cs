using UnityEditor;
using UnityEngine;
using AVSim.Dialogue;

[CustomEditor(typeof(SO_DialogueDatabase))]
public class SO_DialogueDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var db = (SO_DialogueDatabase)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Rebuild From CSV"))
        {
            db.RebuildFromCSV();
            EditorUtility.SetDirty(db);
            Debug.Log("Rebuilt dialogue database: " + db.Lines.Count + " lines parsed.");
        }

        EditorGUILayout.HelpBox(
            "Export the Google Sheet / Excel file as CSV, drop it in SourceCSV, then click Rebuild.",
            MessageType.Info);
    }
}
