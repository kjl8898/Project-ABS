using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // VARIABLES    
    [SerializeField] private bool isPlayerUnit;
    [SerializeField] private bool isDestroyed;

    [SerializeField] private int health;
    [SerializeField] private GameObject killFx;


    // PROPERTIES
    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
        set { isPlayerUnit = value; }
    }

    public bool IsDestroyed
    {
        get { return isDestroyed; }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckHealth();
    }

    /// <summary>
    /// Tags the unit as destroyed for the UnitManager to clean up.
    /// </summary>
    private void CheckHealth()
    {
        if(health <= 0 && !isDestroyed)
        {
            isDestroyed = true;
        }
    }

    public void TakeDamage(int _damageDealt)
    {
        health -= _damageDealt;
    }

    private void OnDestroy()
    {
        if(killFx != null)
        {
            Instantiate(killFx, transform.position, transform.rotation);
        }
    }
}
