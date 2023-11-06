using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliveries : MonoBehaviour
{
        [Header("Prefab")]
        [SerializeField] private GameObject deliveryDriverPrefab;

        [Header("Info")]
        [SerializeField] private DropOff elevatorDropOff;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Transform driveAwayLocation;
        [SerializeField] private List<DeliveryDriver> drivers;

        public List<DeliveryDriver> Drivers => drivers;

        void Start()
        {
                drivers = new List<DeliveryDriver>();
                AddDriver();
        }

        private void Update()
        {
                if (Vector2.Distance(transform.position, driveAwayLocation.position) < 0.1f)
                {
                        drivers[0].transform.position = spawnLocation.position;
                }
        }

        public void AddDriver()
        {
                GameObject newDriver = Instantiate(deliveryDriverPrefab, spawnLocation.position, Quaternion.identity);
                DeliveryDriver driver = newDriver.GetComponent<DeliveryDriver>();
                driver.ElevatorDeposit = elevatorDropOff;
                driver.ElevatorDepositLocation = spawnLocation;
                driver.DriveawayLocation = driveAwayLocation;

                drivers.Add(driver);
        }

        private void DeliveryDriverBoost(DeliveryManagerLocation deliveryManager)
        {
                switch (deliveryManager.Manager.boostType)
                {
                        case BoostType.Movement:
                                foreach (DeliveryDriver driver in Drivers)
                                {
                                        ManagersController.Instance.RunMovementBoost(driver, deliveryManager.Manager.boostDuration, deliveryManager.Manager.boostValue);
                                }
                                break;
                        case BoostType.Loading:
                                foreach (DeliveryDriver driver in Drivers)
                                {
                                        ManagersController.Instance.RunLoadingBoost(driver, deliveryManager.Manager.boostDuration, deliveryManager.Manager.boostValue);
                                }
                                break;
                }
        }

        private void OnEnable()
        {
                DeliveryManagerLocation.OnBoost += DeliveryDriverBoost;
        }        

        private void OnDisable()
        {
                DeliveryManagerLocation.OnBoost -= DeliveryDriverBoost;
        }
}
