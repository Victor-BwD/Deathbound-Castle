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
            SoundManager.Instance.PlayMonsterEffect(keeperDieSound);
        }

        public void KeeperAttackSound() // call in animation event
        {
            SoundManager.Instance.PlayMonsterEffect(keeperAttackSound);
        }
    }
}