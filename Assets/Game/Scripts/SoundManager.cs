using Core.Services;
using UnityEngine;

namespace GameFolder.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        // Audio players components.
        public AudioSource EffectsSource;
        public AudioSource MusicSource;
        // Audio for monster.
        public AudioSource MonsterEffectSource;
        // Random pitch adjustment range.
        public float LowPitchRange = .95f;
        public float HighPitchRange = 1.05f;
        // Singleton instance.
        public static SoundManager Instance = null;
	
        // Initialize the singleton instance.
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                ServiceLocator.Register<SoundManager>(this);
                            
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Cleanup ServiceLocator registration when destroyed.
        private void OnDestroy()
        {
            if (Instance == this)
            {
                ServiceLocator.Unregister<SoundManager>();
                Instance = null;
            }
        }

        // Play a single clip through the sound effects source.
        public void Play(AudioClip clip)
        {
            EffectsSource.clip = clip;
            EffectsSource.Play();
        }
        
        public void PlayMonsterEffect(AudioClip clip)
        {
            MonsterEffectSource.clip = clip;
            MonsterEffectSource.Play();
        }
        // Play a single clip through the music source.
        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }
        // Play a random clip from an array, and randomize the pitch slightly.
        public void RandomSoundEffect(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
            EffectsSource.pitch = randomPitch;
            EffectsSource.clip = clips[randomIndex];
            EffectsSource.Play();
        }
    }
}