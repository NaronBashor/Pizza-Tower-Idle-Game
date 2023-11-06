using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
        [Header("Stats")]
        [SerializeField] private Level levelPrefab;
        [SerializeField] private GameObject blankFloorPrefab;
        [SerializeField] private float newLevelYPos;
        [SerializeField] private int newLevelCost = 500;
        [SerializeField] private float moveHeight;
        [SerializeField] private GameObject elevator;
        [SerializeField] private GameObject elevatorPole;
        [SerializeField] private int currentLevelIndex;

        [Header("Levels")]
        [SerializeField] public List<Level> levels = new List<Level>();

        public List<Level> Levels => levels;

        public int CurrentLevelIndex
        {
                get
                {
                        return currentLevelIndex;
                }
        }

        public int Index
        {
                get; set;
        }

        public int NewLevelCost
        {
                get
                {
                        return newLevelCost;
                }
        }

        private void Start()
        {
                Index = 1;
                levels[0].LevelID = 0;
        }

        public void AddLevel()
        {
                Transform lastLevel = levels[currentLevelIndex].transform;
                Level newLevel = Instantiate(levelPrefab, lastLevel.position, Quaternion.identity);
                newLevel.transform.localPosition = new Vector3(lastLevel.position.x, lastLevel.position.y + newLevelYPos, lastLevel.position.z);

                blankFloorPrefab.transform.position = blankFloorPrefab.transform.position + new Vector3(0, moveHeight, 0);

                Vector3 elevatorLocalScale = new Vector3(0, 1.995f, 0);
                Vector3 elevatorHoleLocalScale = new Vector3(0, 0.82f, 0);
                elevator.transform.localScale = elevator.transform.localScale + elevatorLocalScale;
                elevatorPole.transform.localScale = elevatorPole.transform.localScale + elevatorHoleLocalScale;

                currentLevelIndex++;

                newLevel.LevelID = currentLevelIndex;
                levels.Add(newLevel);

        }
}
