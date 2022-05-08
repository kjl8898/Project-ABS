using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrop : MonoBehaviour
{
    [SerializeField] GameObject unitToDeploy;
    [SerializeField] Vector3 deployPos;
    [SerializeField] float timeStarted;
    [SerializeField] float timeToDeploy;
    private bool deployed = false;

    private void Start()
    {
        timeStarted = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= timeStarted + timeToDeploy && !deployed)
        {
            Instantiate(unitToDeploy, transform.position, transform.rotation);
            deployed = true;
        }
    }
}
