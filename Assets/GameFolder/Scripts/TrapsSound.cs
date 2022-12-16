using UnityEngine;

namespace GameFolder.Scripts
{
    public class TrapsSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip spikeTrap2Sound;
        
        public void SpikeTrap2Sound()
        {
            _audioSource.PlayOneShot(spikeTrap2Sound);
        }
    }
}