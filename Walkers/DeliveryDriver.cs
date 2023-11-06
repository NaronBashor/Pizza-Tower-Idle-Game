using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDriver : BaseWalker
{
        private ulong currentGold;

        public DropOff ElevatorDeposit
        {
                get; set;
        }
        public Transform ElevatorDepositLocation
        {
                get; set;
        }
        public Transform DriveawayLocation
        {
                get; set;
        }

        private void Update()
        {
                if (ElevatorDeposit.CurrentGold > 0 && CurrentGold < (ulong)CarryCapacity)
                {
                        if (Vector2.Distance(transform.position, ElevatorDepositLocation.position) < 0.1f)
                        {
                                CollectGold();
                        }                        
                }
        }

        protected override void CollectGold()
        {
                currentGold = ElevatorDeposit.CollectGold(this);
                CurrentGold = currentGold;
                ElevatorDeposit.RemoveGold(CurrentGold);
                ChangedGoal();
                MoveMiner(DriveawayLocation.position);
                StartCoroutine(Delay());
        }

        IEnumerator Delay()
        {
                yield return new WaitForSeconds(4f);

                transform.position = ElevatorDepositLocation.position;
                GoldManager.Instance.AddGold((CurrentGold));
                CurrentGold = 0;
                ChangedGoal();
        }
}
