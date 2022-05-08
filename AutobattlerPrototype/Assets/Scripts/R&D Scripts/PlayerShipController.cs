using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 30;
    [SerializeField] private float currentVelocity = 0;
    [SerializeField] private float acceleration = 2;
    [SerializeField] private float maxRoationSpeed = 15;
    [SerializeField] private float currentRotationSpeed = 0;
    [SerializeField] private float rotationalAcceleration = 0.25f;
    [SerializeField] private GameObject projectileGO;
    [SerializeField] private GameObject tankGO;
    [SerializeField] private Vector3 heading;
    [SerializeField] private GameObject headingPointerGO;
    private bool autopilotOn = false;

    // Start is called before the first frame update
    void Start()
    {
        heading = transform.forward.normalized;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            autopilotOn = !autopilotOn;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            CreateTank();
        }

        //Fire();
    }

    private void FixedUpdate()
    {
        //Move();
        MoveRelative();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            heading = new Vector3(heading.x, 0, -1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            heading = new Vector3(heading.x, 0, 1);
        }
        else
        {
            heading = new Vector3(heading.x, 0, 0);
        }


        if (Input.GetKey(KeyCode.A))
        {
            heading = new Vector3(1, 0, heading.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            heading = new Vector3(-1, 0, heading.z);
        }
        else
        {
            heading = new Vector3(0, 0, heading.z);
        }

        //heading = heading.normalized;

        if(Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
        {
            //transform.LookAt(transform.position + heading); // change this to a rotate method
            RotateShip();
            transform.position += transform.forward.normalized * currentVelocity * Time.deltaTime;
        }
    }

    private void RotateShip()
    {
        headingPointerGO.transform.LookAt(transform.localPosition + heading);

        float shipRot = Mathf.Abs(transform.rotation.y);
        float headingRot = Mathf.Abs(headingPointerGO.transform.localRotation.eulerAngles.y);

        Debug.Log(headingRot);

        //headingRot = headingRot % 360;

        if(headingRot < -1 || 1 < headingRot)
        {
            Debug.Log("need to rotate");
            if(headingRot > 180)
            {
                Debug.Log("Rotating left");
                transform.Rotate(transform.up, -currentRotationSpeed);
            }
            else
            {
                Debug.Log("Rotating right");
                transform.Rotate(transform.up, currentRotationSpeed);
            }
        }
    }

    private void MoveRelative()
    {
        if(autopilotOn || Input.GetKey(KeyCode.W))
        {
            if(currentVelocity < maxVelocity)
            {
                currentVelocity += acceleration;
            }

            if (currentVelocity > maxVelocity)
            {
                currentVelocity = maxVelocity;
            }

        }
        else
        {
            if(currentVelocity > 0)
            {
                currentVelocity -= acceleration;
            }

            if (currentVelocity < 0)
            {
                currentVelocity = 0;
            }
        }

        transform.position += transform.forward.normalized * currentVelocity * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (currentRotationSpeed > -maxRoationSpeed)
                {
                    currentRotationSpeed -= rotationalAcceleration;
                }

                if (currentRotationSpeed < -maxRoationSpeed)
                {
                    currentRotationSpeed = -maxRoationSpeed;
                }

                transform.Rotate(transform.up, currentRotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (currentRotationSpeed < maxRoationSpeed)
                {
                    currentRotationSpeed += rotationalAcceleration;
                }

                if (currentRotationSpeed > maxRoationSpeed)
                {
                    currentRotationSpeed = maxRoationSpeed;
                }

                transform.Rotate(transform.up, currentRotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            if(currentRotationSpeed > 0)
            {
                currentRotationSpeed -= rotationalAcceleration;
            }
            else
            {
                currentRotationSpeed += rotationalAcceleration;
            }

            transform.Rotate(transform.up, currentRotationSpeed * Time.deltaTime);
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetLoc = Vector3.zero;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                targetLoc = hit.point;
            }
            else
            {
                return;
            }

            GameObject firedProjectile = Instantiate(projectileGO, transform.position, Quaternion.identity);
            firedProjectile.transform.LookAt(targetLoc);
        }
    }

    private void CreateTank()
    {
        Vector3 targetLoc = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            targetLoc = hit.point;
        }
        else
        {
            return;
        }

        GameObject tank = Instantiate(tankGO, targetLoc, Quaternion.identity);
        tank.transform.Rotate(0, Random.Range(0, 360), 0);
    }
}
