using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaTurtle : MonoBehaviour
{
    public bool isWalking = false;
    public bool isSwimming = false;
    public bool foodNearby = false;
    public bool isEating = false;
    public bool searchingMate = false;
    public bool mating = false;
    public Animator SeaTurtleAnim;

    public float walkSpeed = 3.5f;
    public float swimSpeed = 8f;

    FOV fovScript;
    HungerBar hungerScript;
    MatingBarST matingScript;

    public float delay = 2f;

    public float moveSpeed = 2.5f;
    public float rotSpeed = 100f;

    public float waterLevel = 51f;

    public bool isWandering = false;
    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;

    private GameObject Food;
    public GameObject seaTurtle;
    public GameObject egg;

    private Rigidbody rb;

    public bool hasEgg = false;
    public GameObject NestingSite;

    public bool isLayingEgg = false;
    public float growthSpeed = 0.2f;
    private Vector3 scaleChange;

    public GameObject HatchlingGoal;
    public bool returningToWater = false;

    void Start() {
        isSwimming = true;
        SeaTurtleAnim.SetTrigger("StartSwimming");
        fovScript = GetComponent<FOV>();
        hungerScript = GetComponent<HungerBar>();
        matingScript = GetComponent<MatingBarST>();
        rb = gameObject.GetComponent<Rigidbody>();
        Food = FindClosestFood();

        StartCoroutine(Wander());

        matingScript.mating = 10f;
        hungerScript.hunger = 10f;
        delay = 2f;

        growthSpeed = 0.2f;
        scaleChange = new Vector3(0.01f, 0.01f, 0.01f);

        NestingSite = GameObject.FindGameObjectWithTag("NestingSite");
        HatchlingGoal = GameObject.FindGameObjectWithTag("HatchlingGoal");
        egg = GameObject.FindGameObjectWithTag("Egg");

        int gender = Random.Range(1, 100);
        if (gender < 50) {
            gameObject.tag = "MaleST";
        }
        if (gender > 50) {
            gameObject.tag = "FemaleST";
        }

        if (gameObject.transform.localScale.y < 1f) {
            recentlyHatched();
        }
    }

    void Update() {
        GameObject[] STCountMale;
        GameObject[] STCountFemale;
        STCountMale = GameObject.FindGameObjectsWithTag("MaleST");
        STCountFemale = GameObject.FindGameObjectsWithTag("FemaleST");

        float Hatchlingdist = Vector3.Distance(HatchlingGoal.transform.position, transform.position);

        if (isWalking) {
            transform.position += transform.forward * walkSpeed * Time.deltaTime;
        }
        if (isSwimming && transform.position.y <= waterLevel) {
            isSwimming = true;
            transform.position += transform.forward * swimSpeed * Time.deltaTime;
            rb.useGravity = false;
        }
        if (isSwimming && transform.position.y > waterLevel) {
            isSwimming = false;
            isWalking = true;
            rb.useGravity = true;
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
        if (hasEgg) {
            nesting();
        }
        if (gameObject.transform.localScale.y < 1f) {
            gameObject.transform.localScale += scaleChange * growthSpeed * Time.deltaTime;
        }
        if (returningToWater) {
            transform.LookAt(HatchlingGoal.transform.position);
            transform.position += transform.forward * swimSpeed * Time.deltaTime;
        }
        if (Hatchlingdist < 20 && returningToWater) {
            returningToWater = false;
            StartCoroutine(Wander());
        }
    }

    void TargetFood() {
        GameObject FoodItem = FindClosestFood();
        float dist = Vector3.Distance(FoodItem.transform.position, transform.position);

        if (fovScript.canSeeFood && isEating == false && searchingMate == false && hungerScript.hunger < 8 && isWalking) {
            isWalking = false;
            StopCoroutine(Wander());
            transform.LookAt(FoodItem.transform.position);
            transform.position += transform.forward * walkSpeed * Time.deltaTime;
            if (dist < 15) {
                SeaTurtleAnim.SetTrigger("StopWalking");
                isEating = true;
            }
            if (isEating) {
                hungerScript.hunger += 5;
            }
        }
        if (fovScript.canSeeFood && isEating == false && searchingMate == false && hungerScript.hunger < 8 && isSwimming) {
            StopCoroutine(Wander());
            transform.LookAt(FoodItem.transform.position);
            if (dist < 30) {
                SeaTurtleAnim.SetTrigger("StopSwimming");
                isEating = true;
            }
            if (isEating) {
                hungerScript.hunger += 5;
            }
        }
    }

    public void FixedUpdate() {
        GameObject FoodItem = FindClosestFood();

        if (delay > 0f && isEating == true) {
            delay -= Time.fixedDeltaTime;

            if (delay <= 0f && isEating == true) {
                isEating = false;
                if (FoodItem != null) {
                    Destroy(FoodItem);
                    FoodItem = FindClosestFood();
                    delay = 5f;
                }
                SeaTurtleAnim.SetTrigger("StartSwimming");
                isSwimming = true;
                StartCoroutine(Wander());
            }
            if (delay <= 0f && isEating == true) {
                isEating = false;
                if (FoodItem != null) {
                    Destroy(FoodItem);
                    FoodItem = FindClosestFood();
                    delay = 5f;
                }
                SeaTurtleAnim.SetTrigger("StartWalking");
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
        isSwimming = true;
        yield return new WaitForSeconds(walkTime);
        isSwimming = false;
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
        gos = GameObject.FindGameObjectsWithTag("FoodST");
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
        if (gameObject.tag == "MaleST") {
            gos = GameObject.FindGameObjectsWithTag("FemaleST");
        }
        if (gameObject.tag == "FemaleST") {
            gos = GameObject.FindGameObjectsWithTag("MaleST");
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
        yield return new WaitForSeconds(10);
        if (mating && gameObject.tag == "FemaleST") {
            matingScript.mating = 10f;
            if (hasEgg == false) {
                hasEgg = true;
            }
            mating = false;
        }
        if (mating && gameObject.tag == "MaleST") {
            matingScript.mating = 10f;
        }
    }

    public void findMate() {
        GameObject ClosestMate = FindClosestMate();
        searchingMate = true;
        float dist = Vector3.Distance(ClosestMate.transform.position, transform.position);
        transform.LookAt(ClosestMate.transform.position);
        if (dist > 50) {
            transform.position += transform.forward * swimSpeed * Time.deltaTime;
            mating = true;
        }
        if (dist <= 50 && mating)  {
            StartCoroutine(CreateBaby());
        }
        if (mating == false) {
            StopCoroutine(CreateBaby());
        }
    }

    public IEnumerator CreateEgg() {
        yield return new WaitForSeconds(5);
        if (isLayingEgg) {
            hasEgg = false;
            Instantiate(egg, transform.position * 1.02f, transform.rotation);
            returningToWater = true;
        }
        isLayingEgg = false;
    }

    public void nesting() {
        StopCoroutine(Wander());
        if (isLayingEgg == false) {
            transform.LookAt(NestingSite.transform.position);
            transform.position += transform.forward * swimSpeed * Time.deltaTime;
        }
        float dist = Vector3.Distance(NestingSite.transform.position, transform.position);
        if (dist < 10) {
            isLayingEgg = true;
            StartCoroutine(CreateEgg());
        }
        if (isLayingEgg == false) {
            StopCoroutine(CreateEgg());
        }
    }

    public void recentlyHatched() {
        StopCoroutine(Wander());
        returningToWater = true;
    }
}
