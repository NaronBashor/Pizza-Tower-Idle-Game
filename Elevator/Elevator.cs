using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
        [SerializeField] private ElevatorWorker worker;
        [SerializeField] private DropOff elevatorDeposit;
        [SerializeField] private Transform depositLocation;

        public DropOff ElevatorDeposit
        {
                get
                {
                        return elevatorDeposit;
                }
        }
        public Transform DepositLocation
        {
                get
                {
                        return depositLocation;
                }
        }

        public ElevatorWorker Worker
        {
                get
                {
                        return worker;
                }
        }
}
