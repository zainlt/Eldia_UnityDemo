using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Zain.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemToolTip;

        [Header("��קͼƬ")]
        public Image dragItem;

        [Header("��ұ���UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;

        [Header("ͨ�ñ���")]
        [SerializeField] private GameObject baseBag;
        public GameObject shopSlotPrefab;
        public GameObject boxSlotPrefab;
        public Button closeButton;

        [Header("����Ʒ�Ľ��׽���")]
        public Button cancelSellButton;

        [Header("����UI")]
        public TradeUI tradeUI;
        public TextMeshProUGUI playerMoneyText;

        [SerializeField] private SlotUI[] playerSlots;
        [SerializeField] private List<SlotUI> baseBagSlots;

        private InventoryBag_SO bagData;//��ʱ����
        private bool isOpened;

        public List<GameObject> poolPrefabs;
        public Transform parent;
        private Queue<GameObject> pickItemQueue = new Queue<GameObject>();

        private void Awake()
        {
            closeButton.onClick.AddListener(BaseBagClose);
            cancelSellButton.onClick.AddListener(OnCloseTradeUI);
        }

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI += OnShowTradeUI;
            EventHandler.InitPickedItemEffect += OnInitPickedItemEffect;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI -= OnShowTradeUI;
            EventHandler.InitPickedItemEffect -= OnInitPickedItemEffect;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHightLight(-1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnShowTradeUI(ItemDetails item, bool isSell ,SlotType slotType)
        {
            //���̵���߼�
            if(slotType == SlotType.Shop)
            {
                baseBag.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                tradeUI.GetComponent<RectTransform>().pivot = new Vector2(1.05f, 2.65f);
                tradeUI.gameObject.SetActive(true);
                tradeUI.SetupTradeUI(item, isSell);
            }
            else if(slotType == SlotType.Bag)   //�򿪱������߼�
            {
                isOpened = !isOpened;
                if (isOpened)
                {
                    cancelSellButton.gameObject.SetActive(true);
                    tradeUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 7.05f);
                    tradeUI.gameObject.SetActive(true);
                    tradeUI.SetupTradeUI(item, isSell);
                    itemToolTip.gameObject.SetActive(false);
                    EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                }
                else
                {
                    cancelSellButton.gameObject.SetActive(false);
                    tradeUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                    tradeUI.gameObject.SetActive(false);
                    itemToolTip.gameObject.SetActive(true);
                    EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
                }
            }
        }

        /// <summary>
        /// �رս��׽���
        /// </summary>
        public void OnCloseTradeUI()
        {
            tradeUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            baseBag.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            tradeUI.gameObject.SetActive(false);
            UpdateSlotHightLight(-1);
            EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        }

        private void Start()
        {
            for(int i = 0;i < playerSlots.Length; i++)
            {
                //��ÿ���������
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;
            playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        /// <summary>
        /// �򿪱���UI
        /// </summary>
        /// <param name="slotType"></param>
        /// <param name="bagData"></param>
        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            //TODO:ͨ������prefab
            GameObject prefab = slotType switch
            {
                SlotType.Shop => shopSlotPrefab,
                SlotType.Box => boxSlotPrefab,
                _ => null,
            };

            //���ɱ���UI
            baseBag.SetActive(true);

            baseBagSlots = new List<SlotUI>();

            for(int i = 0;i < bagData.itemList.Count; i++)
            {
                var slot = Instantiate(prefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
                slot.slotIndex = i;
                baseBagSlots.Add(slot);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());

            //����UI��ʾ
            OnUpdateInventoryUI(InventoryLocation.Box, bagData.itemList);
        }

        /// <summary>
        /// �رձ���UI
        /// </summary>
        /// <param name="slotType"></param>
        /// <param name="bagData"></param>
        private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            this.bagData = bagData;
            BaseBagClose();

        }

        /// <summary>
        /// �����̵갴ť�Ĺر�
        /// </summary>
        private void BaseBagClose()
        {
            OnCloseTradeUI();
            baseBag.SetActive(false);
            itemToolTip.gameObject.SetActive(false);

            foreach (var slot in baseBagSlots)
            {
                Destroy(slot.gameObject);
            }
            baseBagSlots.Clear();
            //EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        }


        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
                case InventoryLocation.Box:
                    for (int i = 0; i < baseBagSlots.Count; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            baseBagSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }

            playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        /// <summary>
        /// �򿪹رձ���
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;

            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// ����slot������ʾ
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHightLight(int index)
        {
            foreach(var slot in playerSlots)
            {
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
            
            for(int i = 0;i < baseBagSlots.Count; i++)
            {
                if(baseBagSlots[i].isSelected && baseBagSlots[i].slotIndex == index)
                {
                    baseBagSlots[i].slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    baseBagSlots[i].isSelected = false;
                    baseBagSlots[i].slotHightLight.gameObject.SetActive(false);
                }
            }
        }


        #region ������Ʒ�����
        private void CreatePickItemPool()
        {
            //var parent = new GameObject(poolPrefabs[0].name).transform;
            //parent.SetParent(transform);

            for (int i = 0; i < 10; i++)
            {
                GameObject newObj = Instantiate(poolPrefabs[0], parent);
                newObj.SetActive(false);
                pickItemQueue.Enqueue(newObj);
            }
        }

        private GameObject GetItemPoolObject()
        {
            if (pickItemQueue.Count < 2)
                CreatePickItemPool();
            return pickItemQueue.Dequeue();
        }

        private void OnInitPickedItemEffect(ItemDetails itemDetails)
        {
            var obj = GetItemPoolObject();
            obj.GetComponent<PickedItem>().SetItem(itemDetails);
            obj.SetActive(true);
            StartCoroutine(ShowItemPickedPanel(obj.transform,obj));
        }

        private IEnumerator ShowItemPickedPanel(Transform obj,GameObject parent)
        {
            obj.DOMove(new Vector2(obj.position.x, obj.position.y + 50), 0.5f);
            yield return new WaitForSeconds(0.5f);
            parent.SetActive(false);
        }
        #endregion


    }
}

