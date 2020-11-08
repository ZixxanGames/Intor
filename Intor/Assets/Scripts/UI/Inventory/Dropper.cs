using Scripts.Extensions;
using UnityEngine;

namespace Scripts.UI.Inventory
{
    public class Dropper : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemObjectsContainer = null;


        private void Awake()
        {
            BackpackUI.ItemDropped += OnItemDropped;
        }

        private void OnDestroy()
        {
            BackpackUI.ItemDropped -= OnItemDropped;
        }


        private void OnItemDropped(Item item, Transform robotTransform)
        {
            if (item.GameObject.activeSelf)
            {
                if (ItemObject.GetObject(item) is GameObject obj) item.GameObject = obj;
                else item.GameObject = Instantiate(item.GameObject, _itemObjectsContainer);
            }

            item.GameObject.transform.position = robotTransform.position + (2 * (robotTransform.forward + robotTransform.transform.right)).RandomizeInSphere(2.5f);
            item.GameObject.GetComponent<ItemObject>().SetItem(item);
            item.GameObject.SetActive(true);
        }
    }
}