using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotEyes : MonoBehaviour
{
    public float radius;
    public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool seesPlayer;

    void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    seesPlayer = true;
                else
                    seesPlayer = false;
            }
            else
                seesPlayer = false;
        }
        else if (seesPlayer)
            seesPlayer = false;
    }
}
