using System;
using UnityEngine;

namespace GameFolder.Scripts
{
    public class StartMusicPhase : MonoBehaviour
    {
        [SerializeField] private AudioClip musicPhase1;
        private void Start()
        {
            SoundManager.Instance.PlayMusic(musicPhase1);
        }
    }
}