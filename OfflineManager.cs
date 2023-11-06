using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class OfflineManager : MonoBehaviour
{
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI goldText;
        [SerializeField] GameObject offlinePanel;

        private ulong moneyToAdd;

        void Start()
        {
                if (PlayerPrefs.HasKey("LastLogin"))
                {
                        offlinePanel.SetActive(true);
                        DateTime lastLogin = DateTime.Parse(PlayerPrefs.GetString("LastLogin"));

                        TimeSpan ts = DateTime.Now - lastLogin;

                        timeText.text = string.Format("{0} Days {1} Hours {2} Minutes {3} Seconds Ago", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                        goldText.text = Math.Round(ts.TotalMinutes).ToString();
                        moneyToAdd = (ulong)Math.Round(ts.TotalMinutes);
                }
                else
                {
                        offlinePanel.SetActive(false);
                }
        }

        public void ClosePanel()
        {
                Debug.Log(moneyToAdd);
                GoldManager.Instance.AddGold(moneyToAdd);
                offlinePanel?.SetActive(false);
        }

        private void OnApplicationQuit()
        {
                PlayerPrefs.SetString("LastLogin", DateTime.Now.ToString());
        }
}
