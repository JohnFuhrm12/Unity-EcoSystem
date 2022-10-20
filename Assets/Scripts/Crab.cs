using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public bool isWalking = false;
    public bool foodNearby = false;
    public bool isEating = false;
    public bool searchingMate = false;
    public bool mating = false;
    public Animator CrabAnim;

    public float movementSpeed = 3.5f;
    public float runSpeed = 8f;

    public float growthSpeed = 0.2f;

    FOV fovScript;
    HungerBar hungerScript;
    MatingBar matingScript;

    public float delay = 20f;

    public float moveSpeed = 2.5f;
    public float rotSpeed = 100f;

    public bool isWandering = false;
    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;

    private GameObject Food;
    public GameObject crab;

    private Vector3 scaleChange;

    void Start() {
        isWalking = true;
        startWalkingAnim();
        fovScript = GetComponent<FOV>();
        hungerScript = GetComponent<HungerBar>();
        matingScript = GetComponent<MatingBar>();
        Food = FindClosestFood();

        StartCoroutine(Wander());

        matingScript.mating = 10f;
        hungerScript.hunger = 10f;

        growthSpeed = 0.2f;

        scaleChange = new Vector3(0.01f, 0.01f, 0.01f);

        int gender = Random.Range(1, 100);
        if (gender < 50) {
            gameObject.tag = "Male";
        }
        if (gender > 50) {
            gameObject.tag = "Female";
        }
    }

    void Update() {
        GameObject[] CrabCountMale;
        GameObject[] CrabCountFemale;

        Vector3 adultSize = new Vector3(1.031705f, 1.031705f, 1.031705f);

        CrabCountMale = GameObject.FindGameObjectsWithTag("Male");
        CrabCountFemale = GameObject.FindGameObjectsWithTag("Female");
        if (isWalking && hungerScript.isStarving == false) {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }
        if (isWalking && hungerScript.isStarving == true) {
            transform.position += transform.right * runSpeed * Time.deltaTime;
        }
        TargetFood();
        if (isWandering == false && isEating == false) {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        Food = FindClosestFood();
        //Debug.Log(CrabCountMale.Length + CrabCountFemale.Length);

        if (gameObject.transform.localScale.y < 1.031705f) {
            gameObject.transform.localScale += scaleChange * growthSpeed * Time.deltaTime;
        }
    }

    void startWalkingAnim() {
        CrabAnim.SetTrigger("StartWalking");
    }

    void TargetFood() {
        GameObject FoodItem = FindClosestFood();
        float dist = Vector3.Distance(FoodItem.transform.position, transform.position);

        if (fovScript.canSeeFood && isEating == false && searchingMate == false && hungerScript.hunger < 8) {
            if (dist < 15) {
                isWalking = false;
                StopCoroutine(Wander());
                transform.LookAt(FoodItem.transform.position);
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }
            if (dist < 5) {
                CrabAnim.SetTrigger("StopWalking");
                isEating = true;
            }
            if (isEating) {
                CrabAnim.SetTrigger("StartEating");
                hungerScript.hunger += 5;
            }
        }
    }

    public void FixedUpdate() {
        GameObject FoodItem = FindClosestFood();

        if (delay > 0f && isEating == true) {
            delay -= Time.fixedDeltaTime;

            if (delay <= 0f && isEating == true) {
                CrabAnim.SetTrigger("StopEating");
                isEating = false;
                if (FoodItem != null) {
                    Destroy(FoodItem);
                    FoodItem = FindClosestFood();
                    delay = 20f;
                }
                CrabAnim.SetTrigger("StartWalking");
                isWalking = true;
                StartCoroutine(Wander());
            }
        }
    }

     IEnumerator Wander() {
        int rotTime = Random.Range(1, 2);
        int rotateWait = Random.Range(1, 2);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 2);
        int walkTime = Random.Range(3, 10);

        isWandering = true;

    if (isWandering && isEating == false) {
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1 && isEating == false)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2 && isEating == false)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }
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

    public GameObject FindClosestMate() {
        GameObject[] gos = null;
        if (gameObject.tag == "Male") {
            gos = GameObject.FindGameObjectsWithTag("Female");
        }
        if (gameObject.tag == "Female") {
            gos = GameObject.FindGameObjectsWithTag("Male");
        }
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

    public IEnumerator CreateBaby() {
        int spawn = 0;
        yield return new WaitForSeconds(10);
        if (mating && gameObject.tag == "Female") {
            matingScript.mating = 10f;
            if (spawn == 0) {
                spawn = 1;
                Instantiate(crab, transform.position * 1.02f, transform.rotation);
            }
            mating = false;
        }
        if (mating && gameObject.tag == "Male") {
            matingScript.mating = 10f;
        }
    }
    

    public void findMate() {
        GameObject ClosestMate = FindClosestMate();
        searchingMate = true;
        float dist = Vector3.Distance(ClosestMate.transform.position, transform.position);
        transform.LookAt(ClosestMate.transform.position);
        if (dist > 10) {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
            mating = true;
        }
        if (dist <= 10 && mating)  {
            StartCoroutine(CreateBaby());
        }
        if (mating == false) {
            StopCoroutine(CreateBaby());
        }
    }
}
