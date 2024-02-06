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

#if UNITY_EDITOR
            foreach (KeyValuePair<GameObject, int> item in _gachaManagerScpt.NowDictData)
            {
                GachaItemUI nowObject = (GachaItemUI)UnityEditor.PrefabUtility.InstantiatePrefab(_uiItemPrefab);
                nowObject.transform.SetParent(_parentTrans, false);
                nowObject.InitUI(item.Key, item.Value);
                nowObject.gameObject.name = item.Key.name;
            }
#endif
        }

    }
}
