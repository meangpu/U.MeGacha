using Meangpu.Util;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Meangpu.Gacha
{
    public class GachaUIManager : MonoBehaviour
    {
        [SerializeField] GachaItemUI _uiItemPrefab;
        [SerializeField] Transform _parentTrans;
        [SerializeField] GachaManager _gachaManagerScpt;

        [Button]
        public void SetupUI()
        {
            KillAllChild.KillAllChildInTransform(_parentTrans);
            foreach (KeyValuePair<GameObject, int> item in _gachaManagerScpt.NowDictData)
            {
                GachaItemUI nowUI = Instantiate(_uiItemPrefab, _parentTrans);
                nowUI.InitUI(item.Key, item.Value);
            }
        }

    }
}
