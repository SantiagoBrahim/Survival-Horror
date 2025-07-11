using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class InstantiateCaña : MonoBehaviour
{
    public GameObject prefab;
    public int cantidad = 10;
    public Vector3 eje = Vector3.right;
    public string grupoNombre = "Instancias";
    public float alturaY;

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

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("Se requiere un MeshFilter.");
            return;
        }

        Bounds localBounds = meshFilter.sharedMesh.bounds;
        Vector3 min = transform.TransformPoint(localBounds.min);
        Vector3 max = transform.TransformPoint(localBounds.max);

        Bounds worldBounds = new Bounds();
        worldBounds.SetMinMax(Vector3.Min(min, max), Vector3.Max(min, max));

        for (int i = 0; i < cantidad; i++)
        {
            float t = (float)i / (cantidad - 1);
            Vector3 pos = Vector3.zero;

            if (eje == Vector3.right)
            {
                pos.x = Mathf.Lerp(worldBounds.min.x, worldBounds.max.x, t);
                pos.y = alturaY;
                pos.z = Random.Range(worldBounds.min.z, worldBounds.max.z);
            }
            else if (eje == Vector3.up)
            {
                pos.y = Mathf.Lerp(worldBounds.min.y, worldBounds.max.y, t);
                pos.x = Random.Range(worldBounds.min.x, worldBounds.max.x);
                pos.z = Random.Range(worldBounds.min.z, worldBounds.max.z);
            }
            else if (eje == Vector3.forward)
            {
                pos.z = Mathf.Lerp(worldBounds.min.z, worldBounds.max.z, t);
                pos.x = Random.Range(worldBounds.min.x, worldBounds.max.x);
                pos.y = alturaY;
            }

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(obj, "Instanciar Objeto");

            obj.transform.position = pos;
            obj.transform.SetParent(grupo.transform);
        }
#endif
    }
}
