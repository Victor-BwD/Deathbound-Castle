using System;
using UnityEngine;

namespace Player
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip attack1Sound;
        public AudioClip attack2Sound;
        public AudioClip playerGroundedSound;
        public AudioClip damageSound;
        public AudioClip dashSound;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}