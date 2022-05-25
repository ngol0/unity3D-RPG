using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent player;
        Animator anim;

        private void Start()
        {
            player = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }


        public void StartMoving(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            player.destination = destination;
            player.isStopped = false;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = player.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            anim.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void Cancel()
        {
            player.isStopped = true;
        }

    }
}