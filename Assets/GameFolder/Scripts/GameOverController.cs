using UnityEngine;
using UnityEngine.SceneManagement;
using GameFolder.Scripts;

public class GameOverController : MonoBehaviour
{
    public void RestartLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            Destroy(playerObj);
        }
        
        // Recarregar a cena
        SceneManager.LoadScene(currentLevel);
    }

    public void ReturnToMenu()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            Destroy(playerObj);
        }
        
        SceneManager.LoadScene("Menu");
    }
}