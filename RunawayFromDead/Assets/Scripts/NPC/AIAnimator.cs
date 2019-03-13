using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour {


    //0 - idle, 1 - attack1 , 2 - attack2 , 3 - tookHit1, 4 - tookHit2 , 5 - death
    public AudioClip[] idleClips;
    public AudioClip[] attackClips;
    public AudioClip[] tookHitClips;
    public AudioClip[] deadClips;

    private AudioSource audioSource;
    private AIStatus aiStatus;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        aiStatus = GetComponent<AIStatus>();
        audioSource = GetComponent<AudioSource>();
        int random = Random.Range(0, idleClips.Length);
        audioSource.clip = idleClips[random];
        audioSource.Play();

    }
	
	// Update is called once per frame
	void Update () {
        if(aiStatus.IsDead)
        {
            int random = Random.Range(0, deadClips.Length);
            audioSource.clip = deadClips[random];
            audioSource.Play();
            audioSource.loop = false;
        }
        else
        {
            if(!audioSource.isPlaying)
            {
                int random = Random.Range(0, idleClips.Length);
                audioSource.clip = idleClips[random];
                audioSource.Play();
            }
        }
       animator.SetBool("IsDead", aiStatus.IsDead);
       animator.SetBool("IsTracking", aiStatus.IsWalking);
    }
    
    public void Attack1()
    {
        int random = Random.Range(0, attackClips.Length);
        audioSource.PlayOneShot(attackClips[random]);
        animator.SetTrigger("attackTrigger1");
    }

    public void Attack2()
    {
        int random = Random.Range(0, attackClips.Length);
        audioSource.PlayOneShot(attackClips[random]);
        animator.SetTrigger("attackTrigger2");
    }

    public void TookHitOnHead()
    {
        int random = Random.Range(0, tookHitClips.Length);
        audioSource.PlayOneShot(tookHitClips[random]);
        animator.SetTrigger("hitTrigger1");
    }

    public void TookHitHardly()
    {
        int random = Random.Range(0, tookHitClips.Length);
        audioSource.PlayOneShot(tookHitClips[random]);
        animator.SetTrigger("hitTrigger2");
    }
}
