using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ReadWriteCSV))]
public class ReadWriteCSVEditor : Editor
{
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ReadWriteCSV read_write = (ReadWriteCSV)target;

        if (GUILayout.Button("Read CSV"))
        {
            ClearLog();
            read_write.Reset();
            read_write.ReadFile();
            read_write.Printer();
        }

        if (GUILayout.Button("Re-write CSV"))
        {
            ClearLog();
            read_write.ReWriteFile();
            read_write.Printer();
        }

        if (GUILayout.Button("Change Alera Level"))
        {
            ClearLog();
            read_write.ChangeLevel();
            read_write.Printer();
        }

        if (GUILayout.Button("Add Example"))
        {
            ClearLog();
            read_write.AddExample();
            read_write.Printer();
        }
    }
}
