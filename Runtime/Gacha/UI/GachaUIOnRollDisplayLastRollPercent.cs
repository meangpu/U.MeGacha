using UnityEngine;
using TMPro;

namespace Meangpu.Gacha
{
    public class GachaUIOnRollDisplayLastRollPercent : MonoBehaviour
    {
        [SerializeField] TMP_Text _rollPercent;
        [SerializeField] string _percentEndText = "%";

        void OnEnable() => ActionGacha.OnGetRandomItemThePercentIs += DisplayLastRollPercent;
        void OnDisable() => ActionGacha.OnGetRandomItemThePercentIs -= DisplayLastRollPercent;

        private void DisplayLastRollPercent(float percent)
        {
            Debug.Log(percent);
            _rollPercent.SetText($"{percent * 100:F2}{_percentEndText}");
        }
    }
}
