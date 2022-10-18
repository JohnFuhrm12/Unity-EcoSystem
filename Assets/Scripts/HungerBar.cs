using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{

    public float hunger = 10f;
    public GameObject hungerBarUI;
    public Slider hungerSlider;

    public bool isStarving = false;

    void Start() {
        hungerSlider.value = CalculateHunger();
    }

    void Update() {
        hungerSlider.value = CalculateHunger();

        hunger -= 0.0003f;
        
        if (hunger <= 0) {
            Destroy(gameObject);
        }

        if (hunger <= 3) {
            isStarving = true;
        }
        if (hunger > 3) {
            isStarving = false;
        }
    }

    float CalculateHunger() {
        return hunger / 10;
    }
}
