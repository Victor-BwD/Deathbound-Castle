using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFolder.Scripts
{
    public class ChangeScene : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}