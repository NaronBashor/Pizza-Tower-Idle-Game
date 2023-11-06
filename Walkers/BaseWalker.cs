using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BaseWalker : MonoBehaviour
{
        public static Action<BaseWalker, float> OnLoading;

        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float initialCarryCapacity = 2;
        [SerializeField] private float pizzaPerSecond = 1;
        [SerializeField] public int pricePerPizza = 1;

        public float CarryCapacity
        {
                get; set;
        }

        public float PizzaPerSecond
        {
                get; set;
        }

        public ulong CurrentGold
        {
                get; set;
        }

        public bool IsTimeToCollect
        {
                get; set;
        }
        public float MoveSpeed
        {
                get; set;
        }

        private void Awake()
        {
                IsTimeToCollect = true;

                CarryCapacity = initialCarryCapacity;
                PizzaPerSecond = pizzaPerSecond;
                MoveSpeed = moveSpeed;
                CurrentGold = 0;
        }

        public void MoveMiner(Vector3 newPos)
        {
                transform.DOMove(newPos, 10f / MoveSpeed).OnComplete((() =>
                {
                        if (IsTimeToCollect)
                        {
                                CollectGold();
                        }
                        else
                        {
                                DepositGold();
                        }
                })).Play();
        }

        protected virtual void CollectGold()
        {

        }

        protected virtual IEnumerator IECollect(ulong collectGold, float collectTime)
        {
                yield return null;
        }

        protected virtual void DepositGold()
        {

        }

        protected virtual IEnumerator IEDeposit(ulong goldCollected, float depositTime)
        {
                yield return null;
        }

        public void RotateWalker(int direction)
        {
                if (direction == 1)
                {
                        transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                        transform.localScale = new Vector3(-1, 1, 1);
                }
        }

        public void ChangedGoal()
        {
                IsTimeToCollect = !IsTimeToCollect;
        }
}
