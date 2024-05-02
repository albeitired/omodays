using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

    [SerializeField] private string fileName = "data.omo";
    public static DataPersistenceManager instance { get; private set; }

    private PlayerData playerData;
    private GameManager gm;
    private List<IDataPersistence> DataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake() {
        if(instance != null) {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }

    private void Start() {
        this.gm = FindObjectOfType<GameManager>();
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.DataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() {
        this.playerData = new PlayerData();
    }

    public void LoadGame() {
        this.playerData = dataHandler.Load();

        if(this.playerData == null) {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in DataPersistenceObjects) {
            dataPersistenceObj.LoadData(playerData);
        }
    }

    public void SaveGame() {
        foreach (IDataPersistence dataPersistenceObj in DataPersistenceObjects) {
            dataPersistenceObj.SaveData(ref playerData);
        }

        dataHandler.Save(playerData);
        StartCoroutine(gm.savedText());
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
