using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Well_Interact : MonoBehaviour
{

    /// <summary>
    /// These variables are made for the ball launch and a spawn of collectabless
    /// </summary>
    [SerializeField] public float launchForce; 
    [SerializeField] ParticleSystem wellEffect = null;
    [SerializeField] public GameObject collectable;
    [SerializeField] public GameObject CollectableSpawnLocation;


    private Vector3 CollectableSpawnPoint; 


    
    // Start is called before the first frame update
    void Start()
    {
        CollectableSpawnPoint = transform.position;
    }


    // reads when the ball enters the collition box
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9f)// 9 is the ball layer
        {
            Debug.Log("Ball in well");
            //BallInWellEffect(other.gameObject);
            StartCoroutine(BallInWellEffect(other.gameObject));
        }
        else
        {
            Debug.Log("Ball not in well");
        }

    }


    // effect for when the ball enters the collision box is fired
    private IEnumerator BallInWellEffect(GameObject ball)
    {

        yield return new WaitForSeconds(3f);
        // audio effect


        // ball audio effect 
        ball.GetComponent<Rigidbody>().AddForce(Vector3.up * launchForce);
        wellEffect.Play();


        // insted of ball get the collectable
        GameObject Prize = Instantiate(collectable);

        Prize.transform.position = new Vector3(CollectableSpawnPoint.x, 
            CollectableSpawnPoint.y, CollectableSpawnPoint.z);

        Prize.GetComponent<Rigidbody>().AddForce(CollectableSpawnLocation.transform.up * launchForce );

    }



}
