using UnityEngine;
using TMPro;

namespace Meangpu.Gacha
{
    public class GachaUIOnRollDisplayLastRollPercent : MonoBehaviour
    {
        [SerializeField] TMP_Text _rollPercent;
        [SerializeField] string _percentEndText = "%";
        [SerializeField] string _floatingPoint = "F2";

        void OnEnable() => ActionGacha.OnGetRandomItemThePercentIs += DisplayLastRollPercent;
        void OnDisable() => ActionGacha.OnGetRandomItemThePercentIs -= DisplayLastRollPercent;

        private void DisplayLastRollPercent(float percent)
        {
            string percentNumber = (percent * 100).ToString(_floatingPoint);
            _rollPercent.SetText($"{percentNumber}{_percentEndText}");
        }
    }
}
