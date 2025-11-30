using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulManager : MonoBehaviour
{
    private TextMeshProUGUI soul_count;

    [SerializeField]
    private GameObject lostSoulsPrefab;

    [Header("Debug")]
    [SerializeField] private KeyCode cheatKey = KeyCode.F10;
    [SerializeField]
    private int cheatSoulAmount = 1000;

    private static SoulManager _instance;
    public static SoulManager Instance { get { return _instance; } }

    private int soulCount = 0;
    private int lostSouls = 0;

    private Vector3 deathPosition;
    private bool hasSoulsToRecover = false;
    private string deathSceneName = "";
    private GameObject currentLostSoulsObject = null;

    private const string HAS_SOULS_KEY = "HasSoulsToRecover";
    private const string LOST_SOULS_KEY = "LostSoulsAmount";
    private const string DEATH_POS_X_KEY = "DeathPosX";
    private const string DEATH_POS_Y_KEY = "DeathPosY";
    private const string DEATH_POS_Z_KEY = "DeathPosZ";
    private const string DEATH_SCENE_KEY = "DeathSceneName";
    private const string NEW_GAME_STARTED = "NewGameStarted";

    private const float SPAWN_DELAY = 0.5f;


    private void Awake()
    {
  
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSavedData();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        soul_count = GetComponentInChildren<TextMeshProUGUI>();

        UpdateSoulUI();
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    void Update()
    {
        if (Input.GetKeyDown(cheatKey))
        {
            AddSouls(cheatSoulAmount);
            Debug.Log("CHEAT ATIVADO: Adicionadas " + cheatSoulAmount + " almas");
        }
    }
#endif

    public void SetSoulCount(int amount)
    {
        soulCount = amount;
        UpdateSoulUI();
        Debug.Log("CHEAT: Soul count definido para " + amount);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Carrega os dados salvos do PlayerPrefs
    private void LoadSavedData()
    {
        bool isNewGame = PlayerPrefs.GetInt(NEW_GAME_STARTED, 0) == 1;

        if (isNewGame)
        {
            ClearSoulData();
            PlayerPrefs.DeleteKey(NEW_GAME_STARTED);
            PlayerPrefs.Save();

            return;
        }

        hasSoulsToRecover = PlayerPrefs.GetInt(HAS_SOULS_KEY, 0) == 1;

        if (hasSoulsToRecover)
        {
            lostSouls = PlayerPrefs.GetInt(LOST_SOULS_KEY, 0);

            float x = PlayerPrefs.GetFloat(DEATH_POS_X_KEY, 0);
            float y = PlayerPrefs.GetFloat(DEATH_POS_Y_KEY, 0);
            float z = PlayerPrefs.GetFloat(DEATH_POS_Z_KEY, 0);
            deathPosition = new Vector3(x, y, z);

            deathSceneName = PlayerPrefs.GetString(DEATH_SCENE_KEY, "");
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(HAS_SOULS_KEY, hasSoulsToRecover ? 1 : 0);

        if (hasSoulsToRecover)
        {
            PlayerPrefs.SetInt(LOST_SOULS_KEY, lostSouls);

            PlayerPrefs.SetFloat(DEATH_POS_X_KEY, deathPosition.x);
            PlayerPrefs.SetFloat(DEATH_POS_Y_KEY, deathPosition.y);
            PlayerPrefs.SetFloat(DEATH_POS_Z_KEY, deathPosition.z);

            PlayerPrefs.SetString(DEATH_SCENE_KEY, deathSceneName);
        }

        PlayerPrefs.Save();
    }

    // Método para limpar dados de almas - usado APENAS para New Game
    public void ClearSoulData()
    {
        hasSoulsToRecover = false;
        lostSouls = 0;
        soulCount = 0;

        if (currentLostSoulsObject != null)
        {
            Destroy(currentLostSoulsObject);
            currentLostSoulsObject = null;
        }

        PlayerPrefs.DeleteKey(HAS_SOULS_KEY);
        PlayerPrefs.DeleteKey(LOST_SOULS_KEY);
        PlayerPrefs.DeleteKey(DEATH_POS_X_KEY);
        PlayerPrefs.DeleteKey(DEATH_POS_Y_KEY);
        PlayerPrefs.DeleteKey(DEATH_POS_Z_KEY);
        PlayerPrefs.DeleteKey(DEATH_SCENE_KEY);
        PlayerPrefs.Save();

        UpdateSoulUI();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasSoulsToRecover && scene.name == deathSceneName)
        {
            Invoke("SpawnLostSouls", SPAWN_DELAY);
        }
    }

    private void SpawnLostSouls()
    {
        if (lostSoulsPrefab == null)
        {
            Debug.LogError("lostSoulsPrefab não está atribuído no SoulManager!");
            return;
        }


        GameObject lostSoulsObject = Instantiate(lostSoulsPrefab, deathPosition, Quaternion.identity);
        currentLostSoulsObject = lostSoulsObject; // Guarda referencia ao objeto de almas atual

        LostSouls lostSoulsComponent = lostSoulsObject.GetComponent<LostSouls>();
        if (lostSoulsComponent != null)
        {
            lostSoulsComponent.SetSoulAmount(lostSouls);
            Debug.Log("Almas perdidas spawnadas com sucesso: " + lostSouls);
        }
        else
        {
            Debug.LogError("Prefab não contém componente LostSouls!");
        }
    }

    public void PlayerDied(Vector3 position)
    {
        if (hasSoulsToRecover)
        {
            if (currentLostSoulsObject != null)
            {
                Destroy(currentLostSoulsObject);
            }
        }

        if (soulCount > 0)
        {
            deathPosition = position;
            deathSceneName = SceneManager.GetActiveScene().name;
            lostSouls = soulCount;
            soulCount = 0;
            hasSoulsToRecover = true;

            SaveData();
        }
        else
        {
            hasSoulsToRecover = false;
            SaveData();
        }

        UpdateSoulUI();
    }

    public void RecoverLostSouls(int amount)
    {
        AddSouls(amount);
        hasSoulsToRecover = false;
        lostSouls = 0;
        currentLostSoulsObject = null;

        SaveData();
    }

    public void AddSouls(int amount)
    {
        soulCount += amount;
        UpdateSoulUI();
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