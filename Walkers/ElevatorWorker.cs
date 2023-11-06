using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorWorker : BaseWalker
{
        [SerializeField] private Elevator elevator;

        private int currentShaftIndex = -1;
        private DropOff currentDropOff;

        private void Update()
        {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                        MoveToNextLocation();
                }
        }

        public void MoveToNextLocation()
        {
                currentShaftIndex++;

                Level currentLevel = LevelManager.Instance.Levels[currentShaftIndex];
                Vector2 nextPos = currentLevel.DropOffLocation.position;
                Vector2 fixedPos = new Vector2(transform.position.x, nextPos.y);

                currentDropOff = currentLevel.CurrentDropOff;
                MoveMiner(fixedPos);
        }

        protected override void CollectGold()
        {
                if (!currentDropOff.CanCollect() && currentDropOff != null && currentShaftIndex == LevelManager.Instance.Levels.Count - 1)
                {
                        currentShaftIndex = -1;
                        ChangedGoal();
                        Vector3 elevatorDepositPos = new Vector3(transform.position.x, elevator.DepositLocation.position.y);
                        MoveMiner(elevatorDepositPos);
                        return;
                }

                ulong amountToCollect = currentDropOff.CollectGold(this);
                float collectTime = 2f;
                if (OnLoading != null)
                {
                        OnLoading.Invoke(this, collectTime);
                }
                StartCoroutine(IECollect(amountToCollect, collectTime));
        }

        protected override IEnumerator IECollect(ulong collectGold, float collectTime)
        {
                yield return new WaitForSeconds(collectTime);

                if (CurrentGold > 0 && CurrentGold < (ulong)CarryCapacity)
                {
                        CurrentGold += collectGold;
                }
                else
                {
                        CurrentGold = collectGold;
                }
                
                currentDropOff.RemoveGold(collectGold);
                yield return new WaitForSeconds(0.5f);

                if (CurrentGold == (ulong)CarryCapacity || currentShaftIndex == LevelManager.Instance.Levels.Count - 1)
                {
                        currentShaftIndex = -1;
                        ChangedGoal();
                        Vector3 elevatorDepositPos = new Vector3(transform.position.x, elevator.DepositLocation.position.y);
                        MoveMiner(elevatorDepositPos);
                }
                else
                {
                        MoveToNextLocation();
                }
        }

        protected override void DepositGold()
        {
                if (CurrentGold <= 0)
                {
                        currentShaftIndex = -1;
                        ChangedGoal();
                        MoveToNextLocation();
                        return;
                }

                float depositTime = 0;
                if (OnLoading != null)
                {
                        OnLoading.Invoke(this, depositTime);
                }
                StartCoroutine(IEDeposit(CurrentGold, depositTime));
        }

        protected override IEnumerator IEDeposit(ulong goldCollected, float depositTime)
        {
                yield return new WaitForSeconds(depositTime);

                elevator.ElevatorDeposit.DropOffGold(CurrentGold);
                CurrentGold = 0;
                currentShaftIndex = -1;

                ChangedGoal();
                MoveToNextLocation();
        }

        private void ElevatorBoost(ElevatorManagerLocation elevatorManager)
        {
                switch (elevatorManager.Manager.boostType)
                {
                        case BoostType.Movement:
                                ManagersController.Instance.RunMovementBoost(this, elevatorManager.Manager.boostDuration, elevatorManager.Manager.boostValue);
                                break;

                        case BoostType.Loading:
                                ManagersController.Instance.RunLoadingBoost(this, elevatorManager.Manager.boostDuration, elevatorManager.Manager.boostValue);
                                break;
                }
        }

        private void OnEnable()
        {
                ElevatorManagerLocation.OnBoost += ElevatorBoost;
        }

        private void OnDisable()
        {
                ElevatorManagerLocation.OnBoost -= ElevatorBoost;
        }
}
