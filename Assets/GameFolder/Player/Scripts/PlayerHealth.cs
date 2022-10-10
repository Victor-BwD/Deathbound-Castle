using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealth : MonoBehaviour {
        private Characters characters;
        public Text heartCountText;

        private void Start() {
            characters = GetComponent<Characters>();
        }

        void Update() {
            heartCountText.text = "x" + characters.life;
        }
    
        public void PlayerTakaDamage(int damage) {
            characters.life -= damage;
            characters.skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
        }
    }
}

