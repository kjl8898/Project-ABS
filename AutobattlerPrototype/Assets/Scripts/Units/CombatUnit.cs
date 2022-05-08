using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : Unit
{
    // VARIABLES
    [SerializeField] private List<Unit> potentialTargets = new List<Unit>();
    [SerializeField] private Unit targetUnit;

    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float lastFired = 0;
    [SerializeField] private float projectileSpeed = 10;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject weaponOpening;
    [SerializeField] private GameObject projectile;


    // PROPERTIES
    public List<Unit> PotentialTargets
    {
        get { return potentialTargets; }
        set { potentialTargets = value; }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        CheckForTargets();
    }

    /// <summary>
    /// Checks for an enemy unit in range. Will update if the current target moves out of range.
    /// </summary>
    private void CheckForTargets()
    {
        // Check if the current target is in range
        if(targetUnit != null)
        {
            if (Vector3.Distance(transform.position, targetUnit.transform.position) > range)
            {
                targetUnit = null;
            }
            else
            {
                AimWeapon();
            }
        }

        // Find a new unit in range
        else if(targetUnit == null && FindClosestUnitInRange() != null)
        {
            targetUnit = FindClosestUnitInRange();
        }
        else
        {
            weapon.transform.localRotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Returns the closest unit in range. Returns null if no units are in range.
    /// </summary>
    /// <returns></returns>
    private Unit FindClosestUnitInRange()
    {
        float closestTargetDistance = 0;
        Unit closestTarget = null;

        // Finds the closest unit
        foreach (Unit unit in potentialTargets)
        {
            if (closestTargetDistance == 0 ||
                Vector3.Distance(transform.position, unit.transform.position) < closestTargetDistance)
            {
                closestTarget = unit;
                closestTargetDistance = Vector3.Distance(transform.position, unit.transform.position);
            }
        }

        // Is the target in range?
        if(closestTargetDistance <= range)
        {
            return closestTarget;
        }


        return null;
    }

    private void AimWeapon()
    {
        // Check if there is a clear line of sight on the target
        RaycastHit lineOfSightHit;
        Ray lineOfSightRay = new Ray(weapon.transform.position, targetUnit.transform.position - weapon.transform.position);
        Debug.DrawRay(weapon.transform.position, targetUnit.transform.position - weapon.transform.position, Color.blue);

        if (Physics.Raycast(lineOfSightRay, out lineOfSightHit))
        {
            if (lineOfSightHit.collider.gameObject == targetUnit.gameObject)
            {
                weapon.transform.LookAt(targetUnit.transform.position);
            }
            else
            {
                weapon.transform.localRotation = Quaternion.identity;
            }
        }

        // Fire if the weapon is pointed at the target Vector3(-34.7288475,2.29458666,-282.355774)
        RaycastHit aimingHit;
        Ray aimingRay = new Ray(weaponOpening.transform.position, weaponOpening.transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(weaponOpening.transform.position, weaponOpening.transform.TransformDirection(Vector3.forward), Color.red);

        if (Physics.Raycast(aimingRay, out aimingHit))
        {
            if(aimingHit.collider.gameObject == targetUnit.gameObject)
            {
                //Debug.Log(gameObject.name + " attempting to fire...");
                FireWeapon();
            }
        }
    }

    private void FireWeapon()
    {
        if(Time.time >= lastFired + fireRate
            && fireRate > 0)
        {
            //Debug.Log(gameObject.name + " firing weapon...");
            Projectile firedProjectile = Instantiate(projectile, weaponOpening.transform.position, weaponOpening.transform.rotation).GetComponent<Projectile>();
            firedProjectile.Damage = damage;
            firedProjectile.MoveSpeed = projectileSpeed;
            lastFired = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
