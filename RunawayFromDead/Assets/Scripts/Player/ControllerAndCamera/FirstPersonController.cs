﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
    public AudioClip[] walkSoundClips;
    public AudioClip jumpSoundClip;
    public AudioClip landSoundClip;

    public float runSpeed;
    public float walkSpeed;
    public float moveSoundInterval;
    public bool canRun;
    public bool isRun
    {
        get
        {
            return isRunning;
        }
    }
    public bool CanControl
    {
        get
        {
            return canControl;
        }
        set
        {
            canControl = value;
        }
    }


    private const float SROOT2 = 1.4f;
    private const float NORMAL = 1.0f;

    private AudioSource audioSource;
    private CharacterController cc;
    [Range(-1f, 1f)]
    private float verticalMove = 0f;
    [Range(-1f, 1f)]
    private float horizontalMove = 0f;
    private Vector3 moveAxis;
        
    private float moveFlow;
    private bool isRunning;
    private bool canControl;
    private float runTimeInterval;
    private float walkTimeInterval;



    public float vertical
    {
        get
        {
            return Mathf.Clamp(verticalMove * 100f, -1f, 1f);
        }
    }

    public float horizontal
    {
        get
        {
            return Mathf.Clamp(horizontalMove * 100f, -1f, 1f);
        }
    }

    // Use this for initialization
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        walkTimeInterval = moveSoundInterval;
        runTimeInterval = moveSoundInterval / 3f;
        canControl = true;
    }

    private void FixedUpdate()
    {
        if (canControl)
        {
            GetInput();
            Moving();
            CycleWalkAndJumpSound();
            ApplyGravity();
        }
    }

    private void GetInput()
    {
        if (InputManager.GetKey(InputNames.moveForward))
        {
            verticalMove = 1f * Time.deltaTime;
        }
        else if (InputManager.GetKey(InputNames.moveBackward))
        {
            verticalMove = -1f * Time.deltaTime;
        }
        else
        {
            verticalMove = 0f;
        }

        if (InputManager.GetKey(InputNames.moveLeft))
        {
            horizontalMove = -1f * Time.deltaTime;
        }
        else if (InputManager.GetKey(InputNames.moveRight))
        {
            horizontalMove = 1f * Time.deltaTime;
        }
        else
        {
            horizontalMove = 0f;
        }

        if (InputManager.GetKey(InputNames.run) && canRun && verticalMove > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

    }

    private void ApplyGravity()
    {
        moveAxis = Vector3.zero;
            
        if (cc.isGrounded == false)
        {
            //Add our gravity Vecotr
            moveAxis += Physics.gravity;
        }
            
        cc.Move(moveAxis * Time.deltaTime);
    }

    private void Moving()
    {
        float squareRoot = (verticalMove != 0 && horizontalMove != 0)? SROOT2 : NORMAL;
        
        if (isRunning)
        {
            moveAxis = transform.forward * verticalMove / squareRoot * runSpeed + transform.right * horizontalMove / squareRoot * runSpeed;
        }
        else
        {
            moveAxis = transform.forward * verticalMove / squareRoot * walkSpeed + transform.right* horizontalMove / squareRoot * walkSpeed;
        }
        moveAxis.y = 0f;

        cc.Move(moveAxis);
    }

    private void CycleWalkAndJumpSound()
    {

        moveSoundInterval = ((isRunning) ? runTimeInterval : walkTimeInterval);

        if (Time.time - moveFlow > moveSoundInterval && (verticalMove * verticalMove > Mathf.Epsilon || horizontalMove * horizontalMove > Mathf.Epsilon))
        {
            moveFlow = Time.time;
            PlayWalkSound();
        }
    }
        

    private void PlayWalkSound()
    {
        int index = Random.Range(0, walkSoundClips.Length);
        audioSource.clip = walkSoundClips[index];
        audioSource.Play();
    }


    private void PlayJumpSound()
    {
        audioSource.clip = jumpSoundClip;
        audioSource.Play();
    }
        
    private void PlayLandSound()
    {
        audioSource.clip = landSoundClip;
        audioSource.Play();
    }
}
