using Core.Characters;
using GameFolder.Scripts;
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
            characters.Skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
            SoundManager.Instance.Play(_audioPlayer.damageSound);
        }
    }
}

