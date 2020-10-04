using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellPuzzle : Puzzle
{

    /// <summary>
    /// These variables are made for the ball launch and a spawn of collectabless
    /// </summary>
    public float launchForce; 
    public ParticleSystem wellEffect = null;
    public GameObject collectable;
    public GameObject CollectableSpawnLocation;
    private Vector3 CollectableSpawnPoint;

    private AudioSource FountainAudio;
    private bool DoOnce = true;
    
    // Start is called before the first frame update
    void Start()
    {
        CollectableSpawnPoint = transform.position;
        FountainAudio = GetComponent<AudioSource>();
    }


    // reads when the ball enters the collition box
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9f && DoOnce)// 9 is the ball layer
        {
            Debug.Log("Ball in well");
            //BallInWellEffect(other.gameObject);
            StartCoroutine(BallInWellEffect(other.gameObject));
            DoOnce = false;
        }
        else
        {
            Debug.Log("Ball not in well");
        }

    }


    // effect for when the ball enters the collision box is fired
    private IEnumerator BallInWellEffect(GameObject ball)
    {

        yield return new WaitForSeconds(2f);
        // audio effect

        FountainAudio.PlayDelayed(1f);
        // ball audio effect 
        ball.GetComponent<Rigidbody>().AddForce(CollectableSpawnLocation.transform.up * launchForce);
        wellEffect.Play();


        // insted of ball get the collectable
        //GameObject Prize = Instantiate(collectable);

       // Prize.transform.position = new Vector3(CollectableSpawnPoint.x, 
        //    CollectableSpawnPoint.y, CollectableSpawnPoint.z);

        //Prize.GetComponent<Rigidbody>().AddForce(CollectableSpawnLocation.transform.up * launchForce );

        AddCondition("well-puzzle-complete");
        TriggerAlerts("well-puzzle-complete");
    }



}
