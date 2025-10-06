using UnityEngine;
using UnityEngine.AI;

public class MoveableEnemy : MonoBehaviour
{
    // === Amateur Logic: Timer to slow down targeting ===
    public float targetUpdateRate = 1.5f; 
    private float nextTargetTime = 0f;    
    
    // NOTE: moveSpeed is controlled by the NavMeshAgent component in the Inspector.
    public float moveSpeed = 2f; 
    
    private Transform playerTarget;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb; 

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); 

        // 1. SET THE SPEED
        navMeshAgent.speed = moveSpeed; 
        
        // --- NEW CRITICAL FIX: STOP FIGHTING THE RIGIDBODY! ---
        // 2. Tell the NavMeshAgent: "Just calculate the path, but DON'T move my tank."
        // This is necessary because the Rigidbody will handle the actual movement.
        navMeshAgent.updateRotation = true;
        navMeshAgent.updatePosition = false; // <-- This is the key change!

        // 3. Ensure Rigidbody is NOT Kinematic so it can respect collisions and gravity
        if (rb != null)
        {
            rb.isKinematic = false; // Must be false to collide with walls
            // Always lock X/Z rotation if the tank shouldn't tip over
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
            // Check the timer before updating target
            if (Time.time >= nextTargetTime)
            {
                // Set the destination.
                navMeshAgent.SetDestination(playerTarget.position);
                nextTargetTime = Time.time + targetUpdateRate; 
            }
        }
    }

    // FixedUpdate runs at a reliable rate for physics
    void FixedUpdate()
    {
        if (rb != null && navMeshAgent.enabled && navMeshAgent.hasPath)
        {
            // --- THE MOVEMENT FIX ---
            // Get the calculated velocity from the NavMeshAgent
            Vector3 worldDelta = navMeshAgent.desiredVelocity;
            
            // Move the Rigidbody safely to follow the path
            rb.linearVelocity = worldDelta;

            // Manual rotation for tank turning
            if (worldDelta.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(worldDelta);
                
                // --- SIMPLER ROTATION FIX ---
                // RotateTowards is simpler and more aggressive, preventing stalls.
                // NOTE: The tank's turning speed is now controlled by the "Angular Speed" 
                // setting on the NavMeshAgent component in the Inspector. Set this high (e.g., 360).
                rb.rotation = Quaternion.RotateTowards(rb.rotation, lookRotation, navMeshAgent.angularSpeed * Time.fixedDeltaTime);
            }

            // Tell the Agent where the Rigidbody actually is (since we moved it manually)
            navMeshAgent.nextPosition = rb.position;
        }
    }
}
