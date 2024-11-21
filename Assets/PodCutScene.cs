using UnityEngine;
using UnityEngine.Events;

public class PodCutScene : MonoBehaviour
{
    public static PodCutScene instance;


    [SerializeField] float CutSceneZoom;
    [SerializeField] Vector3 CutScene_CamPosition;
    [SerializeField] float EndZoom;
    [SerializeField] Transform Player;

    [SerializeField] UnityEvent OnStart;
    [SerializeField] UnityEvent OnEnd;

    [SerializeField] float Delay;

    public bool OnCutScenen = true;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CameraControl.instance.SetCamPosition(CutScene_CamPosition, 0);
        CameraControl.instance.SetCamSize(CutSceneZoom, 0);
        OnStart.Invoke();
    }



    public void OnCutSceneEnd()
    {
        Invoke("End", Delay);
    }

    void End()
    {
        OnCutScenen = false;
        CameraControl.instance.SetCamSize(EndZoom, 0.5f);
        CameraBehaviouerControll.instance.LookAtTarget(Player);
        CameraBehaviouerControll.instance.LookAtTarget(null);

        OnEnd.Invoke();
    }

}
