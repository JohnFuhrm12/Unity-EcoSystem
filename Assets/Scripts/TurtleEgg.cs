using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEgg : MonoBehaviour
{
    public GameObject seaTurtle;
    public float waterLevel = 51f;

    void Start() {
        StartCoroutine(CreateBaby());
    }

    public IEnumerator CreateBaby() {
        int spawn = 0;
        yield return new WaitForSeconds(10);
        if (spawn == 0 && transform.position.y > waterLevel) {
            spawn = 1;
            yield return new WaitForSeconds(10);
            Instantiate(seaTurtle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
