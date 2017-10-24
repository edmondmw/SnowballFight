using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {
    public enum State {Move, Avoid, Attack};
    public State currentState = State.Attack;
    public GameObject[] playerUnits;
    public float fireRate;

    Transform projectile;
    UnitActions actions;
    UnitHealth health;
    float nextFire;

    private void Awake()
    {
        actions = GetComponent<UnitActions>();
        health = GetComponent<UnitHealth>();
        nextFire = Time.time + 10f;
        //GameObject.FindGameObjectWithTag("Player").transform.
    }

    // Use this for initialization
    void Start () {
        ChangeState(currentState);
	}


    IEnumerator Move()
    {
        Vector3 point = RandomPointOnNavMesh();
        float waitTime = 2f;
        float elapsedTime = 0f;

        // Move to a random location
        while (currentState == State.Move)
        {
            if (!health.active)
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
            nextFire = Time.time + fireRate + Random.Range(0, 0.5f);
            // could add a bit of randomness depending on difficulty
            actions.Attack(playerUnits[Random.Range(0, playerUnits.Length)].transform.position);

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
