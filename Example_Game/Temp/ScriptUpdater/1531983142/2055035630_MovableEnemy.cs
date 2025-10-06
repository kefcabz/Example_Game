using UnityEngine;
using UnityEngine.AI;

public class MoveableEnemy : MonoBehaviour
{
    public float targetUpdateRate = 1.5f; 
    private float nextTargetTime = 0f;    
    public float moveSpeed = 2f; 
    
    private Transform playerTarget;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb; 

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); 

        // SET THE SPEED
        navMeshAgent.speed = moveSpeed; 
        navMeshAgent.updateRotation = true;
        navMeshAgent.updatePosition = false;

        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.freezeRotation = true; 
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTarget = player.transform;
    }

    void Update()
    {
        if (playerTarget != null)
        {
            if (Time.time >= nextTargetTime)
            {
                navMeshAgent.SetDestination(playerTarget.position);
                nextTargetTime = Time.time + targetUpdateRate; 
            }
        }
    }

    void FixedUpdate()
    {
        if (rb != null && navMeshAgent.enabled && navMeshAgent.hasPath)
        {
            Vector3 worldDelta = navMeshAgent.desiredVelocity;
            rb.linearVelocity = worldDelta;

            if (worldDelta.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(worldDelta);
                rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, navMeshAgent.angularSpeed * Time.fixedDeltaTime / Quaternion.Angle(rb.rotation, lookRotation));
            }


            navMeshAgent.nextPosition = rb.position;
        }
    }
}
