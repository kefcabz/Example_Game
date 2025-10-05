using UnityEngine;

public class StationaryEnemy : MonoBehaviour
{
    public GameObject missilePrefab;  
    public Transform barrelTip;      
    public float fireRate = 1f;       
    public float missileSpeed = 10f;
    public float detectionRange = 40f; 

    private float nextFireTime = 0f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        // Only try to see player if within detection range
        if (distance <= detectionRange)
        {
            // Raycast to check line of sight
            Ray ray = new Ray(barrelTip.position, (player.position - barrelTip.position).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // Player is visible — rotate and shoot
                    transform.rotation = Quaternion.LookRotation(direction);

                    if (Time.time >= nextFireTime)
                    {
                        Shoot();
                        nextFireTime = Time.time + 1f / fireRate;
                    }
                }
            }
        }
    }

    void Shoot()
    {
        GameObject missile = Instantiate(missilePrefab, barrelTip.position, barrelTip.rotation);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = barrelTip.forward * missileSpeed;

        Destroy(missile, 5f);
    }
}
