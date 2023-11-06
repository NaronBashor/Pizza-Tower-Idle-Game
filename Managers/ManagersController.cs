using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagersController : Singleton<ManagersController>
{
        [SerializeField] private ManagerCard managerCardPrefab;
        [SerializeField] private int intialManagerCost = 100;
        [SerializeField] private int managerCostMultiplier = 3;

        [Header("Assign Manager UI")]
        [SerializeField] private Image managerIcon;
        [SerializeField] private Image boostIcon;
        [SerializeField] private TextMeshProUGUI managerName;
        [SerializeField] private TextMeshProUGUI managerType;
        [SerializeField] private TextMeshProUGUI boostEffect;
        [SerializeField] private TextMeshProUGUI boostDescription;

        [SerializeField] private TextMeshProUGUI managerPanelTitle;
        [SerializeField] private Transform managerContainer;
        [SerializeField] private GameObject managerPanel;
        [SerializeField] private GameObject assignedManagerPanel;
        [SerializeField] private List<Manager> managerList;
        [SerializeField] private Camera cam;

        private List<ManagerCard> assignedManagerCards;

        public BaseManagerLocation CurrentManagerLocation
        {
                get; set;
        }

        public int NewManagerCost
        {
                get; set;
        }        

        private void Start()
        {
                assignedManagerCards = new List<ManagerCard>();
                NewManagerCost = intialManagerCost;
        }

        private void Update()
        {
                if (Input.GetMouseButtonDown(0))
                {
                        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo) )
                        {
                                if (hitInfo.transform.GetComponent<StoreManager>() != null)
                                {
                                        CurrentManagerLocation = hitInfo.transform.GetComponent<StoreManager>().Location;
                                        OpenPanel(true);
                                }
                        }
                }
        }

        #region Boosts

        public void RunMovementBoost(BaseWalker walker, float duration, float value)
        {
                StartCoroutine(IEMovementBoost(walker, duration, value));
        }

        public void RunLoadingBoost(BaseWalker walker, float duration, float value)
        {
                StartCoroutine(IELoadingBoost(walker, duration, value));
        }

        private IEnumerator IEMovementBoost(BaseWalker walker, float duration, float value)
        {
                float startSpeed = walker.MoveSpeed;
                walker.MoveSpeed *= value;
                yield return new WaitForSeconds(duration);
                walker.MoveSpeed = startSpeed;
        }
        private IEnumerator IELoadingBoost(BaseWalker walker, float duration, float value)
        {
                float startSpeed = walker.PizzaPerSecond;
                walker.PizzaPerSecond *= value;
                yield return new WaitForSeconds(duration);
                walker.PizzaPerSecond = startSpeed;
        }

        #endregion

        public void UnassignManager()
        {
                RestoreManagerCard(CurrentManagerLocation.Manager);
                CurrentManagerLocation.Manager = null;
                UpdateAssignedManagerInfo(CurrentManagerLocation);
        }

        public void HireManager()
        {
                if (GoldManager.Instance.CurrentGold >= (ulong)NewManagerCost)
                {
                        ManagerCard card = Instantiate(managerCardPrefab, managerContainer);

                        int randomIndex = Random.Range(0, managerList.Count);
                        Manager randomManager = managerList[randomIndex];
                        card.SetupManagerCard(randomManager);

                        managerList.RemoveAt(randomIndex);

                        GoldManager.Instance.RemoveGold((ulong)NewManagerCost);
                        NewManagerCost *= managerCostMultiplier;
                }
        }

        public void UpdateAssignedManagerInfo(BaseManagerLocation location)
        {
                if (location.Manager != null)
                {
                        managerIcon.sprite = location.Manager.managerIcon;
                        boostIcon.sprite = location.Manager.boostIcon;
                        managerName.text = location.Manager.managerName;
                        managerType.text = location.Manager.managerLevel.ToString();
                        boostEffect.text = location.Manager.boostDuration.ToString();
                        boostDescription.text = location.Manager.boostDescription.ToString();
                        assignedManagerPanel.SetActive(true);
                }
                else
                {
                        managerIcon.sprite = null;
                        boostIcon.sprite = null;
                        managerName.text = null;
                        managerType.text = null;
                        boostEffect.text = null;
                        boostDescription.text = null;
                        assignedManagerPanel.SetActive(false);
                }
        }

        public void AddAssignedManagerCard(ManagerCard card)
        {
                assignedManagerCards.Add(card);
        }

        public void OpenPanel(bool value)
        {
                managerPanel.SetActive(value);
                if (value)
                {
                        managerPanelTitle.text = CurrentManagerLocation.LocationTitle;
                        UpdateAssignedManagerInfo(CurrentManagerLocation);
                }
        }

        private void RestoreManagerCard(Manager manager)
        {
                ManagerCard managerCard = null;
                for (int i = 0; i < assignedManagerCards.Count; i++)
                {
                        if (assignedManagerCards[i].Manager.managerName == manager.managerName)
                        {
                                assignedManagerCards[i].gameObject.SetActive(true);
                                managerCard = assignedManagerCards[i];
                        }
                }
                if (managerCard != null)
                {
                        assignedManagerCards.Remove(managerCard);
                }
        }
}
