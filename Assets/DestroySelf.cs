using UnityEngine;
using UnityEngine.Events;
public class DestroySelf : MonoBehaviour
{
    public TurretAnimation turret;

    public UnityEvent OnStartEvent;

    void Start()
    {
        OnStartEvent.Invoke();
    }

    public void InvokeEndTurn()
    {
        Invoke("EndTurn", 0.5f);
    }

    public void DestroyInvoke()
    {
        Invoke("DesInvoke", 1);
    }


    public void EndTurn()
    {
        turret.UnPauseCharacter();
        turret.EndAction();
    }
    public void DesInvoke()
    {
        Destroy(this.gameObject);
    }


}
