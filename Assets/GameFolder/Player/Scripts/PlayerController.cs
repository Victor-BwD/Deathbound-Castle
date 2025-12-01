using GameFolder.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour {

        [SerializeField] private GameObject GameOverScreen;
        
        private Rigidbody2D rb;
        private Characters charactersController;
        private PlayerMovement playerMovement;
        private AudioPlayer audioPlayer;
        private string currentLevel;
        private bool isInitialized;
        private bool playerDead;

        public AudioPlayer AudioPlayer => audioPlayer;

        void Start() {
            InitializeComponents();
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            CheckSceneChange();
        }

        void FixedUpdate()
        {
            if (!isInitialized || playerDead) return;
            
            CheckPlayerDeath();
        }

        private void InitializeComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            charactersController = GetComponent<Characters>();
            audioPlayer = GetComponent<AudioPlayer>();
            playerMovement = GetComponent<PlayerMovement>();

            if (rb == null || charactersController == null || playerMovement == null)
            {
                Debug.LogError("PlayerController: Missing required components!");
                isInitialized = false;
                return;
            }

            currentLevel = SceneManager.GetActiveScene().name;
            isInitialized = true;
            playerDead = false;
        }

        private void CheckSceneChange()
        {
            if (!isInitialized) return;
            
            string activeScene = SceneManager.GetActiveScene().name;
            if (!currentLevel.Equals(activeScene))
            {
                HandleSceneChange(activeScene);
            }
        }

        private void HandleSceneChange(string newScene)
        {
            currentLevel = newScene;
            GameObject spawnPoint = GameObject.Find("Spawn");
            
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning($"Spawn point not found in scene: {newScene}");
            }
        }

        private void CheckPlayerDeath()
        {
            if (charactersController.life <= 0)
            {
                HandlePlayerDeath();
            }
        }

        private void HandlePlayerDeath()
        {
            if (playerDead) return;
            
            playerDead = true;

            if (SoulManager.Instance != null)
            {
                SoulManager.Instance.PlayerDied(transform.position);
            }

            rb.simulated = false;
            playerMovement.enabled = false;
            this.enabled = false;
            
            if (GameOverScreen != null)
            {
                GameOverScreen.SetActive(true);
            }
        }

        public void DestroyPlayer()
        {
            Destroy(transform.gameObject);
        }
    }
}
