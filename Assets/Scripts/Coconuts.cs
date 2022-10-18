using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconuts : MonoBehaviour
{
    public GameObject coconut;
    public bool coroutineRunning = false;

    void Start() {
        StartCoroutine(SpawnCoconuts());
    }

    public IEnumerator SpawnCoconuts() {
        int spawnTime = Random.Range(30, 75);
        coroutineRunning = true;
        while(true) {
            yield return new WaitForSeconds(spawnTime);
            Instantiate(coconut, transform.position, transform.rotation);
        }
        coroutineRunning = false;
    }
}
