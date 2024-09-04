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
        Target.TakeDamage(50);
    }

    public void EndAction()
    {
        EntityTurn.OnActionEnd();

    }
}
