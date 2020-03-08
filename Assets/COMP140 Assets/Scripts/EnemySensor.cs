using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [SerializeField]
    float detectionRange = 10.0f;

    bool detectedEnemy = false;

    [SerializeField]
    LayerMask layerMask;

    Transform enemyTransform=null;

    void Start()
    {
        detectedEnemy = false;
        enemyTransform = null;
        //Trigger off Coroutine
        StartCoroutine(CheckForEnemy());
    }

    public bool HasDetectedEnemy()
    {
        return detectedEnemy;
    }

    public Transform TargetedEnemy()
    {
        return enemyTransform;
    }


    IEnumerator CheckForEnemy()
    {
        while(true)
        {
            Debug.Log("Checking for Enemy");
            Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange, layerMask);
            if (detectedObjects.Length>0)
            {
                Debug.Log("Found Enemies");
                //Sort the array
                System.Array.Sort<Collider>(detectedObjects, delegate (Collider x, Collider y)
                 {
                     float distanceX = (transform.position - x.transform.position).sqrMagnitude;
                     float distanceY = (transform.position - y.transform.position).sqrMagnitude;

                     return distanceX.CompareTo(distanceY);
                 });

                enemyTransform=detectedObjects[0].transform;
                if (enemyTransform)
                {
                    Debug.Log("Found actual target "+enemyTransform.gameObject.name);
                    detectedEnemy = true;
                }

            }
            else
            {
                enemyTransform = null;
                detectedEnemy = false;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
