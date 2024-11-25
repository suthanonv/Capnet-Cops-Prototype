using UnityEngine;

public class Pod : Health
{
    public AudioSource audioSource;
    public AudioClip openning;
    //private bool Used = false;
    public override void Died()
    {
        //if (Used) return;
        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;


        audioSource.PlayOneShot(openning);
        this.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Open");



        //Used = true;
        //Destroy(this.gameObject);
    }

    private void Update()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.gameObject.GetComponent<Character>().characterTile.GetComponent<Collider>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<Character>().characterTile.GetComponent<Collider>().enabled = true;

        }
    }
    public override void OnDied()
    {

        Exploring.Instance.OnExploringComplete();
        PodStroingScript.instance.CollecedPod += 1;
        this.GetComponent<EntityTeam>().EntityTeamSide = Team.Pod;
        Destroy(gameObject);
    }
}
