using System;
using TMPro;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    private TextMeshProUGUI soul_count;

    private static SoulManager _instance;
    public static SoulManager Instance { get { return _instance; } }

    private int soulCount = 0;

    private void Awake()
    {
        // Singleton pattern setup
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        soul_count = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(soul_count);
        UpdateSoulUI();
    }


    public void AddSouls(int amount)
    {
        soulCount += amount;
        UpdateSoulUI();
        Debug.Log("Souls added: " + amount + " | Total: " + soulCount);
    }


    public bool SpendSouls(int amount)
    {
        if (soulCount >= amount)
        {
            soulCount -= amount;
            UpdateSoulUI();
            return true;
        }
        return false;
    }


    public int GetSoulCount()
    {
        return soulCount;
    }


    private void UpdateSoulUI()
    {
        if (soul_count != null)
        {
            soul_count.text = soulCount.ToString();
        }
    }
}

