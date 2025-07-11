using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPerroController : MonoBehaviour
{

    [SerializeField] private GameObject familiarPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");

        GameObject instance = Instantiate(familiarPrefab);
        instance.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
    }
}
