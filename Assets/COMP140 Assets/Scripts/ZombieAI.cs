using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(WaypointController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(AttackRangeChecker))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class ZombieAI : MonoBehaviour
{
    //Enum for AI States, we need to add a new entry here and a function to handle it
    public enum AIState
    {
        Patrol,
        Chase,
        Attack,
        Dead

    }

    //This is the current AI State
    [SerializeField]
    AIState currentAIState=AIState.Patrol;

    WaypointController waypointController;
    Animator zombieAnimator;
    EnemySensor sensor;
    AICharacterControl characterControl;
    AttackRangeChecker attackRangeChecker;
    ThirdPersonCharacter thirdPersonCharacter;

    void Start()
    {
        waypointController = GetComponent<WaypointController>();
        zombieAnimator = GetComponent<Animator>();
        sensor = GetComponent<EnemySensor>();
        characterControl = GetComponent<AICharacterControl>();
        attackRangeChecker = GetComponent<AttackRangeChecker>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();

        attackRangeChecker.enabled = false;

        AnimationClip[] animationClips=zombieAnimator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in animationClips)
        {
            if (clip.name=="Zombie Attack")
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = "AttackAnimationComplete";
                animationEvent.time = 1.13f;
                clip.AddEvent(animationEvent);
            }
            if (clip.name=="Zombie Death")
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = "DeathAnimationComplete";
                animationEvent.time = clip.length;
                clip.AddEvent(animationEvent);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current AI State is " + currentAIState);
        //Find the current state and call the function to handle the state
        switch (currentAIState)
        {
            case AIState.Patrol:
            {
                    OnPatrol();
                    break;
            }
            case AIState.Attack:
            {
                    OnAttack();
                    break;
            }
            case AIState.Chase:
            {
                    OnChase();
                    break;
            }
            case AIState.Dead:
            {
                    OnDead();
                    break;
            }
        }
    }

    void OnPatrol()
    {
        //Check to see if we have sensed a target, and move into chase
        if (sensor.HasDetectedEnemy())
        {
            Debug.Log("Move to Chase");
            characterControl.SetTarget(sensor.TargetedEnemy());
            currentAIState = AIState.Chase;
            sensor.enabled = false;
            waypointController.enabled = false;

            attackRangeChecker.enabled = true;
            attackRangeChecker.SetTarget(sensor.TargetedEnemy());
        }
    }

    void OnAttack()
    {
        //Attack
        zombieAnimator.SetBool("Attack", true);

        if (!attackRangeChecker.InRangeToAttack())
        {
            zombieAnimator.SetBool("Attack", false);
            currentAIState = AIState.Chase;
            thirdPersonCharacter.enabled = true;
        }

        //give some damage to our target and chjeck if its dead
        //If it is dead then go back to patrol


    }

    void OnChase()
    {
        characterControl.SetTarget(sensor.TargetedEnemy());
        //Are we close enough to attack?
        if (attackRangeChecker.InRangeToAttack())
        {
            currentAIState = AIState.Attack;
        }
    }

    void OnDead()
    {
        //Have we lost our health
    }

    public void DeathAnimationComplete()
    {

    }

    public void AttackAnimationComplete()
    {
        Transform enemy = sensor.TargetedEnemy();
        if (enemy)
        {
            Health enemyHealth = enemy.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(50.0f);
            if (enemyHealth.IsDead())
            {
                currentAIState = AIState.Patrol;
                waypointController.enabled = true;
                sensor.enabled = true;
            }
        }
        else
        {
            currentAIState = AIState.Patrol;
            waypointController.enabled = true;
            sensor.enabled = true;
        }
    }
}
