using UnityEngine;
using System.IO;

public class LoadAndSaveData : MonoBehaviour
{
    [SerializeField] InfoToSave infoToSave = new InfoToSave();

    [Space(10)]
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSE_SaveData rseSaveData;
    [SerializeField] RSE_LoadData rseLoadData;

    string filepath;

    private void OnEnable()
    {
        rseLoadData.action += LoadFromJson;
        rseSaveData.action += SaveToJson;
    }
    private void OnDisable()
    {
        rseLoadData.action -= LoadFromJson;
        rseSaveData.action -= SaveToJson;
    }

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/InfoToLoad.json";

        if (FileAlreadyExist()) LoadFromJson();
        else SaveToJson();

        foreach (var item in infoToSave.achievments)
        {
            item.GiveBonnus();
        }
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