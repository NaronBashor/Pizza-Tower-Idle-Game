using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{
        [SerializeField] private GameObject loadingBarPrefab;
        [SerializeField] private Transform loadingBarPos;

        private Image fillImage;
        private BaseWalker walker;
        private GameObject barCanvas;

        void Start()
        {
                walker = GetComponent<BaseWalker>();
                CreateLoadBar();
        }

        private void CreateLoadBar()
        {
                barCanvas = Instantiate(loadingBarPrefab, loadingBarPos.position, Quaternion.identity);
                barCanvas.transform.SetParent(transform);
                fillImage = barCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        }

        private void LoadingBar(BaseWalker walkerSender, float duration)
        {
                if (walker == walkerSender)
                {
                        barCanvas.gameObject.SetActive(true);
                        fillImage.fillAmount = 0;
                        fillImage.DOFillAmount(1, duration).OnComplete((() => barCanvas.SetActive(false)));
                }
        }

        private void OnEnable()
        {
                BaseWalker.OnLoading += LoadingBar;
        }        

        private void OnDisable()
        {
                BaseWalker.OnLoading -= LoadingBar;
        }
}
