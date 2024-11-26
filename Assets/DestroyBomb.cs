using UnityEngine;

public class DestroyBomb : MonoBehaviour
{
    [SerializeField] float Time;

    void Start()
    {
        Invoke("DestorySelf", Time);
    }

    public void DestorySelf()
    {
        Destroy(this.gameObject);
    }

}
