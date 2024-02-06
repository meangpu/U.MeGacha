using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaItemUI : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _itemName;
        [SerializeField] protected TMP_Text _itemCount;
        [SerializeField] protected TMP_Text _itemGetPercent;
        [SerializeField] string _percentEndText = "%";
        float _percentToGet;

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
            if (dictionary.ContainsKey(keyData))
            {
                UpdateCount(dictionary[keyData]);
            }

            if (_itemGetPercent == null) return;

            _percentToGet = dictionary[keyData] / (float)dictionary.Values.Sum();

            if (float.IsNaN(_percentToGet)) _percentToGet = 0;
            UpdatePercent(_percentToGet);
        }

        public virtual void InitUI(GameObject obj, int count)
        {
            keyData = obj;
            _itemName.SetText(obj.name);
            UpdateCount(count);
            UpdatePercent(0);
        }

        public void UpdateCount(int newCount) => _itemCount.SetText(newCount.ToString());
        public void UpdatePercent(float percent) => _itemGetPercent.SetText($"{percent * 100:F2}{_percentEndText}");
    }
}
