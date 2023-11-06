using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerLevel
{
        Supervisor,
        Manager,
        GeneralManger
}

public enum BoostType
{
        Movement,
        Loading
}

[CreateAssetMenu]
public class Manager : ScriptableObject
{
        [Header("Manager Info")]
        public ManagerLevel managerLevel;
        public Color levelColor;
        public Sprite managerIcon;
        public string managerName;

        [Header("Boost Info")]
        public BoostType boostType;
        public Sprite boostIcon;
        public float boostDuration;
        public float boostValue;
        public string boostDescription;
}
