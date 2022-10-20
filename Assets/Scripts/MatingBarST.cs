using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatingBarST : MonoBehaviour
{
    public float mating = 3f;
    public GameObject matingBarUI;
    public Slider matingSlider;

    SeaTurtle seaTurtleScript;
    HungerBar hungerScript;

    void Start() {
        matingSlider.value = CalculateMating();
        hungerScript = GetComponent<HungerBar>();
        seaTurtleScript = GetComponent<SeaTurtle>();
    }

    void Update() {
        matingSlider.value = CalculateMating();

        mating -= 0.0005f;

        if (mating <= 4 && hungerScript.isStarving == false) {
            seaTurtleScript.findMate();
        }
        
        if (mating > 4) {
            seaTurtleScript.searchingMate = false;
        }
    }

    float CalculateMating() {
        return mating / 10;
    }
}
