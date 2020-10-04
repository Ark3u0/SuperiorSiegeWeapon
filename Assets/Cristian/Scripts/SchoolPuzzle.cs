using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolPuzzle : Puzzle
{

    public GameObject Generator1;
    public GameObject Generator2;
    public GameObject Generator3;

    public GameObject Collectable;

    private bool Activated1 = false;
    private bool Activated2 = false;
    private bool Activated3 = false;

    private bool Stop = true;


    // Start is called before the first frame update
    void Start()
    {
        Activated1 = Generator1.GetComponent<YardCollision>().TargetHit;
        Activated2 = Generator2.GetComponent<YardCollision>().TargetHit;
        Activated3 = Generator3.GetComponent<YardCollision>().TargetHit;

    }

    // Update is called once per frame
    void Update()
    {
        if (Activated1 && Activated2 && Activated3 && Stop)
        {
            AddCondition("school-puzzle-complete");
            EndPuzzleResult();
            TriggerAlerts("school-puzzle-complete");
            Stop = false;
        }
        else
        {
            CheckGenerators();
        }
    }

    private void CheckGenerators()
    {
        Activated1 = Generator1.GetComponent<YardCollision>().TargetHit;
        Activated2 = Generator2.GetComponent<YardCollision>().TargetHit;
        Activated3 = Generator3.GetComponent<YardCollision>().TargetHit;
        
        if (Activated1) {
            AddCondition("school-puzzle-1-partial");
        }
        if (Activated2) {
            AddCondition("school-puzzle-2-partial");
        }
        if (Activated3) {
            AddCondition("school-puzzle-3-partial");
        }
    }

    private void EndPuzzleResult()
    {
        Debug.Log("OOOOOOOOOOOOOPuzzle is over OOOOOOOOOOOOOOO");
        //GameObject Prize = Instantiate(Collectable);
        // Prize.transform.position = new Vector3(transform.position.x,
        //  transform.position.y, transform.position.z);

        //add audio effect for the win
        //change the lights and turn thme on

    }
}
