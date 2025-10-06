using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalEnemies = 2; 

    public TextMeshProUGUI winText; 
    public TextMeshProUGUI youLostText;

    private void Awake()
    {
        // Set up the static instance
        Instance = this;
        
        // Hide all end-game messages at the start
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
        if (youLostText != null)
        {
            youLostText.gameObject.SetActive(false);
        }
    }

    public void EnemyDied()
    {
        totalEnemies--;

        Debug.Log("An enemy was destroyed! " + totalEnemies + " left.");

        // Check for Win condition
        if (totalEnemies <= 0)
        {
            HandleGameWin();
        }
    }

    public void PlayerDied()
    {
        Debug.Log("Player Lost! Game Over!");
        if (youLostText != null)
        {
            // Show "You Lost" message
            youLostText.text = "YOU LOST!";
            youLostText.gameObject.SetActive(true);
        }
    }

    private void HandleGameWin()
    {
        Debug.Log("Player Wins! Game Over!");
        if (winText != null)
        {
            // Show "You Win" message
            winText.text = "YOU WIN!!";
            winText.gameObject.SetActive(true);
        }
    }
}
