using UnityEngine;

namespace GameFolder.Scripts
{
    public class TrapsSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip spikeTrap2Sound;
        [SerializeField] private float minDistance = 0.2f;
        [SerializeField] private float maxDistance = 0.5f;

        private void Awake()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) return;

            _audioSource.spatialBlend = 1f;
            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
        }

        public void SpikeTrap2Sound()
        {
            if (_audioSource == null || spikeTrap2Sound == null) return;
            _audioSource.PlayOneShot(spikeTrap2Sound);
        }
    }
}