using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealth : MonoBehaviour {
        private Characters characters;
        private AudioPlayer _audioPlayer;
        public Text heartCountText;

        private void Start() {
            characters = GetComponent<Characters>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }

        void Update() {
            heartCountText.text = "x" + characters.life;
        }
    
        public void PlayerTakaDamage(int damage) {
            characters.life -= damage;
            characters.skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
            _audioPlayer.audioSource.PlayOneShot(_audioPlayer.damageSound, 0.2f);
        }
    }
}

