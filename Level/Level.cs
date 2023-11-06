using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
        [Header("Prefabs")]
        [SerializeField] private KitchenWalker walkerPrefab;
        [SerializeField] private DropOff dropOffPrefab;

        [Header("Locations")]
        [SerializeField] private Transform dropOffLocation;
        [SerializeField] private Transform pickUpLocation;
        [SerializeField] private Transform dropOffInstantiateLocation;

        [Header("Manager")]
        [SerializeField] private Transform managerPos;
        [SerializeField] private GameObject managerPrefab;

        [SerializeField] private Vector3 mangerDeskLocation;

        public int LevelID
        {
                get; set;
        }

        private GameObject walkersContainer;
        private List<KitchenWalker> walkers;
        private FloorManagerLocation floorManagerLocation;

        public Transform PickupLocation
        {
                get
                {
                        return pickUpLocation;
                }
        }

        public Transform DropOffLocation
        {
                get
                {
                        return dropOffLocation;
                }
        }
        public List<KitchenWalker> Walkers
        {
                get
                {
                        return walkers;
                }
        }

        public DropOff CurrentDropOff
        {
                get; set;
        }

        private void Start()
        {
                floorManagerLocation = GetComponent<FloorManagerLocation>();
                walkersContainer = new GameObject("Walkers");

                walkers = new List<KitchenWalker>();

                CreateWalker();
                CreateDropOff();
        }

        public void CreateWalker()
        {
                KitchenWalker newWalker = Instantiate(walkerPrefab, dropOffLocation.position, Quaternion.identity);
                newWalker.CurrentLevel = this;
                newWalker.transform.SetParent(walkersContainer.transform);
                newWalker.MoveMiner(pickUpLocation.position);

                if (walkers.Count  > 0)
                {
                        newWalker.CarryCapacity = walkers[0].CarryCapacity;
                        newWalker.PizzaPerSecond = walkers[0].PizzaPerSecond;
                        newWalker.MoveSpeed = walkers[0].MoveSpeed;
                }

                walkers.Add(newWalker);
        }

        public void CreateManager()
        {
                GameObject levelManager = Instantiate(managerPrefab, managerPos.position + mangerDeskLocation, Quaternion.identity);
                StoreManager storeManager = levelManager.GetComponent<StoreManager>();
                storeManager.SetupManager(floorManagerLocation);
        }

        private void CreateDropOff()
        {
                CurrentDropOff = Instantiate(dropOffPrefab, dropOffInstantiateLocation.position, Quaternion.identity);
                CurrentDropOff.transform.SetParent(dropOffInstantiateLocation);
        }

        private void FloorBoost(Level level, FloorManagerLocation floorManager)
        {
                if (level == this)
                {
                        switch (floorManager.Manager.boostType)
                        {
                                case BoostType.Movement:
                                        foreach (KitchenWalker walker in walkers)
                                        {
                                                ManagersController.Instance.RunMovementBoost(walker, floorManager.Manager.boostDuration, floorManager.Manager.boostValue);
                                        }
                                        break;

                                case BoostType.Loading:
                                        foreach (KitchenWalker walker in walkers)
                                        {
                                                ManagersController.Instance.RunLoadingBoost(walker, floorManager.Manager.boostDuration, floorManager.Manager.boostValue);
                                        }
                                        break;
                        }
                }
        }

        private void OnEnable()
        {
                FloorManagerLocation.OnBoost += FloorBoost;
        }

        private void OnDisable()
        {
                FloorManagerLocation.OnBoost -= FloorBoost;
        }
}
