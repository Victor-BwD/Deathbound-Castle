using UnityEngine;
using UnityEngine.SceneManagement;
using GameFolder.Scripts;

public class GameOverController : MonoBehaviour
{
    public void RestartLevel()
    {
        // Log detalhado para depuração
        Debug.Log("### RESTART LEVEL FOI CHAMADO ###");
        
        // Apenas recarregar o nível atual sem limpar os dados de almas
        string currentLevel = SceneManager.GetActiveScene().name;
        Debug.Log("Recarregando nível: " + currentLevel);
        
        // Garantir que o objeto do jogador seja destruído para ser recriado
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            Debug.Log("Destruindo jogador antes de reiniciar");
            Destroy(playerObj);
        }
        
        // Recarregar a cena
        SceneManager.LoadScene(currentLevel);
        Debug.Log("Reiniciando nível mantendo dados de almas");
    }

    public void ReturnToMenu()
    {
        // Log detalhado para depuração
        Debug.Log("### RETURN TO MENU FOI CHAMADO ###");
        
        // Retornar ao menu principal
        Debug.Log("Carregando cena do menu");
        
        // Destruir o objeto do jogador para não persistir
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            Debug.Log("Destruindo jogador antes de voltar ao menu");
            Destroy(playerObj);
        }
        
        SceneManager.LoadScene("Menu");
    }
}