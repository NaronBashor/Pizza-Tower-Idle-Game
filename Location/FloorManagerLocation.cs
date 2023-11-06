using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManagerLocation : BaseManagerLocation
{
        public static Action<Level, FloorManagerLocation> OnBoost;

        private Level level;

        private void Start()
        {
                level = GetComponent<Level>();
        }

        public override void RunBoost()
        {
                OnBoost?.Invoke(level, this);
        }
}
