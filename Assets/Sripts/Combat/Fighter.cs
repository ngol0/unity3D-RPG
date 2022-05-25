using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using UnityEngine.AI;

namespace RPG.Combat
{ 

    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        Health target;
        Mover moving;
        float timeSinceLastAttack = 0;
        float weaponDamage = 30f;

        private void Start()
        {
            moving = GetComponent<Mover>();
        }


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            //if no combat target- keep moving
            if (target == null) return;
            if (target.IsDead()) return;

            //move to enemy
            if (!isInRange())
            {
                moving.MoveTo(target.transform.position);
            }
            //when moving close to range start attacking animation and stop moving
            else
            {
                moving.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            //slowing down attack animation
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
            
        }

        private bool isInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        //set target position
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
                
        }

        public void Cancel()
        {
            target = null;
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //animation event
        void Hit()
        {
            target.TakeDamge(weaponDamage);
        }
        
    }
}
