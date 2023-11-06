using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpgrade : BaseUpgrade
{
        public int walkerLevelsAdded;

        public int WalkerLevelsAdded
        {
                get
                {
                        return walkerLevelsAdded;
                }
        }

        private void Update()
        {
                walkerLevelsAdded = CurrentLevel;
        }

        protected override void RunUpgrade()
        {
                if (level != null)
                {
                        if (CurrentLevel % 10 ==0)
                        {
                                level.CreateWalker();
                        }
                        if (CurrentLevel == 10)
                        {
                                level.CreateManager();
                        }
                        foreach (KitchenWalker walker in level.Walkers)
                        {
                                walker.CarryCapacity *= (int)carryCapacityMultiplier;
                                walker.PizzaPerSecond *= (int)pizzaPerSecondMultiplier;

                                if (CurrentLevel % 10  == 0)
                                {
                                        walker.MoveSpeed *= moveSpeedMultiplier;
                                }                                
                        }
                }
        }
}
