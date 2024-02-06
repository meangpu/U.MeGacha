using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Meangpu.Gacha
{
    public class GachaItemUI : MonoBehaviour
    {
        [Header("Main data to check")]
        public GameObject keyData;
        [Header("display setting")]
        [SerializeField] protected TMP_Text _itemName;
        [SerializeField] protected TMP_Text _itemCount;
        [SerializeField] protected TMP_Text _itemGetPercent;
        [SerializeField] string _percentEndText = "%";
        [SerializeField] Slider _sliderPercent;

        float _percentToGetAsZeroPointFloat;

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

            _percentToGetAsZeroPointFloat = dictionary[keyData] / (float)dictionary.Values.Sum();
            if (float.IsNaN(_percentToGetAsZeroPointFloat)) _percentToGetAsZeroPointFloat = 0;

            if (_itemGetPercent != null)
            {
                UpdatePercent(_percentToGetAsZeroPointFloat);
            }
            if (_sliderPercent != null)
            {
                UpdatePercentSlider(_percentToGetAsZeroPointFloat);
            };
        }

        public virtual void InitUI(GameObject obj, int count)
        {
            keyData = obj;
            _itemName.SetText(obj.name);
            UpdateCount(count);
            if (_itemGetPercent != null)
            {
                UpdatePercent(0);
            }
            if (_sliderPercent != null)
            {
                InitSlider();
            };
        }

        void InitSlider()
        {
            _sliderPercent.minValue = 0;
            _sliderPercent.maxValue = 100;
            _sliderPercent.value = 0;
        }

        public void UpdateCount(int newCount) => _itemCount.SetText(newCount.ToString());
        public void UpdatePercent(float percent) => _itemGetPercent.SetText($"{percent * 100:F2}{_percentEndText}");
        public void UpdatePercentSlider(float percent) => _sliderPercent.value = percent * 100;
    }
}
