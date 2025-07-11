using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class InstanciarSobreCubo : MonoBehaviour
{
    public GameObject prefab;
    public int cantidad = 10;
    public string grupoNombre = "Instancias";
    public float offsetAltura = 0.0f; // opcional: separación sobre la cara del cubo

    public void Generar()
    {
#if UNITY_EDITOR
        if (prefab == null)
        {
            Debug.LogWarning("Asigna un prefab.");
            return;
        }

        // Eliminar instancias previas
        Transform grupoExistente = transform.Find(grupoNombre);
        if (grupoExistente != null)
        {
            DestroyImmediate(grupoExistente.gameObject);
        }

        GameObject grupo = new GameObject(grupoNombre);
        grupo.transform.SetParent(transform);
        grupo.transform.localPosition = Vector3.zero;

        MeshRenderer rend = GetComponent<MeshRenderer>();
        if (rend == null)
        {
            Debug.LogError("Se requiere un MeshRenderer.");
            return;
        }

        Bounds bounds = rend.bounds;

        for (int i = 0; i < cantidad; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            float y = bounds.max.y + offsetAltura;

            Vector3 posicion = new Vector3(x, y, z);

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(obj, "Instanciar objeto");
            obj.transform.position = posicion;
            obj.transform.SetParent(grupo.transform);
        }
#endif
    }
}
