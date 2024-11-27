using UnityEngine;

public class EnemyPlayingSound : MonoBehaviour
{
    public void PlayRoarSound()
    {
        this.transform.parent.gameObject.GetComponent<EnemyTurnBehaviour>().playRoarSound(true);
    }
}
