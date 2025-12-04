using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public InventoryManager inventoryManager;

    //monobehaviors
    public StageManager stageManager;
    public UIManager uiManager;

    public UserData userData;

    [HideInInspector] public bool gameReady;

    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = 120;
        uiManager.objBlackUI.SetActive(true);
    }

    void Start()
    {
        Load();
        InitManagers();
        
    }

    public void Load()
    {
        bool dataExist = PlayerPrefs.HasKey(Parameter.PlayerPrefKey.SAVE_DATA);
        if (!dataExist)
        {
            userData = new UserData();
            userData.Init(true);
        }
        else
        {
            string userDataJSON = PlayerPrefs.GetString(Parameter.PlayerPrefKey.SAVE_DATA);
            userData = JsonUtility.FromJson<UserData>(userDataJSON);
        }
    }

    public void Save()
    {
        string userDataJSON = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString(Parameter.PlayerPrefKey.SAVE_DATA, userDataJSON);
        Debug.Log("Save Called!");
    }

    public void InitManagers()
    {
        currencyManager = new CurrencyManager();
        inventoryManager = new InventoryManager();

        currencyManager.Init(this);
        inventoryManager.Init(this);

        stageManager.Init(this);

        uiManager.Init(this);

        uiManager.ShowUI(UIState.TitleMenu);
        uiManager.objBlackUI.SetActive(false);

        gameReady = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameReady)
        {
            float dt = Time.deltaTime;
            stageManager.DoUpdate(dt);
            uiManager.DoUpdate(dt);
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }
}
