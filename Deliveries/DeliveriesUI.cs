using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveriesUI : MonoBehaviour
{
        public static Action<Deliveries, DeliveryUpgrade> OnUpgradeRequest;

        [SerializeField] private TextMeshProUGUI globalGoldTMP;
        [SerializeField] private TextMeshProUGUI currentLevelTMP;

        private Deliveries deliveries;
        private DeliveryUpgrade deliveryUpgrade;

        private void Start()
        {
                deliveries = GetComponent<Deliveries>();
                deliveryUpgrade = GetComponent<DeliveryUpgrade>();
        }

        private void Update()
        {
                globalGoldTMP.text = GoldManager.Instance.CurrentGold.ToString("C");
        }

        public void UpgradeRequest()
        {
                OnUpgradeRequest?.Invoke(deliveries, deliveryUpgrade);
        }

        private void UpgradeLevel(BaseUpgrade upgrade, int currentLevel)
        {
                if (upgrade == deliveryUpgrade)
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
