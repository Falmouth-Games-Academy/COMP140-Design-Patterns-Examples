using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeChecker : MonoBehaviour
{
    [SerializeField]
    float attackRange=5.0f;

    [SerializeField]
    Transform target;

    bool attack = false;



    // Start is called before the first frame update
    void Start()
    {
        //Trigger off Coroutine
        StartCoroutine(CheckAttackRange());
    }

    float GetSquareDistance(Vector3 targetPosition)
    {
        return (transform.position - targetPosition).sqrMagnitude;
    }

    IEnumerator CheckAttackRange()
    {
        while (true)
        {
            if (target)
            {
                if (GetSquareDistance(target.transform.position) < (attackRange * attackRange))
                {
                    attack = true;
                }
                else
                {
                    attack = false;
                }
            }
            else
            {
                attack = false;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public bool InRangeToAttack()
    {
        return attack;
    }
}
