using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zain.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        public Item bounceItemPrefab;
        private Transform itemParent;

        private Transform PlayerTransform => FindObjectOfType<Player>().transform;

        //存储场景中物品
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInSence;
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent += GetAllSceneItems;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInSence;
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent -= GetAllSceneItems;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        private void OnAfterSceneLoadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItem();
        }

        private void OnInstantiateItemInSence(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }

        private void OnDropItemEvent(int ID,Vector3 mousePosition)
        {
            var item = Instantiate(bounceItemPrefab, PlayerTransform.position, Quaternion.identity, itemParent);
            item.itemID = ID;

            var dir = (mousePosition - PlayerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePosition, dir);
        }

        /// <summary>
        /// 获取当前场景所有物品信息并存储
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else //
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }

        }

        /// <summary>
        /// 刷新创建场景物品
        /// </summary>
        private void RecreateAllItem()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    //
                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}

