using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
        [SerializeField] private BaseManagerLocation location;

        public BaseManagerLocation Location
        {
                get
                {
                        return location;
                }
        }

        public FloorManagerLocation FloorManagerLocation
        {
                get; set;
        }

        public void SetupManager(BaseManagerLocation managerLocation)
        {
                location = managerLocation;
                FloorManagerLocation = managerLocation as FloorManagerLocation;
        }

        public void RunBoost()
        {
                FloorManagerLocation.RunBoost();
        }
}
