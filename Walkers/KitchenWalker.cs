using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenWalker : BaseWalker
{
        public Level CurrentLevel
        {
                get; set;
        }

        Animator anim;
        private int carryingAnimationParamater = Animator.StringToHash("carrying");
        private int walkingAnimationParamater = Animator.StringToHash("walking");

        private void Start()
        {
                anim = GetComponent<Animator>();
                anim.SetBool(walkingAnimationParamater, true);
                anim.SetBool(carryingAnimationParamater, false);
        }

        protected override void CollectGold()
        {
                float collectTime = CarryCapacity / PizzaPerSecond;
                anim.SetBool(carryingAnimationParamater, true);
                anim.SetBool(walkingAnimationParamater, false);
                OnLoading?.Invoke(this, collectTime);
                StartCoroutine(IECollect((ulong)(CarryCapacity * pricePerPizza), collectTime));
        }

        protected override IEnumerator IECollect(ulong collectGold, float collectTime)
        {
                yield return new WaitForSeconds(collectTime);

                CurrentGold = collectGold;
                ChangedGoal();
                RotateWalker(-1);
                MoveMiner(CurrentLevel.DropOffLocation.position);
        }

        protected override void DepositGold()
        {
                CurrentLevel.CurrentDropOff.DropOffGold(CurrentGold);

                CurrentGold = 0;
                ChangedGoal();
                anim.SetBool(carryingAnimationParamater, false);
                anim.SetBool(walkingAnimationParamater, true);
                RotateWalker(1);
                MoveMiner(CurrentLevel.PickupLocation.position);
        }         
}
