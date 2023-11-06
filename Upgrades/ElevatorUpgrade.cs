using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade, iDataPersistance
{
        [SerializeField] private GameObject elevatorManger;
        [SerializeField] private GameObject upgradeManager;
        [SerializeField] private GameObject elevators;

        private ElevatorUpgrade elevatorUpgrade;

        public void LoadData(GameData data)
        {
                this.CurrentLevel = data.elevatorCurrentLevel;
                elevatorManger.GetComponent<ElevatorUI>().UpgradeLevel(elevatorUpgrade, data.elevatorCurrentLevel);
                for (int i = 1; i <= this.CurrentLevel; i++)
                {
                        RunUpgrade();
                        if (i % 10 == 0)
                        {
                                elevators.GetComponent<ElevatorWorker>().MoveSpeed *= 1.25f;
                        }
                }
        }

        public void SaveData(ref GameData data)
        {
                data.elevatorCurrentLevel = this.CurrentLevel;
        }

        protected override void RunUpgrade()
        {
                elevators.GetComponent<ElevatorWorker>().CarryCapacity *= CarryCapacityMultiplier;
                elevators.GetComponent<ElevatorWorker>().PizzaPerSecond *= PizzaPerSecondMultiplier;

                if (this.CurrentLevel % 10 == 0)
                {
                        elevators.GetComponent<ElevatorWorker>().MoveSpeed *= MoveSpeedMultiplier;
                }
        }
}
