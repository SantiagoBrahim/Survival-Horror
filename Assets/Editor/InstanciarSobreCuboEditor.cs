using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InstanciarSobreCubo))]
public class InstanciarSobreCuboEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InstanciarSobreCubo script = (InstanciarSobreCubo)target;
        if (GUILayout.Button("Generar instancias sobre el cubo"))
        {
            script.Generar();
        }
    }
}
