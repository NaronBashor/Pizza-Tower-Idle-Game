using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
        public int elevatorCurrentLevel;

        public void LoadData(GameData data)
        {
                this.elevatorCurrentLevel = data.elevatorCurrentLevel;
        }

        public void SaveData(ref GameData data)
        {
                data.elevatorCurrentLevel = this.elevatorCurrentLevel;
        }
}
