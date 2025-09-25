using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject missilePrefab;      
    public Transform barrelTip;          
    public float missileSpeed = 20f;      
    public float fireRate = 1f;           

    private float nextFireTime = 0f;

    void Update()
    {
        // Shoot when Space is pressed and cooldown has passed
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        // Spawn missile at barrel tip with barrel rotation
        GameObject missile = Instantiate(missilePrefab, barrelTip.position, barrelTip.rotation);

        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = barrelTip.forward * missileSpeed;
        }

    }
}
