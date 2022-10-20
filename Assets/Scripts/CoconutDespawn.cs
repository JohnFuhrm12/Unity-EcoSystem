using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutDespawn : MonoBehaviour
{
    void Start() {
        int TargetLayer = LayerMask.NameToLayer("Target");
        gameObject.layer = TargetLayer;
        gameObject.tag = "Food";
        StartCoroutine(Despawn());
    }

    public IEnumerator Despawn() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}
