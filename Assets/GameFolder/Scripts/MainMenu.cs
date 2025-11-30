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
            PlayerPrefs.SetInt("NewGameStarted", 1);
            
            PlayerPrefs.DeleteKey("HasSoulsToRecover");
            PlayerPrefs.DeleteKey("LostSoulsAmount");
            PlayerPrefs.DeleteKey("DeathPosX");
            PlayerPrefs.DeleteKey("DeathPosY");
            PlayerPrefs.DeleteKey("DeathPosZ");
            PlayerPrefs.DeleteKey("DeathSceneName");
            PlayerPrefs.Save();

            if (SoulManager.Instance != null)
            {
                SoulManager.Instance.ClearSoulData();
                Debug.Log("Almas resetadas pelo início de um novo jogo via SoulManager");
            }

            Debug.Log("PlayerPrefs de almas limpos diretamente para novo jogo");

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