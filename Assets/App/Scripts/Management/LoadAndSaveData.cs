using UnityEngine;
using System.IO;

public class LoadAndSaveData : MonoBehaviour
{
    [SerializeField] InfoToSave infoToSave = new InfoToSave();

    [Space(10)]
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSE_SaveData rseSaveData;
    [SerializeField] RSE_LoadData rseLoadData;
    [SerializeField] RSE_SaveAllGameData rseSaveAllGameData;

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

        foreach (var item in infoToSave.achievments)
        {
            item.GiveBonnus();
        }
    }

    void SaveAllGameData()
    {
        Invoke("SaveToJson", .1f);
    }

    void SaveToJson()
    {
        infoToSave = rsoContentSave.Value;
        string infoData = JsonUtility.ToJson(infoToSave);
        System.IO.File.WriteAllText(filepath, infoData);
    }
    void LoadFromJson()
    {
        string infoData = System.IO.File.ReadAllText(filepath);
        infoToSave = JsonUtility.FromJson<InfoToSave>(infoData);
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
    public Achievment[] achievments;

    public float totalDistanceTravelled;
    public int totalEnemysKilled;
    public float totalTimeAlive;
}