using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private float startTime;
    [SerializeField] float lifetime = 3;
    [SerializeField] bool repeat = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= startTime + lifetime)
        {
            if(repeat)
            {
                Instantiate(gameObject, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
