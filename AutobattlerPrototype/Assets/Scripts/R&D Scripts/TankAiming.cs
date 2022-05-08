using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAiming : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turret.transform.LookAt(target.transform.position);
        turret.transform.rotation = new Quaternion(0, turret.transform.rotation.y, 0, 1);

        barrel.transform.LookAt(target.transform.position);
        //barrel.transform.rotation = new Quaternion(barrel.transform.rotation.x, barrel.transform.rotation.y, 0, 1);
    }
}
