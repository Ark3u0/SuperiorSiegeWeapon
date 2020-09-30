using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Tree_Interact : MonoBehaviour
{
    [SerializeField] public bool HasCollectable = false;

    [SerializeField] public float launchForce;
    [SerializeField] ParticleSystem treeEffect = null;
    [SerializeField] public GameObject collectable;
    [SerializeField] public GameObject collectableSpawnPoint;


    private Vector3 CollectableSpawnPoint;


    private Vector3 bounceVector;
    // Start is called before the first frame update
    void Start()
    {
        CollectableSpawnPoint = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9f)// 9 is the ball layer
        {
            if (HasCollectable)
            {
                StartCoroutine(CollectabelEffect());
            }
            bounceVector.x = collision.contacts[0].normal.x;
            bounceVector.y = collision.contacts[0].normal.y;
            bounceVector.z = collision.contacts[0].normal.z;

            Debug.Log("Ball in well");
            //BallInWellEffect(other.gameObject);
            //StartCoroutine(BallInWellEffect(collision.gameObject));
            HitTreeEffect(collision.gameObject);
        }
        else
        {
            Debug.Log("Ball not in well");
        }
    }

    private void HitTreeEffect(GameObject Ball)
    {
        StartCoroutine(PulsingTree());
        treeEffect.Play();
        //need to play audio
        Ball.GetComponent<Rigidbody>().AddForce(bounceVector * launchForce);

        Debug.Log("Ball bounce");
    }

    private IEnumerator CollectabelEffect()
    {
        yield return new WaitForSeconds(3f);
        // spawn the collectable 
        Debug.Log("Spawn Collectable");
        GameObject treeDrop = Instantiate(collectable);
        treeDrop.transform.position = new Vector3(collectableSpawnPoint.transform.position.x,
            collectableSpawnPoint.transform.position.y, collectableSpawnPoint.transform.position.z);

    }

    private IEnumerator PulsingTree()
    {

        //Grow
        for (float i = 0f; i<= 1f; i+= 0.2f)
        {
            transform.localScale = new Vector3(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x + 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y + 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.z, transform.localScale.z + 0.025f, Mathf.SmoothStep(0f, 1f, i)))
                );
                yield return new WaitForSeconds(0.015f);
        }

        //shrink
        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector3(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x - 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y - 0.025f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.z, transform.localScale.z - 0.025f, Mathf.SmoothStep(0f, 1f, i)))
                );
            yield return new WaitForSeconds(0.015f);
        }
    }
}
