using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCannon : MonoBehaviour
{
    [SerializeField] private GameObject cannonGO;
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject targetingGO;
    private Vector3 targetDirection;

    [SerializeField] private List<GameObject> barrelOpenings;
    private int currentBarrel = 0;
    [SerializeField] private float fireRate = 4;
    private float shotsPerBurst = 3;
    private float timeBetweenBursts = 0.25f;
    private float lastFired = 0;

    [SerializeField] private float turnSpeed = 15;
    private float rotation = 0;
    [SerializeField] private int maxRotationAngle = 90;
    [SerializeField] private bool isTopside = true;
    [SerializeField] private int topSideRotMod = 1;
    private float pitch = 0;
    [SerializeField] private float minPitch = 70;
    [SerializeField] private float maxPitch = 0;

    [SerializeField] private float projectileVelocity = 10;
    [SerializeField] int damage = 1;

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lastFired = Time.time;

        targetDirection = targetingGO.transform.forward;

        if(isTopside)
        {
            topSideRotMod = 1;
        }
        else
        {
            topSideRotMod = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateCannonSimple();

        if(Input.GetKey(KeyCode.Mouse0))
        {
            FireCannon();
        }
    }

    void RotateCannonSimple()
    {
        // Grab the world position the mouse is pointig at
        Vector3 targetLoc = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit);
            targetLoc = hit.point;
            Debug.Log(targetLoc);
        }
        else
        {
            return;
        }

        cannonGO.transform.LookAt(targetLoc);
    }

    void RotateCannon()
    {
        // Grab the world position the mouse is pointig at
        Vector3 targetLoc = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit);
            targetLoc = hit.point;
            Debug.Log(targetLoc);
        }
        else
        {
            return;
        }

        // Aim the targeting object at the target
        targetingGO.transform.LookAt(targetLoc);
        float targetingRot = targetingGO.transform.localRotation.eulerAngles.y;
        //float targetingPitch = Vector3.Angle(targetingGO.transform.forward, cannonGO.transform.forward);
        float targetingPitch = (targetingGO.transform.rotation * Quaternion.Euler(180, 0, 0)).eulerAngles.x;

        Debug.Log("targeting pitch " + targetingPitch);
        Debug.Log("my pitch " + pitch);

        lr.SetPosition(0, cannonGO.transform.position);
        lr.SetPosition(1, targetLoc - cannonGO.transform.localPosition);


        // Align the cannon with the targeting object
        if (targetingRot < -5 || 5 < targetingRot)
        {
            if(Mathf.Abs(rotation) <= maxRotationAngle || maxRotationAngle == 0)
            {
                if (targetingRot > 180)
                {
                    rotation -= turnSpeed * Time.deltaTime;
                    if(rotation < -maxRotationAngle && maxRotationAngle != 0)
                    {
                        rotation = -maxRotationAngle;
                    }
                }
                else
                {
                    rotation += turnSpeed * Time.deltaTime;
                    if (rotation > maxRotationAngle && maxRotationAngle != 0)
                    {
                        rotation = maxRotationAngle;
                    }
                }
            }
        }

        // Align the barrels with the target
        Vector3 localTargetPos = cannonGO.transform.InverseTransformPoint(targetLoc);
        localTargetPos.x = 0.0f;

        Vector3 clampedLocalVec2Target = localTargetPos;
        if(localTargetPos.y >= 0.0f)
        {
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * maxPitch, float.MaxValue);
        }
        else
        {
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * minPitch, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
        Quaternion newRotation = Quaternion.RotateTowards(cannonGO.transform.localRotation, rotationGoal, 2.0f * turnSpeed * Time.deltaTime);

        cannonGO.transform.localRotation = newRotation;

        /*if(targetingPitch < pitch)
        {
            pitch -= turnSpeed * Time.deltaTime;
            if(pitch < minPitch)
            {
                pitch = minPitch;
            }
        }
        else if(targetingPitch > pitch)
        {
            pitch += turnSpeed * Time.deltaTime;
            if (pitch > maxPitch)
            {
                pitch = maxPitch;
            }
        }

*//*        lr.SetPosition(0, barrelOpenings[1].transform.position);
        lr.SetPosition(1, barrelOpenings[1].transform.position + (barrelOpenings[1].transform.forward.normalized * 500));*//*

        cannonGO.transform.localRotation = Quaternion.Euler(pitch, rotation, 0);
        //cannonGO.transform.Rotate(pitch, rotation, 0);*/
    }

    void FireCannon()
    {
        if(Time.time >= lastFired + fireRate)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                targetDirection = hit.point;
            }
            else
            {
                return;
            }

            for (int i = 0; i < shotsPerBurst; i++)
            {
                StartCoroutine(FireShell(timeBetweenBursts * i, i));
            }

            lastFired = Time.time;
        }
    }

    IEnumerator FireShell(float _waitTime, int _barrel)
    {
        yield return new WaitForSeconds(_waitTime);

        Projectile shell = Instantiate(projectile, 
            barrelOpenings[_barrel].transform.position, 
            barrelOpenings[_barrel].transform.rotation);

        shell.Damage = 2;
    }
}
