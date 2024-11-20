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
        TurnBaseSystem.instance.enemiesTurnSystems.List.RemoveAll(i => i == null);

        foreach (EntityTurnBehaviour i in TurnBaseSystem.instance.enemiesTurnSystems.List)
        {
            Vector3 thisTransform = new Vector3(this.transform.parent.position.x, 0, this.transform.parent.position.z);
            Vector3 EnemyTransform = new Vector3(i.transform.position.x, 0, i.transform.position.z);
            if (i.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
                if (Vector3.Distance(thisTransform, EnemyTransform) <= ActiveRange && health.CanbeTarget())
                {
                    return i.gameObject;
                }
        }
        return null;
    }

    private void Update()
    {
        if (EnemyToAttack != null)
        {
            if (EnemyToAttack.transform.GetChild(1).gameObject.GetComponent<AnimationControll>().IsPuasingSelf == false)
            {
                Vector3 thisTransform = new Vector3(this.transform.parent.position.x, 0, this.transform.parent.position.z);
                Vector3 EnemyTransform = new Vector3(EnemyToAttack.transform.position.x, 0, EnemyToAttack.transform.position.z);

                if (Vector3.Distance(thisTransform, EnemyTransform) <= ActiveRange && EnemyToAttack.GetComponent<EnemyHealth>().CanbeTarget() && !AttackAlready.Contains(EnemyToAttack))
                {
                    AttackAlready.Add(EnemyToAttack);
                    entity.SetTarget(EnemyToAttack);
                    TurretQuque.instance.AddingQuque(entity);
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
