using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InstantiateCaña))]
public class InstantiateCañaInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstantiateCaña script = (InstantiateCaña)target;
        if (GUILayout.Button("Generar Instancias"))
        {
            script.Generar();
        }
    }
}
