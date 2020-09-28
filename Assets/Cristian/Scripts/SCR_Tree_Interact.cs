using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Tree_Interact : MonoBehaviour
{
    [SerializeField] public bool HasCollectable = false;

    [SerializeField] public float launchForce;
    [SerializeField] ParticleSystem treeEffect = null;
    [SerializeField] public GameObject collectable;
    [SerializeField] public GameObject collectableSpawnVector;


    private Vector3 CollectableSpawnPoint;

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


            Debug.Log("Ball in well");
            //BallInWellEffect(other.gameObject);
            //StartCoroutine(BallInWellEffect(collision.gameObject));
            HitTreeEffect();
        }
        else
        {
            Debug.Log("Ball not in well");
        }
    }

    private void HitTreeEffect()
    {
        treeEffect.Play();
    }

    private IEnumerator CollectabelEffect()
    {
        yield return new WaitForSeconds(3f);
        // spawn the collectable 

    }
}
