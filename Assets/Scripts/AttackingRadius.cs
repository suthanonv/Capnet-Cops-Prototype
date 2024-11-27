using System.Collections.Generic;
using UnityEngine;

public class AttackingRadius : MonoBehaviour
{
    public GameObject EnemyToAttack;
    [SerializeField] float ActiveRange;
    [SerializeField] Transform Center;

    List<GameObject> AttackAlready = new List<GameObject>();

    SampleTurretTurn entity;

    private void Start()
    {
        entity = this.transform.parent.GetComponent<SampleTurretTurn>();
    }

    public void GetEnemy(GameObject Enemy)
    {
        EnemyToAttack = Enemy;
    }

    public void AttackAnyEnemy()
    {
        GameObject Enemy = GetEnemy();

        if (Enemy != null)
        {
            entity.SetTarget(Enemy);
            TurretQuque.instance.AddingQuque(entity);
        }
    }

    public GameObject GetEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Center.position, ActiveRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<EntityTurnBehaviour>(out EntityTurnBehaviour entityTurnBehaviour))
            {
                if (entityTurnBehaviour.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
                {
                    if (health.CanbeTarget())
                    {
                        return entityTurnBehaviour.gameObject;
                    }
                }
            }
        }
        return null;
    }

    private void Update()
    {
        if (EnemyToAttack != null)
        {
            if (EnemyToAttack.transform.GetChild(1).gameObject.GetComponent<AnimationControll>().IsPuasingSelf == false && EnemyToAttack.GetComponent<EnemyTurnBehaviour>().Spawned)
            {
                Collider[] hitColliders = Physics.OverlapSphere(Center.position, ActiveRange);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject == EnemyToAttack && !AttackAlready.Contains(EnemyToAttack))
                    {
                        if (EnemyToAttack.TryGetComponent<EnemyHealth>(out EnemyHealth health))
                        {
                            if (health.CanbeTarget())
                            {
                                AttackAlready.Add(EnemyToAttack);
                                entity.SetTarget(EnemyToAttack);
                                TurretQuque.instance.AddingQuque(entity);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Center != null)
        {
            Gizmos.color = Color.red; // Set Gizmos color for visualization
            Gizmos.DrawWireSphere(Center.transform.position, ActiveRange);
        }
    }
}
