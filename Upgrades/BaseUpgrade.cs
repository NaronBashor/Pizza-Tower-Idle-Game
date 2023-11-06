using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Search;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
        public static Action<BaseUpgrade, int> OnUpgrade;

        [Header("Upgrades")]
        [SerializeField] public float carryCapacityMultiplier = 2f;
        [SerializeField] public float pizzaPerSecondMultiplier = 1.25f;
        [SerializeField] public float moveSpeedMultiplier = 4f;

        [Header("Cost")]
        [SerializeField] private float initialUpgradeCost = 200;
        [SerializeField] private float upgradeCostMultiplier = 2;

        public float CarryCapacityMultiplier
        {
                get
                {
                        return this.carryCapacityMultiplier;
                }
        }
        public float PizzaPerSecondMultiplier
        {
                get
                {
                        return this.pizzaPerSecondMultiplier;
                }
        }
        public float MoveSpeedMultiplier
        {
                get
                {
                        return this.moveSpeedMultiplier;
                }
        }
        public float UpgradeCostMultiplier
        {
                get
                {
                        return this.upgradeCostMultiplier;
                }
        }
        public Elevator Elevator => elevator;

        public int CurrentLevel
        {
                get; set;
        }

        public float UpgradeCost
        {
                get; set;
        }

        protected Level level;
        protected Elevator elevator;
        protected Deliveries deliveries;

        void Start()
        {
                UpgradeCost = initialUpgradeCost;
                level = GetComponent<Level>();
                elevator = GetComponent<Elevator>();
                deliveries = GetComponent<Deliveries>();
        }

        public virtual void Upgrade(int upgradeAmount)
        {
                if (upgradeAmount > 0)
                {
                        for (int i = 0; i < upgradeAmount; i++)
                        {
                                UpgradeSuccess();
                                UpdateUpgradeValues();
                                RunUpgrade();
                        }
                }
        }

        protected virtual void UpgradeSuccess()
        {
                GoldManager.Instance.RemoveGold((ulong)UpgradeCost);
                CurrentLevel++;
                OnUpgrade?.Invoke(this, CurrentLevel);
        }

        protected virtual void UpdateUpgradeValues()
        {
                UpgradeCost *= upgradeCostMultiplier;
        }

        protected virtual void RunUpgrade()
        {
        }
}
