using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundManagerMainMenu : MonoBehaviour
{
    private void Awake()
    {
        var soundManager = GameObject.Find("SoundManager");
        if (!ReferenceEquals(soundManager, null))
        {
            DestroyImmediate(soundManager);
        }
    }
}
