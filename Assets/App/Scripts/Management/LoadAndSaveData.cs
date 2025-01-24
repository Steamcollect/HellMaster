using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LoadAndSaveData : MonoBehaviour
{
    [SerializeField] InfoToSave infoToSave = new InfoToSave();

    [Space(10)]
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSO_Achievements rsoAchievements;
    [SerializeField] RSO_AchievmentCompleteCount rsoAchievmentCompleteCount;

    [Space(10)]
    [SerializeField] RSE_SaveData rseSaveData;
    [SerializeField] RSE_LoadData rseLoadData;
    [SerializeField] RSE_SaveAllGameData rseSaveAllGameData;

    [SerializeField] Achievment[] achievments;

    string filepath;

    private void OnEnable()
    {
        rseLoadData.action += LoadFromJson;
        rseSaveData.action += SaveToJson;
        rseSaveAllGameData.action += SaveAllGameData;
    }
    private void OnDisable()
    {
        rseLoadData.action -= LoadFromJson;
        rseSaveData.action -= SaveToJson;
        rseSaveAllGameData.action -= SaveAllGameData;
    }

    private void Awake()
    {
        rsoAchievements.Value = achievments;
    }

    private void Start()
    {
        filepath = Application.persistentDataPath + "/InfoToLoad.json";
        print(filepath);

        if (FileAlreadyExist())
        {
            LoadFromJson();
        }
        else
        {
            InitializeDefaultValues();
            SaveToJson();
        }

        rsoAchievmentCompleteCount.Value = 0;
    }

    void InitializeDefaultValues()
    {
        rsoContentSave.Value.mouseSensitivity = 1;
        infoToSave.InitializeDefaults(achievments.Length);
        rsoContentSave.Value = infoToSave;
    }

    void SaveAllGameData()
    {
        Invoke("SaveToJson", .05f);
    }

    void SaveToJson()
    {
        infoToSave = rsoContentSave.Value;
        for (int i = 0; i < achievments.Length; i++)
        {
            infoToSave.achievmentsStatus[i] = achievments[i].isAchieve;
        }

        string infoData = JsonUtility.ToJson(infoToSave);
        System.IO.File.WriteAllText(filepath, infoData);
    }

    void LoadFromJson()
    {
        if (!FileAlreadyExist())
        {
            Debug.LogWarning("No save file found, initializing default values.");
            InitializeDefaultValues();
            return;
        }

        string infoData = System.IO.File.ReadAllText(filepath);
        infoToSave = JsonUtility.FromJson<InfoToSave>(infoData);

        // Validate and correct data
        if (infoToSave.achievmentsStatus == null || infoToSave.achievmentsStatus.Length != achievments.Length)
        {
            Debug.LogWarning("Mismatch between saved achievements and current achievements. Adjusting size.");
            bool[] correctedStatus = new bool[achievments.Length];

            for (int i = 0; i < Mathf.Min(infoToSave.achievmentsStatus.Length, correctedStatus.Length); i++)
            {
                correctedStatus[i] = infoToSave.achievmentsStatus[i];
            }

            infoToSave.achievmentsStatus = correctedStatus;
        }

        // Correct any invalid numeric values
        if (infoToSave.bestScore < 0 || infoToSave.bestScore > 1000000) // Set an arbitrary upper limit
        {
            Debug.LogWarning("Invalid bestScore detected, resetting to 0.");
            infoToSave.bestScore = 0;
        }

        rsoContentSave.Value = infoToSave;
    }

    bool FileAlreadyExist()
    {
        return File.Exists(filepath);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}

[System.Serializable]
public class InfoToSave
{
    public bool[] achievmentsStatus;

    public float totalDistanceTravelled;
    public int totalEnemysKilled;
    public float totalTimeAlive;
    public int jumpCount;
    public int healCount;
    public int bestScore;

    public float musicVolume = 1;
    public float soundVolume = 1;
    public float mouseSensitivity = 1;
    public bool isFullScreen = true;
    public bool canShake = true;

    public void InitializeDefaults(int achievementCount)
    {
        achievmentsStatus = new bool[achievementCount];
        totalDistanceTravelled = 0;
        totalEnemysKilled = 0;
        totalTimeAlive = 0;
        jumpCount = 0;
        healCount = 0;
        bestScore = 0;
        musicVolume = 1;
        soundVolume = 1;
        mouseSensitivity = 1;
        isFullScreen = true;
        canShake = true;
    }
}