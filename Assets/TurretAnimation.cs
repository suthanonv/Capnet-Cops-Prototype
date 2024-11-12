using UnityEngine;

public class TurretAnimation : AnimationControll
{
    public override void Attacking()
    {
        if (Target != null)
        {
            ShowMoveingRange.instance.CloseMovingRangeVisual();
            this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
            Debug.Log(this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint);
            Target.gameObject.GetComponent<EnemyHealth>().RemoveBulletQuque();
            Target.TakeDamage(EntityTurn.Status.BaseDamage);
        }
    }
}
