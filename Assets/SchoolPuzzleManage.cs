using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolPuzzleManage : MonoBehaviour
{

    [SerializeField] public GameObject Generator1;
    [SerializeField] public GameObject Generator2;
    [SerializeField] public GameObject Generator3;

    [SerializeField] public GameObject Collectable;

    private bool Activated1 = false;
    private bool Activated2 = false;
    private bool Activated3 = false;

    private bool Stop = true;


    // Start is called before the first frame update
    void Start()
    {
        Activated1 = Generator1.GetComponent<SCR_YardColilsion>().TargetHit;
        Activated2 = Generator2.GetComponent<SCR_YardColilsion>().TargetHit;
        Activated3 = Generator3.GetComponent<SCR_YardColilsion>().TargetHit;

    }

    // Update is called once per frame
    void Update()
    {
        if (Activated1 && Activated2 && Activated3 && Stop)
        {
            EndPuzzleResult();
            Stop = false;
        }
        else
        {
            CheckGenerators();
        }
    }

    private void CheckGenerators()
    {
        Activated1 = Generator1.GetComponent<SCR_YardColilsion>().TargetHit;
        Activated2 = Generator2.GetComponent<SCR_YardColilsion>().TargetHit;
        Activated3 = Generator3.GetComponent<SCR_YardColilsion>().TargetHit;
    }

    private void EndPuzzleResult()
    {
        Debug.Log("Puzzle is over");
        GameObject Prize = Instantiate(Collectable);
        Prize.transform.position = new Vector3(transform.position.x,
            transform.position.y, transform.position.z);
    }
}
