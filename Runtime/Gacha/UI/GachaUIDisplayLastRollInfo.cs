using TMPro;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaUIDisplayLastRollInfo : MonoBehaviour
    {
        [SerializeField] TMP_Text _textRollPercent;
        [SerializeField] string _floatingPoint = "P2";
        [SerializeField] TMP_Text _itemName;

        void OnEnable()
        {
            ActionGacha.OnLastRollInfo += DisplayLastRollInfo;
        }

        void OnDisable()
        {
            ActionGacha.OnLastRollInfo -= DisplayLastRollInfo;
        }

        private void DisplayLastRollInfo(float percent, string objectName)
        {
            string percentNumber = percent.ToString(_floatingPoint);
            _textRollPercent?.SetText($"{percentNumber}");
            _itemName?.SetText(objectName);
        }


    }
}
