using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour {

    private AIStatus aiStatus;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        aiStatus = GetComponent<AIStatus>();


    }
	
	// Update is called once per frame
	void Update () {
       animator.SetBool("IsDead", aiStatus.IsDead);
       animator.SetBool("IsTracking", aiStatus.IsWalking);
    }
    
    public void Attack1()
    {
        animator.SetTrigger("attackTrigger1");
    }

    public void Attack2()
    {
        animator.SetTrigger("attackTrigger2");
    }

    public void TookHitOnHead()
    {
        animator.SetTrigger("hitTrigger1");
    }

    public void TookHitHardly()
    {
        animator.SetTrigger("hitTrigger2");
    }
}
