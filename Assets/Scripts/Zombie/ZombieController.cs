using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieController : MonoBehaviour
{
    //=========== UNITY INSPECTOR ==============
    [Header("Requirements")] 
    [SerializeField] private ScoreManager score;
    [Header("Parameters")]
    [Range(10f, 50f)] 
    [SerializeField] private float detectionRadius;
    //==========================================
    
    
    private NavMeshAgent agent;
    private GameObject closestSurvivor;
    private enum State { HUNTING, IDLE, DEAD }
    private State currentState;

    private int lives = 3;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.IDLE;

        score = GameObject.Find("GameManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.closestSurvivor = GetClosestSurvivor();
        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.HUNTING :
                Hunt();
                break;
        }
    }
    
    //=================================== STATES ==========================================
    
    /// <summary>
    /// This function will execute the exit function of the <c>this.currentState</c>. Then,
    /// it will perform the start function of the <c>nextState</c>. Lastly, it will
    /// change the value of this.currentState to <c>nextState</c>.
    /// </summary>
    /// <param name="nextState">
    /// the <c>State</c> that the attribute <c>currentState</c> will be updated to.
    /// </param>
    /// <exception cref="Exception">
    /// Thrown when the <c>nextState</c> is the same to the <c>this.currentState</c>
    /// </exception>
    private void ChangeState(State nextState)
    {
        if (this.currentState == nextState) throw new Exception("nextState must be different to the current state");
        
        //note: Each case in this switch case should execute the exit function of each state
        switch (this.currentState)
        {
            case State.IDLE:
                break;
            case State.HUNTING:
                break;
        }
        
        //note: Each case in this switch case should execute the start function of each state
        switch (nextState)
        {
            case State.IDLE:
                break;
            case State.HUNTING:
                break;
            
            case State.DEAD:
                Debug.Log("hello there");
                Destroy(this.gameObject);
                return;
        }

        this.currentState = nextState;
    }

    /// <summary>
    /// Simulates a walk without any specific destination. If there is a survivor inside the
    /// detection radius, then it will change the state tu <c>HUNTING</c>. 
    /// </summary>
    private void Idle()
    {
        if (this.closestSurvivor)
        {
            float distance = Vector3.Distance(this.closestSurvivor.transform.position, this.transform.position);
            if (distance < detectionRadius)
            {
                ChangeState(State.HUNTING);
                return;
            }
        }
        
        Wander();
        
    }
    
    /// <summary>
    /// Pursuits the closest survivor. If the survivor gets out the detection radius then, it
    /// will change the state to <c>IDLE</c>
    /// </summary>
    private void Hunt()
    {
        if (!this.closestSurvivor)
        {
            ChangeState(State.IDLE);
            return;
        }

        if (Vector3.Distance(this.closestSurvivor.transform.position, this.transform.position) > detectionRadius)
        {
            ChangeState(State.IDLE);
            return;
        }
        
        Pursuit(this.closestSurvivor);
    }
    
    //=========================================================================================

    
    //============================== STEERING BEHAVIOURS ======================================
    /// <summary>
    /// Given a target, calculate a prediction vector and seek it depending on the target speed
    /// and the distance between this zombie and the target
    /// </summary>
    /// <param name="target">
    /// The <c>GameObject</c> to be pursued. This target must be the player (w/ <c>RigidBody</c>)
    /// or a survivor agent (w/ <c>NavMeshAgent</c>)
    /// </param>
    private void Pursuit(GameObject target)
    {
        const float MIN_DISTANCE = 1f;
        const float MIN_SPEED = 1f;
        const float PREDICTION_SCALE_FACTOR = 0.001f; 
        
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        
        if (distance < MIN_DISTANCE) return;

        float targetSpeed = 0f;
        Vector3 targetDirection;

        try
        {
            NavMeshAgent targetAgent = target.GetComponent<NavMeshAgent>();
            targetSpeed = targetAgent.speed; //tries to get it from survivors agents
            targetDirection = targetAgent.velocity.normalized;
        }
        catch (MissingComponentException)
        {
            Rigidbody playerRB = target.GetComponent<Rigidbody>();
            targetSpeed = playerRB.velocity.magnitude; //gets if from player rigidbody
            targetDirection = playerRB.velocity.normalized;
        }
        
        if(targetSpeed < MIN_SPEED) Seek(target.transform.position);
        
        float scaleFactor = (targetSpeed * distance) * PREDICTION_SCALE_FACTOR; 
        
        Vector3 destination = target.transform.position + (targetDirection * scaleFactor);
        
        Seek(destination);
    }

    /// <summary>
    /// Set the NavMeshAgent destination to a specific position
    /// </summary>
    /// <param name="destination">
    /// <c>Vector3</c> The location where the agent will be directed to
    /// </param>
    private void Seek(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private Vector3 wanderTarget = Vector3.zero;
    private void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 5;
        
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();

        wanderTarget *= wanderRadius;
        Vector3 targetlocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.transform.InverseTransformVector(targetlocal);
        
        Seek(targetWorld);

    }
    
    //============================================================================================
    
    
    /// <summary>
    /// Compare the distance all the positions of all game objects with the tag "<i>Survivor</i>" 
    /// </summary>
    /// <returns>
    /// the <c>GameObject</c> with the closest distance with this zombie.
    /// </returns>
    private GameObject GetClosestSurvivor()
    {
        GameObject[] survivors = GameObject.FindGameObjectsWithTag("Survivor");

        if (survivors.Length < 1) return null;

        float minDistance = Mathf.Infinity;
        GameObject closestSurvivor = null;
        
        foreach (GameObject survivor in survivors)
        {
            float dist = Vector3.Distance(survivor.transform.position, this.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestSurvivor = survivor;
            }
        }

        return closestSurvivor;
    }
    
    /// <summary>
    /// Reduce the lives by <c>n</c> lives and if there are no lives
    /// remaining, then change the state to <c>DEAD</c>
    /// </summary>
    /// <param name="n">
    /// The number of lives to be reduced
    /// </param>
    public void Damage(int n = 1)
    {
        this.lives -= n;
        if (this.lives < 1)
        {
            score.AddToScore(50);
            ChangeState(State.DEAD);
        }
    }


}
