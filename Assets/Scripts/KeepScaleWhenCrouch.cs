using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScaleWhenCrouch : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] GameObject player;
    Transform parentTransform;

    private Vector3 initialLocalScale;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = player.gameObject.transform;
        initialLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 parentLossy = parentTransform.lossyScale;
        transform.localScale = new Vector3(
            initialLocalScale.x / parentLossy.x,
            initialLocalScale.y / parentLossy.y,
            initialLocalScale.z / parentLossy.z
            );
    }
}
