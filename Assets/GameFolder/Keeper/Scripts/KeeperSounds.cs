using GameFolder.Scripts;
using UnityEngine;

namespace Keeper
{
    public class KeeperSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip keeperDieSound;
        [SerializeField] private AudioClip keeperAttackSound;
        
        public void DieSound()
        {
            SoundManager.Instance.Play(keeperDieSound);
        }

        public void KeeperAttackSound()
        {
            SoundManager.Instance.Play(keeperAttackSound);
        }
    }
}