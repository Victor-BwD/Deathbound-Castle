using System;
using GameFolder.Scripts;
using UnityEngine;

namespace Player
{
    public class PlayerCombo : MonoBehaviour
    {
        [SerializeField] private Transform skin;
        
        private int comboNumber;
        private Animator receiveSkinAnimator; // Variable to receive animator from the skin game object
        private float timeCombo;
        private AudioPlayer audioPlayer;

        public int ComboNumber => comboNumber;

        private void Start()
        {
            receiveSkinAnimator = skin.GetComponent<Animator>();
            audioPlayer = GetComponent<AudioPlayer>();
        }

        private void Update()
        {
            timeCombo += Time.deltaTime;
            Combo();
        }

        private void Combo()
        {
            if (Input.GetButtonDown("Fire1") && timeCombo > 0.5f) {
                comboNumber++;
                SoundManager.Instance.Play(audioPlayer.attack1Sound);
                if (comboNumber > 2) {
                    comboNumber = 1;
                    SoundManager.Instance.Play(audioPlayer.attack2Sound);
                }

                timeCombo = 0;
                receiveSkinAnimator.Play("PlayerAttack" + comboNumber, -1);
            }

            if (timeCombo >= 1) {
                comboNumber = 0;
            }
        }
    }
}