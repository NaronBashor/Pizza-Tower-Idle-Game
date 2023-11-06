using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
        public ulong CurrentGold
        {
                get; set;
        }

        public void DropOffGold(ulong amount)
        {
                CurrentGold += amount;
        }

        public void RemoveGold(ulong amount)
        {
                CurrentGold -= amount;
        }

        public ulong CollectGold(BaseWalker walker)
        {
                ulong collectCap = (ulong)walker.CarryCapacity;
                ulong currentGold = walker.CurrentGold;
                ulong carryCapacity = collectCap - currentGold;
                return EvaluateAmountToCollect(carryCapacity);
        }

        private ulong EvaluateAmountToCollect(ulong workerCarryCapacity)
        {
                if (workerCarryCapacity <= CurrentGold)
                {
                        return workerCarryCapacity;
                }
                else
                {
                        return CurrentGold;
                }
        }

        public bool CanCollect()
        {
                return CurrentGold > 0;
        }
}
