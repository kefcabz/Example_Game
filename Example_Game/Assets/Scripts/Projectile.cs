using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;

    void Start()
    {
        // Destroy after lifetime expires in case it never hits anything
        Destroy(gameObject, lifetime);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        // Destroy missile when it hits anything
            Destroy(gameObject);
    }
}
