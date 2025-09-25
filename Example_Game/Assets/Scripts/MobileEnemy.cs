using UnityEngine;

public class MobileEnemy : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform barrelTip;
    public float fireRate = 1f;
    public float missileSpeed = 20f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 90f; 

    private float nextFireTime = 0f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Direction to player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // Rotate tank smoothly toward player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward toward player
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Shoot at intervals
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
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
