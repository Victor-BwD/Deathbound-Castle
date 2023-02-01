using System;
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
            var getCanvas = GameObject.Find("Canvas");
            if (!ReferenceEquals(getCanvas, null))
            {
                DestroyImmediate(getCanvas);
            }
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
            var getCanvas = GameObject.Find("Canvas");
            if (!ReferenceEquals(getCanvas, null))
            {
                DestroyImmediate(getCanvas);
            }
        }
    }
}