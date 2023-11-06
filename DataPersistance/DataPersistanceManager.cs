using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
        [Header("File Storafe Config")]
        [SerializeField] private string fileName;

        private GameData gameData;
        private List<iDataPersistance> dataPersistanceObjects;

        private FileDataHandler dataHandler;

        public static DataPersistanceManager Instance
        {
                get; private set;
        }

        private void Awake()
        {
                if (Instance != null)
                {
                        Debug.LogError("More than one Data Persistance Manager found in the scene.");
                }
                Instance = this;
        }

        private void Start()
        {
                this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
                this.dataPersistanceObjects = FindAllDataPersistanceObjects();
                LoadGame();
        }

        public void NewGame()
        {
                this.gameData = new GameData();
        }

        public void LoadGame()
        {
                // Load any saved data from a file using the data handler
                this.gameData = dataHandler.Load();

                // If no data can be loaded, initialize to a new game
                if (this.gameData == null)
                {
                        Debug.Log("No data was found.  Initializing data to defaults.");
                        NewGame();
                }

                // Push the loaded data to all other scripts that need it
                foreach (iDataPersistance dataPersistanceObj in dataPersistanceObjects)
                {
                        dataPersistanceObj.LoadData(gameData);
                }
                Debug.Log("Loaded elevator count is " + gameData.elevatorCurrentLevel);
        }

        public void SaveGame()
        {
                // Pass the data to other scripts so they can update it
                foreach (iDataPersistance dataPersistanceObj in dataPersistanceObjects)
                {
                        dataPersistanceObj.SaveData(ref gameData);
                }

                // Save the data to a file using the data handler
                dataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
                SaveGame();
        }

        private List<iDataPersistance> FindAllDataPersistanceObjects()
        {
                IEnumerable<iDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<iDataPersistance>();

                return new List<iDataPersistance>(dataPersistanceObjects);
        }
}
