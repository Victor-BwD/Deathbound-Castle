using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFolder.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public string firstScene = "Level1";
        
        public void NewGame()
        {
            SceneManager.LoadScene(firstScene);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}