using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private List<Unit> playerUnits = new List<Unit>();
    [SerializeField] private List<Unit> enemyUnits = new List<Unit>();
    [SerializeField] private List<Unit> allUnits = new List<Unit>();
    [SerializeField] private List<Unit> destroyedUnits = new List<Unit>();

    // PROPERTIES
    public List<Unit> PlayerUnits
    {
        get { return playerUnits; }
    }

    public List<Unit> EnemyUnits
    {
        get { return enemyUnits; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Collect and update any units starting on the map
        foreach(GameObject unitGO in GameObject.FindGameObjectsWithTag("Unit")) // Unity says I did a bad
        {
            Unit unit = unitGO.GetComponent<Unit>();
            
            if(unit.IsPlayerUnit)
            {
                playerUnits.Add(unit);
            }
            else
            {
                enemyUnits.Add(unit);
            }

            allUnits.Add(unit);
        }

        UpdatePotentialTargets(true);
        UpdatePotentialTargets(false);
    }

    // Update is called once per frame
    void Update()
    {
        CleanUpUnits();
    }

    /// <summary>
    /// Spawns a unit and assigns it to a team. Updates unit's potentialTarget lists accordingly.
    /// </summary>
    /// <param name="_unitToSpawn"></param>
    /// <param name="_isPlayerUnit"></param>
    /// <param name="_spawnLocation"></param>
    /// <param name="_spawnRotation"></param>
    public void SpawnUnit(GameObject _unitToSpawn, bool _isPlayerUnit, Vector3 _spawnLocation, Quaternion _spawnRotation)
    {
        Unit spawnedUnit = Instantiate(_unitToSpawn, _spawnLocation, _spawnRotation).GetComponent<Unit>();
        spawnedUnit.IsPlayerUnit = _isPlayerUnit;

        allUnits.Add(spawnedUnit);

        if (_isPlayerUnit)
        {
            playerUnits.Add(spawnedUnit);
            UpdatePotentialTargets(false);
        }
        else
        {
            enemyUnits.Add(spawnedUnit);
            UpdatePotentialTargets(true);
        }
    }

    /// <summary>
    /// Checks for any destroyed units.
    /// </summary>
    private void CleanUpUnits()
    {
        bool updatePlayerUnits = false;
        bool updateEnemyUnits = false;

        // Checks for any destroyed units
        foreach (Unit unit in allUnits)
        {
            if(unit.IsDestroyed)
            {
                destroyedUnits.Add(unit);
            }
        }

        // If there are destroyed units, finds them, destroys them, and updates
        //      unit's potentialTarget lists accordingly.
        if(destroyedUnits.Count > 0)
        {
            // Removes the destroyed units from their team lists.
            for(int i = destroyedUnits.Count - 1; i >= 0; i--)
            {
                Unit unitToDestroy = destroyedUnits[i];

                if (unitToDestroy.IsPlayerUnit)
                {
                    playerUnits.Remove(unitToDestroy);

                    if (!updateEnemyUnits)
                    {
                        updateEnemyUnits = true;
                    }
                }
                else
                {
                    enemyUnits.Remove(unitToDestroy);

                    if (!updatePlayerUnits)
                    {
                        updatePlayerUnits = true;
                    }
                }

                allUnits.Remove(unitToDestroy);
                destroyedUnits.Remove(unitToDestroy);

                Destroy(unitToDestroy.gameObject);
            }

            destroyedUnits.Clear();

            // Update potential target lists if nessecary.
            if (updatePlayerUnits)
            {
                UpdatePotentialTargets(true);
            }

            if (updateEnemyUnits)
            {
                UpdatePotentialTargets(false);
            }
        }
    }

    /// <summary>
    /// Updates the potentialTarget lists of the corresponding team's units.
    /// </summary>
    /// <param name="_playerUnits"></param>
    private void UpdatePotentialTargets(bool _playerUnits)
    {
        if (_playerUnits)
        {
            foreach(CombatUnit combatUnit in playerUnits)
            {
                combatUnit.PotentialTargets = enemyUnits;
            }
        }
        else
        {
            foreach(CombatUnit combatUnit in enemyUnits)
            {
                combatUnit.PotentialTargets = playerUnits;
            }
        }
    }
}
