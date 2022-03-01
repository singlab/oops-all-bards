using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ReadWriteTXT))]
public class ReadWriteTXTEditor : Editor
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
        ReadWriteTXT read_write = (ReadWriteTXT)target;
        if (GUILayout.Button("Test Reading"))
        {
            ClearLog();
            read_write.Reset();
            read_write.TestReading();
        }
        if (GUILayout.Button("Test Writing"))
        {
            ClearLog();
            read_write.Reset();
            read_write.Reload();
            read_write.TestWriting();
        }
        if (GUILayout.Button("Change Mudd's Level"))
        {
            ClearLog();
            read_write.Reset();
            read_write.Reload();
            read_write.ChangeMuddLevel();
        }
        if (GUILayout.Button("Reset"))
        {
            ClearLog();
            read_write.Reset();
        }
    }
}
