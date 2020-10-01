using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePuzzle : Puzzle
{
    public bool HasCollectable = false;
    public float launchForce;
    public ParticleSystem treeEffect = null;
    public GameObject collectable;
    public GameObject collectableSpawnPoint;
    private Vector3 CollectableSpawnPoint;


    private Vector3 bounceVector;

    // Start is called before the first frame update
    void Start()
    {
        CollectableSpawnPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9f)// 9 is the ball layer
        {
            if (HasCollectable)
            {
                AddCondition("tree-puzzle-complete");
                StartCoroutine(CollectableEffect());
            }
            bounceVector.x = collision.contacts[0].normal.x;
            bounceVector.y = collision.contacts[0].normal.y;
            bounceVector.z = collision.contacts[0].normal.z;

            Debug.Log("Ball in tree");
            HitTreeEffect(collision.gameObject);
        }
        else
        {
            Debug.Log("Ball not in tree");
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

    private IEnumerator CollectableEffect()
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
