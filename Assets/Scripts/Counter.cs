using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public float Crabs;
    public float Turtles;

    void Update() {
        GameObject[] CrabCountMale;
        GameObject[] CrabCountFemale;

        CrabCountMale = GameObject.FindGameObjectsWithTag("Male");
        CrabCountFemale = GameObject.FindGameObjectsWithTag("Female");

        GameObject[] TurtleCountMale;
        GameObject[] TurtleCountFemale;

        TurtleCountMale = GameObject.FindGameObjectsWithTag("MaleST");
        TurtleCountFemale = GameObject.FindGameObjectsWithTag("FemaleST");

        Crabs = CrabCountMale.Length + CrabCountFemale.Length;
        Turtles = TurtleCountMale.Length + TurtleCountFemale.Length;
    }
}
