using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
        public static Action<ElevatorUpgrade> OnUpgradeRequest;

        [SerializeField] private TextMeshProUGUI elevatorDepositGold;
        [SerializeField] private TextMeshProUGUI currentLevelTMP;

        private Elevator elevator;
        private ElevatorUpgrade elevatorUpgrade;

        void Start()
        {
                elevatorUpgrade = GetComponent<ElevatorUpgrade>();
                elevator = GetComponent<Elevator>();
        }

        void Update()
        {
                elevatorDepositGold.text = elevator.ElevatorDeposit.CurrentGold.ToString("C");
        }

        public void RequestUpgrade()
        {
                OnUpgradeRequest?.Invoke(elevatorUpgrade);
        }

        public void UpgradeLevel(BaseUpgrade upgrade, int currentLevel)
        {
                if (upgrade == elevatorUpgrade)
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
