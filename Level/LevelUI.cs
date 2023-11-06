using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelUI : MonoBehaviour
{
        public static Action<Level, LevelUpgrade> OnUpgradeRequest;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI currentGoldTMP;
        [SerializeField] private TextMeshProUGUI currentLevelTMP;
        [SerializeField] private TextMeshProUGUI nextUpgradeTMP;

        private Level level;
        private BaseUpgrade baseUpgrade;
        private LevelUpgrade levelUpgrade;

        public int PowerIndex
        {
                get
                {
                        return LevelManager.Instance.Index;
                }
        }

        private void Start()
        {
                level = GetComponent<Level>();
                baseUpgrade = GetComponent<BaseUpgrade>();
                levelUpgrade = GetComponent<LevelUpgrade>();

                nextUpgradeTMP.text = (LevelManager.Instance.NewLevelCost * ((int)Math.Pow(5, PowerIndex))).ToString("C");
        }

        private void Update()
        {
                currentGoldTMP.text = level.CurrentDropOff.CurrentGold.ToString("C");
                nextUpgradeTMP.text = (LevelManager.Instance.NewLevelCost * ((int)Math.Pow(5, PowerIndex))).ToString("C");
        }

        public void BuyNewLevel()
        {
                if (GoldManager.Instance.CurrentGold > (ulong)LevelManager.Instance.NewLevelCost)
                {
                        GoldManager.Instance.RemoveGold((ulong)LevelManager.Instance.NewLevelCost * (ulong)(Math.Pow(5, PowerIndex)));
                        LevelManager.Instance.AddLevel();
                        LevelManager.Instance.Index++;
                }
        }

        public void UpgradeRequest()
        {
                OnUpgradeRequest?.Invoke(level, levelUpgrade);
        }

        private void UpgradeLevel(BaseUpgrade upgrade, int currentLevel)
        {
                if (upgrade == levelUpgrade)
                {
                        currentLevelTMP.text = $"Level\n{currentLevel}";
                }
        }

        private void OnEnable()
        {
                LevelUpgrade.OnUpgrade += UpgradeLevel;
        }

        private void OnDisable()
        {
                LevelUpgrade.OnUpgrade -= UpgradeLevel;
        }
}
