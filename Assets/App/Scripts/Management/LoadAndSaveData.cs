using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadAndSaveData : MonoBehaviour
{
    [SerializeField] InfoToSave infoToSave = new InfoToSave();

    [Space(10)]
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSO_AchievmentCompleteCount rsoAchievmentCompleteCount;
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

    private void Start()
    {
        filepath = Application.persistentDataPath + "/InfoToLoad.json";
        print(filepath);

        if (FileAlreadyExist()) LoadFromJson();
        else SaveToJson();

        rsoAchievmentCompleteCount.Value = 0;
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
            if (infoToSave.achievmentsStatus.Length <= i) achievments[i].isAchieve = false;
            else achievments[i].isAchieve = infoToSave.achievmentsStatus[i];
        }

        string infoData = JsonUtility.ToJson(infoToSave);
        System.IO.File.WriteAllText(filepath, infoData);
    }

    void LoadFromJson()
    {
        string infoData = System.IO.File.ReadAllText(filepath);
        infoToSave = JsonUtility.FromJson<InfoToSave>(infoData);

        // Vérification de la taille du tableau
        if (infoToSave.achievmentsStatus.Length != achievments.Length)
        {
            Debug.LogWarning("Mismatch between saved achievements and current achievements. Adjusting size.");
            bool[] correctedStatus = new bool[achievments.Length];

            // Copier les valeurs existantes dans les limites du tableau
            for (int i = 0; i < Mathf.Min(infoToSave.achievmentsStatus.Length, correctedStatus.Length); i++)
            {
                correctedStatus[i] = infoToSave.achievmentsStatus[i];
            }

            infoToSave.achievmentsStatus = correctedStatus;
        }

        // Appliquer les états chargés aux réalisations
        for (int i = 0; i < achievments.Length; i++)
        {
            infoToSave.achievmentsStatus[i] = achievments[i].isAchieve;
        }

        rsoContentSave.Value.totalDistanceTravelled = infoToSave.totalDistanceTravelled;
        rsoContentSave.Value.totalEnemysKilled = infoToSave.totalEnemysKilled;
        rsoContentSave.Value.totalTimeAlive = infoToSave.totalTimeAlive;
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

    public float musicVolume = 1;
    public float soundVolume = 1;
    public float mouseSensitivity = 1;
    public bool isFullScreen = true;

    public InfoToSave()
    {
        achievmentsStatus = new bool[0];
        totalDistanceTravelled = 0;
        totalEnemysKilled = 0;
        totalTimeAlive = 0;
        jumpCount = 0;
        healCount = 0;

        musicVolume = 1;
        soundVolume = 1;
        mouseSensitivity = 1;
        isFullScreen = true;
    }
}