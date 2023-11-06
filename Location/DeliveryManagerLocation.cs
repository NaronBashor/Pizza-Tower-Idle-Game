using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerLocation : BaseManagerLocation
{
        public static Action<DeliveryManagerLocation> OnBoost;

        public override void RunBoost()
        {
                OnBoost?.Invoke(this);
        }
}
