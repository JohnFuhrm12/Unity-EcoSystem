using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float radius;
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeeFood;

    public GameObject FoodRef;

    void Start() {
        GameObject FoodRef = FindClosestFood();
    }

    void Update() {
        FieldOfViewCheck();
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
                    canSeeFood = true;
                else
                    canSeeFood = false;
            }
            else
                canSeeFood = false;
        }
        else if (canSeeFood)
            canSeeFood = false;
    }

    public GameObject FindClosestFood() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Food");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
