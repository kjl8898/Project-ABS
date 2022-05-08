using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject objToFollowGO;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = objToFollowGO.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = objToFollowGO.transform.position + offset;
    }
}
