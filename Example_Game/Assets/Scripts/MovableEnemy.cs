using UnityEngine;
using UnityEngine.AI;

public class MoveableEnemy : MonoBehaviour
{
    public float maxHealth = 50f;
    public float moveSpeed = 2f;
    public float turnSpeed = 500f; 

    private float currentHealth;
    private Transform playerTarget;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false; 
        navMeshAgent.updatePosition = false; 
        currentHealth = maxHealth;
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
           
            navMeshAgent.SetDestination(playerTarget.position);

            if (navMeshAgent.hasPath)
            {
                Vector3 direction = (navMeshAgent.steeringTarget - transform.position).normalized;

                // Rotate like a tank 
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

                // Move forward
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy tank took " + damage + " damage. Health: " + currentHealth);

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Tank Destroyed!!.");
        Destroy(gameObject);
    }
}
