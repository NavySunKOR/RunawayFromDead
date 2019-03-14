using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStatus : MonoBehaviour {

    public GameObject bloodEffect;
    public float attackInterval = 2.0f;
    public int damage = 20;
    public bool IsEnd
    {
        get
        {
           return isOver;
        }
        set
        {
            isOver = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            isDead = value;
        }
    }

    public bool IsWalking
    {
        get
        {
            return isWalking;
        }
        set
        {
            isWalking = value;
        }
    }



    private NavMeshAgent agent;
    private Transform playerTr;
    private AIAnimator animator;
    private int health;
    private bool isDead;
    private bool isSpotted;
    private bool isWalking;
    private bool isOver;
    private float attackTimer;
    private float spotRange;
    private float attackRange;
    

	// Use this for initialization
	void Start () {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<AIAnimator>();
        isDead = false;
        isSpotted = false;
        isOver = false;
        health = 100;
        spotRange = 7f;
        attackRange = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        if(!isOver)
        {
            if (!isDead)
            {
                if (health <= 0)
                {
                    isDead = true;
                }

                if(isSpotted)
                {
                    attackTimer += Time.deltaTime;

                    if (Vector3.Distance(transform.position, playerTr.position) < attackRange)
                    {
                        if(attackTimer > attackInterval)
                        {
                            attackTimer = 0f;
                            Attack();
                        }
                    }
                    else
                    {
                        agent.SetDestination(playerTr.position);
                        agent.isStopped = false;
                        isWalking = true;
                    }
                }
                else
                {
                    if(Vector3.Distance(transform.position,playerTr.position) < spotRange)
                    {
                        isSpotted = true;
                    }
                }
                
            }
            else
            {
                isWalking = false;
                isOver = true;
                Destroy(gameObject, 3f);
            }
        }
	}

    void Attack()
    {
        attackTimer = 0f;
        int rng = Random.Range(1, 3);
        if (rng == 1)
        {
            animator.Attack1();
        }
        else
        {
            animator.Attack2();
        }
    }

    public void TookHit(Transform collTr, int damage)
    {
        if(collTr.CompareTag("Head"))
        {
            health -= (damage * 2);
            animator.TookHitOnHead();
        }
        else if(collTr.CompareTag("Body"))
        {
            health -= damage;
            if(damage > 30)
            {
                animator.TookHitHardly();
            }
        }
        else if(collTr.CompareTag("Leg") || collTr.CompareTag("Arm"))
        {
            health -= (int)damage / 2;
        }
    }
}
