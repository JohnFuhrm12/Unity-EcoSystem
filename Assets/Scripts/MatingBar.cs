using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MatingBar : MonoBehaviour
{
    public float mating = 3f;
    public GameObject matingBarUI;
    public Slider matingSlider;

    Crab crabScript;

    public bool mated = false;

    void Start() {
        matingSlider.value = CalculateMating();
        crabScript = GetComponent<Crab>();
    }

    void Update() {
        matingSlider.value = CalculateMating();

        mating -= 0.0005f;

        if (mating <= 4 && mated == false) {
            crabScript.findMate();
        }
        
        if (mating > 4) {
            crabScript.searchingMate = false;
        }
    }

    float CalculateMating() {
        return mating / 10;
    }
}
