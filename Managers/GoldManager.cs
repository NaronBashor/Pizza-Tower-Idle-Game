using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
        [SerializeField] private int testGold = 0;

        public ulong CurrentGold
        {
                get; set;
        }

        private readonly string Gold_Key = "Goldkey";

        void Start()
        {
                PlayerPrefs.DeleteAll();
                LoadGold();
        }

        public void LoadGold()
        {
                CurrentGold = (ulong)PlayerPrefs.GetInt(Gold_Key, testGold);
        }

        public void AddGold(ulong amount)
        {
                CurrentGold += amount;
                PlayerPrefs.SetInt(Gold_Key, (int)amount);
                PlayerPrefs.Save();
        }

        public void RemoveGold(ulong amount)
        {
                CurrentGold -= amount;
                PlayerPrefs.SetInt(Gold_Key, (int)amount);
                PlayerPrefs.Save();
        }
}
