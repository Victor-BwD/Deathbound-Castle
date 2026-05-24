using Core.Characters;
using GameFolder.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour {

        [SerializeField] private GameObject GameOverScreen;
        
        private Rigidbody2D rb;
        private Characters charactersController;
        private HealthComponent healthComponent;
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
            healthComponent = charactersController != null ? charactersController.Health : GetComponent<HealthComponent>();
            audioPlayer = GetComponent<AudioPlayer>();
            playerMovement = GetComponent<PlayerMovement>();

            if (rb == null || healthComponent == null || playerMovement == null)
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
        }

        private void CheckPlayerDeath()
        {
            if (healthComponent.IsDead)
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
