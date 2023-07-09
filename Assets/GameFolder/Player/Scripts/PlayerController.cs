using GameFolder.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour {

    [SerializeField] private Vector2 speed;
    [SerializeField] private Transform floorCollider;
    [SerializeField] private Transform skin;
    [SerializeField] private GameObject GameOverScreen;

    public LayerMask floorLayer;

    private Rigidbody2D rb;
    private Characters charactersController;
    private PlayerMovement playerMovement;
    private AudioPlayer audioPlayer;
    private string currentLevel;

    public AudioPlayer AudioPlayer => audioPlayer;

        void Start() {
            rb = GetComponent<Rigidbody2D>();
            charactersController = GetComponent<Characters>();
            audioPlayer = GetComponent<AudioPlayer>();
            playerMovement = GetComponent<PlayerMovement>();
    
            currentLevel = SceneManager.GetActiveScene().name;
        
            DontDestroyOnLoad(this.gameObject);
        }

        void Update() 
        {
            if (!currentLevel.Equals(SceneManager.GetActiveScene().name))
            {
                currentLevel = SceneManager.GetActiveScene().name;
                transform.position = GameObject.Find("Spawn").transform.position;
            }

            DisableControls();

        }

        private void DisableControls()
        {
            if (charactersController.life <= 0) {
                rb.simulated = false;
                this.enabled = false;
                playerMovement.enabled = false;
                GameOverScreen.SetActive(true);
            }
        }


        public void DestroyPlayer()
        {
            Destroy(transform.gameObject);
        }
    }
}
