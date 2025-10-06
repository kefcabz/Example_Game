using UnityEngine;
using UnityEngine.AI;

public class MoveableEnemy : MonoBehaviour
{
    public float targetUpdateRate = 1.5f; 
    private float nextTargetTime = 0f;    
    
    public float moveSpeed = 2f;
    public float turnSpeed = 500f;

    private Transform playerTarget;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Match the NavMeshAgent speed to custom moveSpeed
        navMeshAgent.speed = moveSpeed; 
        
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
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
            // Check the timer before updating target
            if (Time.time >= nextTargetTime)
            {
                // Only set destination once every 1.5 seconds
                navMeshAgent.SetDestination(playerTarget.position);
                nextTargetTime = Time.time + targetUpdateRate; 
            }

            if (navMeshAgent.hasPath)
            {
                // Calculate the direction the enemy needs to turn to follow the path
                Vector3 direction = (navMeshAgent.steeringTarget - transform.position).normalized;

                // Rotate like a tank
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

                // Move forward
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
