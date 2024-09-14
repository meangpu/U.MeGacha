using TMPro;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaUIDisplayLastRollInfo : MonoBehaviour
    {
        [Header("Percent")]
        [SerializeField] TMP_Text _textRollPercent;
        [SerializeField] string _floatingPoint = "F2";
        [SerializeField] string _endingWord = "%";
        [Header("Name")]
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
            string percentNumber = (percent * 100).ToString(_floatingPoint);
            _textRollPercent?.SetText($"{percentNumber}{_endingWord}");
            _itemName?.SetText(objectName);
        }


    }
}
