using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconuts : MonoBehaviour
{
    public GameObject coconut;
    public bool coroutineRunning = false;

    public float spawnDistance = 300f;

    void Start() {
        StartCoroutine(SpawnCoconuts());
        spawnDistance = 300f;
    }

    void Update() {
        if (coroutineRunning == false) {
            StartCoroutine(SpawnCoconuts());
        }
    }

    public GameObject FindClosestMaleCrab() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Male");
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

    public GameObject FindClosestFemaleCrab() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Female");
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

    public IEnumerator SpawnCoconuts() {
        int spawnTime = Random.Range(30, 75);

        // Find Distance from Closest Crabs
        GameObject ClosestMale = FindClosestMaleCrab();
        float distM = Vector3.Distance(ClosestMale.transform.position, transform.position);
        GameObject ClosestFemale = FindClosestFemaleCrab();
        float distF = Vector3.Distance(ClosestFemale.transform.position, transform.position);

        // Only spawn a coconut if the distance of nearest crabs < spawnDistance
        coroutineRunning = true;
        while(true) {
            yield return new WaitForSeconds(spawnTime);
            if (distM < spawnDistance || distF < spawnDistance) {
                Instantiate(coconut, transform.position, transform.rotation); 
            }      
        }
        coroutineRunning = false;
    }
}
