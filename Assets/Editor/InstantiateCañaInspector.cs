using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InstantiateCa�a))]
public class InstantiateCa�aInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiateCa�a script = (InstantiateCa�a)target;
        if (GUILayout.Button("Generar Instancias"))
        {
            script.Generar();
        }
    }
}
