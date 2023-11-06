using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryUpgrade : BaseUpgrade
{
        public int currentdeliveryLevels;
        public int deliveryLevelsAdded;

        private void Update()
        {
                currentdeliveryLevels = CurrentLevel;
        }

        protected override void RunUpgrade()
        {
                if (CurrentLevel % 10 == 0)
                {
                        deliveries.AddDriver();
                }

                foreach (DeliveryDriver driver in deliveries.Drivers)
                {
                        driver.CarryCapacity *= carryCapacityMultiplier;
                        driver.PizzaPerSecond *= pizzaPerSecondMultiplier;

                        if (CurrentLevel % 10 == 0)
                        {
                                driver.MoveSpeed *= moveSpeedMultiplier;
                        }
                }
        }
}
