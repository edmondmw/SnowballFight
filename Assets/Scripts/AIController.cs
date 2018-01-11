using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {
    public enum State {Move, Avoid, Attack};
    public State currentState;
    
    public float fireRate;

    Transform projectile;
    UnitActions actions;
    UnitHealth health;
    List<GameObject> playerUnits;
    float nextFire;

    private void Awake()
    {
        actions = GetComponent<UnitActions>();
        health = GetComponent<UnitHealth>();
        nextFire = Time.time + fireRate + Random.Range(0.25f, 2f);
        playerUnits = new List<GameObject>();

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        int count = playerTransform.childCount;
        for(int i = 0; i < count; ++i)
        {
            playerUnits.Add(playerTransform.GetChild(i).gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        ChangeState(currentState);
	}


    IEnumerator Move()
    {
        Vector3 point = RandomPointOnNavMesh();
        float waitTime = 3f;
        float elapsedTime = 0f;

        // Move to a random location
        while (currentState == State.Move)
        {
            if (!health.isAlive())
            {
                StopAllCoroutines();
                yield return null;
            }

            actions.Move(point);
            elapsedTime += Time.deltaTime;
            
            // Select a new destination
            if(elapsedTime >= waitTime)
            {
                elapsedTime = 0f;
                point = RandomPointOnNavMesh();
            }
            
            // If can fire, then fire
            if(Time.time >= nextFire)
            {
                ChangeState(State.Attack);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator Attack()
    {
        while (currentState == State.Attack)
        {
            nextFire = Time.time + fireRate + Random.Range(0, 1f);

            int randIndex = Random.Range(0, playerUnits.Count);
            while (!playerUnits[randIndex].GetComponent<UnitHealth>().isAlive())
            {
                playerUnits.Remove(playerUnits[randIndex]);
                randIndex = Random.Range(0, playerUnits.Count);
            }

            // could add a bit of randomness depending on difficulty
            actions.Attack(playerUnits[randIndex].transform.position);

            if (Time.time < nextFire)
            {
                ChangeState(State.Move);
            } 

            yield return null;
        } 
    }

    IEnumerator Avoid()
    {
        while (currentState == State.Avoid)
        {
            yield return null;

        }
    }

    void ChangeState(State state)
    {
        StopAllCoroutines();
        currentState = state;
        switch(state)
        {
            case State.Move:
                StartCoroutine(Move());
                break;
            case State.Attack:
                StartCoroutine(Attack());
                break;
            case State.Avoid:
                StartCoroutine(Avoid());
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Snowball") && other.gameObject.layer != gameObject.layer)
        {
            projectile = other.transform;
           //  ChangeState(State.Avoid);
        }
    }

    Vector3 RandomPointOnNavMesh()
    {
        float radius = 15f;
        Vector3 point = transform.position + Random.insideUnitSphere * radius;
        NavMeshHit nh;
        NavMesh.SamplePosition(point, out nh, radius, NavMesh.AllAreas);
        return nh.position;
    }
}
