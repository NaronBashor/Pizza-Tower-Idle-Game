using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManagerLocation : MonoBehaviour
{
        [SerializeField] private string locationTitle;

        public string LocationTitle
        {
                get
                {
                        return locationTitle;
                }
        }

        public Manager Manager
        {
                get; set;
        }

        public virtual void RunBoost()
        {

        }
}
