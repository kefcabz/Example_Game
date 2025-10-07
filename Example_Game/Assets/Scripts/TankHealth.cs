using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public int currentHealth = 100;

    // How much damage one missile deals
    private const int MISSILE_DAMAGE = 25; 

    private bool isDestroyed = false;

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return; // Stop if tank is already dead

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! Remaining Health: " + currentHealth);

        // Check if the tank's health hit zero
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go negative
            Die();
        }
    }

    private void Die()
    {
        isDestroyed = true;
        Debug.Log(gameObject.name + " is destroyed!");

        if (GameManager.Instance != null)
        {
            // Check if this tank is tagged "Enemy"
            if (gameObject.CompareTag("Enemy"))
            {
                GameManager.Instance.EnemyDied();
            }
            // If it's not tagged "Enemy", assume it's the Player.
            else if (gameObject.CompareTag("Player"))
            {
                GameManager.Instance.PlayerDied();
            }
        }

        //hide and disable the tank object.
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            // Apply damage to this tank
            TakeDamage(MISSILE_DAMAGE);

            // Destroy the missile object so it doesn't continue flying or hit multiple times
            Destroy(other.gameObject); 
        }
    }
}
