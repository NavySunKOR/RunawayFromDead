using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleAI : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform tr;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        tr = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(tr.position);
	}
}
