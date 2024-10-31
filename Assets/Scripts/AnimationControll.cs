using UnityEngine;

public class AnimationControll : MonoBehaviour
{

    public Health Target { get; set; }

    EntityTurnBehaviour EntityTurn;

    private void Start()
    {
        EntityTurn = transform.parent.GetComponent<EntityTurnBehaviour>();
    }

    public void Attacking()
    {
        if (Target != null)
        {

            this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
            Target.TakeDamage(EntityTurn.Status.BaseDamage);
        }
    }

    public void EndAction()
    {
        EntityTurn.OnActionEnd();

    }
}
