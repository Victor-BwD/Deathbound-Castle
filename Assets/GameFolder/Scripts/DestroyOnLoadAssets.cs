using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoadAssets : MonoBehaviour
{
    private void Awake()
    {
        var findPlayer = GameObject.Find("Player");
        if (!ReferenceEquals(findPlayer, null))
        {
            DestroyImmediate(findPlayer);
        }
    }
}
