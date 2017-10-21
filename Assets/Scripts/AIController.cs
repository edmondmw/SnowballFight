using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    public enum State {Chase, Avoid, Attack};
    public State currentState = State.Attack;
    public GameObject[] playerUnits;
    public float fireRate;

    UnitActions actions;
    UnitHealth health;
    float nextFire;

    private void Awake()
    {
        actions = GetComponent<UnitActions>();
        health = GetComponent<UnitHealth>();
        nextFire = Time.time + fireRate + Random.Range(0, 0.5f);
        //GameObject.FindGameObjectWithTag("Player").transform.
    }

    // Use this for initialization
    void Start () {
	}

    private void Update()
    {
        if(health.active)
            StateHandler(currentState);
    }

    void Move()
    {
    }

    void Attack()
    {
        Debug.Log("in attack");
        if (Time.time >= nextFire)
        {
            Debug.Log("should attack!");
            nextFire = Time.time + fireRate + Random.Range(0, 0.5f);
            actions.Attack(playerUnits[Random.Range(0, playerUnits.Length)].transform.position);
                
        }
    }

    void StateHandler(State state)
    {
        switch(state)
        {
            case State.Attack:
                Attack();
                break;
            default:
                break;
        }
    }
}
