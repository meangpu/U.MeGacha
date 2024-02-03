using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaItemUI : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _itemName;
        [SerializeField] protected TMP_Text _itemCount;
        public GameObject keyData;

        void OnEnable()
        {
            ActionGacha.OnCurrentDictUpdate += OnGachaUpdate;
        }
        void OnDisable()
        {
            ActionGacha.OnCurrentDictUpdate -= OnGachaUpdate;
        }

        private void OnGachaUpdate(Dictionary<GameObject, int> dictionary)
        {
            Debug.Log($"what");
            if (dictionary.ContainsKey(keyData))
            {
                UpdateCount(dictionary[keyData]);
            }
        }

        public virtual void InitUI(GameObject obj, int count)
        {
            keyData = obj;
            _itemName.SetText(obj.name);
            _itemCount.SetText(count.ToString());
        }

        public void UpdateCount(int newCount)
        {
            _itemCount.SetText(newCount.ToString());
        }
    }
}
