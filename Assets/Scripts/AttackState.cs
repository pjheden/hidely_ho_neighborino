﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class AttackState : State
{
    /*
     * Go to player
     * Throw away player
     */
    private bool playerThrown;
    [SerializeField] private float radiusTreshold;
    [SerializeField] private float escapeTreshold;
    [SerializeField] private float moveSpeed;
    public Animator anim;

    [Header("Sounds")]
    public AudioClip walkClip;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerThrown = false;
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        if (playerThrown)
        {
            ResetValues();
            //Transition
            //PatrolState state = GetComponent<PatrolState>();
            //controller.TransitionToState(state);

            SleepState state = GetComponent<SleepState>();
            state.StartAnimation();
            controller.TransitionToState(state);
        }
    }
    protected override void DoActions(FiniteStateMachine controller)
    {
        // MOVEMENT
        Transform target = controller.doll.transform;
        // Calculate direction
        Vector3 heading = target.position - controller.kid.transform.position;
        // if target reached, pick up doll
        float distance = heading.magnitude;
        if (distance <= radiusTreshold)
        {
            // pick up doll
            controller.doll.transform.position = new Vector3(0, 1, 0);
            // Add time punishment
            controller.gameController.AddTime(-10.0f);

            playerThrown = true;
            return;
        }else if(distance > escapeTreshold || controller.doll.GetComponent<PlayerController>().IsSafe())
        {
            playerThrown = true;
            return;
        }
        // move to target
        var direction = heading / distance;
        controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = walkClip;
            audioSource.Play();
        }

        // rotate to target
        controller.kid.transform.LookAt(target.position);
    }

    private void ResetValues()
    {
        playerThrown = false;
        anim.SetBool("attacking", false);
    }

    public void StartAnimation()
    {
        anim.SetBool("attacking", true);
    }

    public override string GetStateName()
    {
        return "AttackState";
    }
}
