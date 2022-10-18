using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{

    public float hunger = 10f;
    public GameObject hungerBarUI;
    public Slider hungerSlider;

    void Start() {
        hungerSlider.value = CalculateHunger();
    }

    void Update() {
        hungerSlider.value = CalculateHunger();

        hunger -= 0.0005f;
        
        if (hunger <= 0) {
            Destroy(gameObject);
        }
    }

    float CalculateHunger() {
        return hunger / 10;
    }
}
