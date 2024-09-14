using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaUIAmountItemLeftCount : MonoBehaviour
    {
        [SerializeField] TMP_Text _countTxt;
        [SerializeField] GachaManager _gachaManagerScpt; // for update on start

        void OnEnable() => ActionGacha.OnCurrentDictUpdate += OnGachaUpdate;
        void OnDisable() => ActionGacha.OnCurrentDictUpdate -= OnGachaUpdate;

        private void Start() => OnGachaUpdate(_gachaManagerScpt.NowDictData);

        private void OnGachaUpdate(Dictionary<GameObject, int> dictionary) => UpdateCount(dictionary.Values.Sum());
        public void UpdateCount(int newCount) => _countTxt.SetText(newCount.ToString());
    }
}
