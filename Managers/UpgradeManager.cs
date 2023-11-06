using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
        #region Inspector
        [Header("Panel Options")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private TextMeshProUGUI panelTitle;
        [SerializeField] private GameObject[] stats;

        [Header("Button Colors")]
        [SerializeField] private Color buttonDisableColor;
        [SerializeField] private Color buttonEnableColor;

        [Header("Buttons")]
        [SerializeField] private GameObject[] upgradeButtons;

        [Header("Text")]
        [SerializeField] public TextMeshProUGUI upgradeCost;
        [SerializeField] private TextMeshProUGUI currentStat1;
        [SerializeField] private TextMeshProUGUI currentStat2;
        [SerializeField] private TextMeshProUGUI currentStat3;
        [SerializeField] private TextMeshProUGUI currentStat4;
        [SerializeField] private TextMeshProUGUI stat1Title;
        [SerializeField] private TextMeshProUGUI stat2Title;
        [SerializeField] private TextMeshProUGUI stat3Title;
        [SerializeField] private TextMeshProUGUI stat4Title;

        [Header("Upgraded")]
        [SerializeField] public TextMeshProUGUI statUpgraded1;
        [SerializeField] public TextMeshProUGUI statUpgraded2;
        [SerializeField] public TextMeshProUGUI statUpgraded3;
        [SerializeField] public TextMeshProUGUI statUpgraded4;

        [Header("Button Anchor Locations")]
        [SerializeField] private Transform[] buttonLocations;

        [Header("Images")]
        [SerializeField] private Image icon;
        [SerializeField] private Image image;

        [Header("Level Icons")]
        [SerializeField] private Sprite cookingIcon;
        [SerializeField] private Sprite walkerIcon;
        [SerializeField] private Sprite elevatorIcon;
        [SerializeField] private Sprite deliveryIcon;

        [Header("Level Images")]
        [SerializeField] private Sprite kitchenImage;
        [SerializeField] private Sprite deliveryImage;

        #endregion

        public int TimesToUpgrade
        {
                get; set;
        }

        private Level selectedLevel;
        private Deliveries currentDelivery;
        private LevelUpgrade selectedLevelUpgrade;
        private BaseUpgrade currentUpgrade;

        private void Start()
        {
                ActivateButton(0);
                TimesToUpgrade = 1;
        }

        private void Update()
        {
                for (int i = 0; i < upgradeButtons.Length; i++)
                {
                        upgradeButtons[i].transform.position = buttonLocations[i].position;
                        if (i == upgradeButtons.Length)
                        {
                                i = 0;
                                return;
                        }
                }
        }

        public void Upgrade()
        {
                if (GoldManager.Instance.CurrentGold >= currentUpgrade.UpgradeCost)
                {
                        currentUpgrade.Upgrade(TimesToUpgrade);

                        if (currentUpgrade is LevelUpgrade)
                        {
                                UpdateUpgradePanel(currentUpgrade);
                        }
                        if (currentUpgrade is ElevatorUpgrade)
                        {
                                UpdateElevatorPanel(currentUpgrade);
                        }
                        if (currentUpgrade is DeliveryUpgrade)
                        {
                                UpdateDeliveryPanel(currentUpgrade);
                        }
                }
        }

        public void OpenUpgradePanel(bool status)
        {
                upgradePanel.SetActive(status);
        }

        #region Upgrade Buttons
        public void UpgradeX1()
        {
                ActivateButton(0);
                TimesToUpgrade = 1;
                upgradeCost.text = Currency.DisplayCurrency((ulong)GetUpgradeCost(1, currentUpgrade));
        }

        public void UpgradeX5()
        {
                ActivateButton(1);
                if (CanUpgradeManyTimes(5, currentUpgrade))
                {
                        TimesToUpgrade = 5;
                }
                else
                {
                        TimesToUpgrade = 0;
                }
                upgradeCost.text = Currency.DisplayCurrency((ulong)GetUpgradeCost(5, currentUpgrade));
        }

        public void UpgradeX10()
        {
                ActivateButton(2);
                if (CanUpgradeManyTimes(10, currentUpgrade))
                {
                        TimesToUpgrade = 10;
                }
                else
                {
                        TimesToUpgrade = 0;
                }
                upgradeCost.text = Currency.DisplayCurrency((ulong)GetUpgradeCost(10, currentUpgrade));
        }

        public void UpgradeXMax()
        {
                ActivateButton(3);
                TimesToUpgrade = CalculateUpgradeCount(currentUpgrade);
                upgradeCost.text = Currency.DisplayCurrency((ulong)GetUpgradeCost(TimesToUpgrade, currentUpgrade));
        }
        #endregion

        #region Help Methods
        public void ActivateButton(int buttonIndex)
        {
                for (int i = 0; i < upgradeButtons.Length; i++)
                {
                        upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
                }
                upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;
                upgradeButtons[buttonIndex].transform.DOPunchPosition(new Vector3(0f, 5f, 0f), .5f).Play();
        }

        private int GetUpgradeCost(int amount, BaseUpgrade upgrade)
        {
                int cost = 0;
                int upgradeCost = (int)upgrade.UpgradeCost;

                for (int i = 0; i < amount; i++)
                {
                        cost += upgradeCost;
                        upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
                }
                return cost;
        }

        public bool CanUpgradeManyTimes(int upgradeAmout, BaseUpgrade upgrade)
        {
                int count = (int)CalculateUpgradeCount(upgrade);
                if (count > upgradeAmout)
                {
                        return true;
                }
                else
                {
                        return false;
                }
        }

        public int CalculateUpgradeCount(BaseUpgrade upgrade)
        {
                int count = 0;
                ulong currentGold = GoldManager.Instance.CurrentGold;
                int upgradeCost = (int)upgrade.UpgradeCost;
                for (int i = (int)currentGold; i >= 0; i -= upgradeCost)
                {
                        count++;
                        upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
                }
                return count;
        }
        #endregion

        #region Update Deliveries Panel

        private void UpdateDeliveryPanel(BaseUpgrade upgrade)
        {
                panelTitle.text = $"Delivery\n Level {upgrade.CurrentLevel}";
                upgradeCost.text = $"{upgrade.UpgradeCost}";
                upgradeCost.text = Currency.DisplayCurrency((ulong)upgrade.UpgradeCost);

                // Update icons
                icon.sprite = deliveryIcon;
                image.sprite = deliveryImage;

                // Update titles
                stat1Title.text = "Drivers";
                stat2Title.text = "Carry Capacity";
                stat3Title.text = "Load Speed";
                stat4Title.text = "Move Speed";

                // Update curretn values
                currentStat1.text = $"{currentDelivery.Drivers.Count}";
                currentStat2.text = Math.Round(currentDelivery.Drivers[0].CarryCapacity, 2).ToString("N");
                currentStat3.text = currentDelivery.Drivers[0].PizzaPerSecond.ToString("N");
                currentStat4.text = Math.Round(currentDelivery.Drivers[0].MoveSpeed, 2).ToString("N");

                // Update drivers count
                if ((upgrade.CurrentLevel + 1) % 10 == 0)
                {
                        statUpgraded1.text = $"+ 1";
                }
                else
                {
                        statUpgraded1.text = $"+ 0";
                }

                // Update delivery
                float carryCapacity = currentDelivery.Drivers[0].CarryCapacity;
                float carryCapacityMTP = upgrade.CarryCapacityMultiplier;
                float carryCapacityAdded = Mathf.Abs(carryCapacity - (carryCapacity * carryCapacityMTP));
                statUpgraded2.text = $"{carryCapacityAdded}";

                // Update load speed
                float loadingSpeed = currentDelivery.Drivers[0].PizzaPerSecond;
                float loadingSpeedMTP = upgrade.PizzaPerSecondMultiplier;
                float loadingAdded = Mathf.Abs(loadingSpeed - (loadingSpeed * loadingSpeedMTP));
                statUpgraded3.text = Math.Round(loadingAdded, 2).ToString("N") + " / sec";

                // Update move speed
                float currentMoveSpeed = currentDelivery.Drivers[0].MoveSpeed;
                float moveSpeedMTP = upgrade.MoveSpeedMultiplier;
                float moveSpeedAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * moveSpeedMTP));
                if ((upgrade.CurrentLevel + 1) % 10 == 0)
                {
                        statUpgraded4.text = Math.Round(moveSpeedAdded, 2).ToString("N") + " / sec";
                }
                else
                {
                        statUpgraded4.text = $"+ 0 / sec";
                }
        }

        #endregion

        #region Update Elevator Panel

        public void UpdateElevatorPanel(BaseUpgrade upgrade)
        {
                ElevatorWorker elevatorWorker = upgrade.Elevator.Worker;
                panelTitle.text = $"Elevator Level\n{upgrade.CurrentLevel}";

                stats[stats.Length - 1].SetActive(false);
                icon.sprite = elevatorIcon;
                image.sprite = kitchenImage;

                upgradeCost.text = $"{upgrade.UpgradeCost}";
                upgradeCost.text = Currency.DisplayCurrency((ulong)upgrade.UpgradeCost);
                stat1Title.text = "Load";
                stat2Title.text = "Move Speed";
                stat3Title.text = "Load Speed";

                currentStat1.text = Math.Round(elevatorWorker.CarryCapacity, 2).ToString("N");
                currentStat2.text = Math.Round(elevatorWorker.MoveSpeed, 2).ToString("N");
                currentStat3.text = Math.Round(elevatorWorker.PizzaPerSecond, 2).ToString("N");

                // Update load capacity
                float currentloadCapacity = elevatorWorker.CarryCapacity;
                float loadMTP = upgrade.CarryCapacityMultiplier;
                float loadCapacityAdded = Mathf.Abs(currentloadCapacity - (currentloadCapacity * loadMTP));
                statUpgraded1.text = loadCapacityAdded.ToString("N");

                // Update move speed
                float currentMoveSpeed = elevatorWorker.MoveSpeed;
                float moveSpeedMTP = upgrade.MoveSpeedMultiplier;                
                float moveSpeedAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * moveSpeedMTP));
                if ((upgrade.CurrentLevel + 1) % 10 == 0)
                {
                        statUpgraded2.text = Math.Round(moveSpeedAdded, 2).ToString("N") + " / sec";
                }
                else
                {
                        statUpgraded2.text = $"+ 0 / sec";
                }

                // Update load speed
                float loadingSpeed = elevatorWorker.PizzaPerSecond;
                float loadingSpeedMTP = upgrade.PizzaPerSecondMultiplier;
                float loadingAdded = Mathf.Abs(loadingSpeed - (loadingSpeed * loadingSpeedMTP));
                statUpgraded3.text = Math.Round(loadingAdded, 2).ToString("N") + " / sec";
        }

        #endregion

        #region Update Level Panel
        private void UpdateUpgradePanel(BaseUpgrade upgrade)
        {
                panelTitle.text = $"Floor {selectedLevel.LevelID + 1}\n Level {upgrade.CurrentLevel}";

                upgradeCost.text = $"{upgrade.UpgradeCost}";
                upgradeCost.text = Currency.DisplayCurrency((ulong)upgrade.UpgradeCost);
                currentStat1.text = $"{selectedLevel.Walkers.Count}";
                currentStat2.text = Math.Round(selectedLevel.Walkers[0].MoveSpeed, 2).ToString("N");
                currentStat3.text = Math.Round(selectedLevel.Walkers[0].PizzaPerSecond, 2).ToString("N");
                currentStat4.text = Math.Round(selectedLevel.Walkers[0].CarryCapacity, 2).ToString("N");

                stats[stats.Length - 1].SetActive(true);
                icon.sprite = walkerIcon;
                image.sprite = kitchenImage;

                stat1Title.text = "Workers";
                stat2Title.text = "Walking Speed";
                stat3Title.text = "Load Speed";
                stat4Title.text = "Worker Capacity";

                //Upgrade carry capacity
                float carryCapacity = selectedLevel.Walkers[0].CarryCapacity;
                float carryCapacityMTP = upgrade.CarryCapacityMultiplier;
                float carryCapacityAdded = Mathf.Abs(carryCapacity - (carryCapacity * carryCapacityMTP));
                statUpgraded4.text = $"+ {carryCapacityAdded}";

                //Upgrade load speed
                int currentLoadSpeed = (int)selectedLevel.Walkers[0].PizzaPerSecond;
                float currentLoadSpeedMTP = upgrade.PizzaPerSecondMultiplier;
                int loadSpeedAdded = Mathf.Abs(currentLoadSpeed - (int)(currentLoadSpeed * currentLoadSpeedMTP));
                statUpgraded3.text = $"+ {loadSpeedAdded}";

                //Upgrade move speed
                int walkSpeed = (int)selectedLevel.Walkers[0].MoveSpeed;
                float walkSpeedMTP = upgrade.MoveSpeedMultiplier;
                float walkSpeedAdded = Mathf.Abs(walkSpeed - (walkSpeed * walkSpeedMTP));
                if ((upgrade.CurrentLevel + 1) % 10 == 0)
                {
                        statUpgraded2.text = $"+ {walkSpeedAdded} / sec";
                }
                else
                {
                        statUpgraded2.text = $"+ 0 / sec";
                }

                // Upgrade elevatorWorker count
                if ((upgrade.CurrentLevel + 1) % 10 == 0)
                {
                        statUpgraded1.text = $"+ 1";
                }
                else
                {
                        statUpgraded1.text = $"+ 0";
                }
        }
        #endregion

        #region Events
        private void LevelUpgradeRequest(Level level, LevelUpgrade upgrade)
        {
                List<Level> levelList = LevelManager.Instance.Levels;
                for (int i = 0; i < levelList.Count; i++)
                {
                        if (level.LevelID == levelList[i].LevelID)
                        {
                                selectedLevel = levelList[i];
                                selectedLevelUpgrade = levelList[i].GetComponent<LevelUpgrade>();
                        }
                }
                currentUpgrade = upgrade;
                OpenUpgradePanel(true);
                UpdateUpgradePanel(selectedLevelUpgrade);
        }

        public void ElevatorUpgradeRequest(ElevatorUpgrade elevatorUpgrade)
        {
                currentUpgrade = elevatorUpgrade;
                OpenUpgradePanel(true);
                UpdateElevatorPanel(elevatorUpgrade);
        }

        private void DeliveryUpgradeRequest(Deliveries deliveries, DeliveryUpgrade deliveryUpgrade)
        {
                currentUpgrade = deliveryUpgrade;
                currentDelivery = deliveries;
                OpenUpgradePanel(true);
                UpdateDeliveryPanel(deliveryUpgrade);
        }

        private void OnEnable()
        {
                LevelUI.OnUpgradeRequest += LevelUpgradeRequest;
                ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
                DeliveriesUI.OnUpgradeRequest += DeliveryUpgradeRequest;
        }        

        private void OnDisable()
        {
                LevelUI.OnUpgradeRequest -= LevelUpgradeRequest;
                ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
                DeliveriesUI.OnUpgradeRequest -= DeliveryUpgradeRequest;
        }
        #endregion        
}
