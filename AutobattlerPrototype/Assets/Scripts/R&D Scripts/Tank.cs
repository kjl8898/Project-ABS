using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    [Header("General Settings")]
    private NavMeshAgent agent = null;
    [SerializeField] private Vector3 targetLoc;
    [SerializeField] private int team = 0;
    [SerializeField] private int health = 5;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject explosion;
    [SerializeField] private string targetTag;

    [Header("Cannon Settings")]
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject cannonOpening;
    [SerializeField] private List<Tank> enemyTargets;
    [SerializeField] private Tank closestTarget = null;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float fireRate = 2.0f;
    private float lastFired = 0;


    public int Team
    {
        get { return team; }
    }


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            agent = GetComponent<NavMeshAgent>();
        }
        catch
        {

        }

        try
        {
            targetLoc = GameObject.FindGameObjectWithTag(targetTag).transform.position;
        }
        catch
        {
            Debug.Log("No target location found.");
        }

        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tank"))
        {
            Tank tankScript = tank.GetComponent<Tank>();
            if(team != tankScript.Team)
            {
                enemyTargets.Add(tankScript);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(targetLoc != null && agent.isActiveAndEnabled)
        {
            agent.destination = targetLoc;
        }

        try
        {
            if(enemyTargets.Count > 0)
            {
                for (int i = enemyTargets.Count - 1; i >= 0; i--)
                {
                    if (enemyTargets[i] == null)
                    {
                        enemyTargets.RemoveAt(i);
                    }
                }

                if (target == null)
                {
                    closestTarget = enemyTargets[0];
                }

                foreach (Tank tank in enemyTargets)
                {
                    if (Vector3.Distance(transform.position, closestTarget.transform.position) >
                        Vector3.Distance(transform.position, tank.transform.position))
                    {
                        closestTarget = tank;
                    }
                }

                if (Vector3.Distance(transform.position, closestTarget.transform.position) <= 100)
                {
                    cannon.transform.LookAt(closestTarget.transform.position);
                    Fire();
                }
                else
                {
                    cannon.transform.rotation = transform.localRotation;
                }
            }
        }
        catch
        {
            closestTarget = null;
            cannon.transform.rotation = transform.localRotation;
        }
    }

    void Fire()
    {
        if(Time.time >= lastFired + fireRate)
        {
            Projectile firedShell = Instantiate(projectile, cannonOpening.transform.position, cannon.transform.rotation);
            firedShell.Damage = damage;
            lastFired = Time.time;
        }
    }

    public void TakeDamage(int _damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
