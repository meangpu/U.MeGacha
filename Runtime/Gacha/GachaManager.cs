using EasyButtons;
using UnityEngine;
using Meangpu.Pool;
using System.Collections.Generic;
using Meangpu.Util;

namespace Meangpu.Gacha
{
    public class GachaManager : MonoBehaviour
    {
        [Expandable]
        [SerializeField] protected SOLootTable _startTable;
        [SerializeField] protected Transform _parentPreview;
        [SerializeField] protected Transform _parentSpawnGacha;

        [SerializeField] protected List<GameObject> _previewObjectList = new();
        [SerializeField] protected bool _refillOnPreviewEmpty;
        [SerializeField] protected Vector3 _spawnOffset = new();

        [SerializeField] protected bool _DeleteObjectOnSpawn;

        public void GetRandomFromLootTable() => _startTable.GetRandomObject();

        [Button]
        public void CreatePreviewGameObject()
        {
            _startTable.ResetInit();
            KillAllChild.KillAllChildInTransform(_parentPreview);
            _previewObjectList.Clear();
            for (int i = 0; i < _startTable.ObjectLootList.Count; i++)
            {
                GameObject previewObj = PoolManager.SpawnObject(_startTable.ObjectLootList[i], _parentPreview);
                previewObj.transform.localPosition += (_spawnOffset * (i + 1)) - (_startTable.ObjectLootList.Count * .5f * _spawnOffset);
                _previewObjectList.Add(previewObj);
            }
        }

        [Button]
        public GameObject GetObjectFromPreview()
        {
            if (_previewObjectList.Count == 0)
            {
                if (_refillOnPreviewEmpty)
                {
                    CreatePreviewGameObject();
                }
                else
                {
                    Debug.Log("<color=red>Object is not set to auto reset</color>");
                    return null;
                }
            }
            int randomIndex = Random.Range(0, _previewObjectList.Count);
            GameObject finalObject = _previewObjectList[randomIndex];
            _previewObjectList.RemoveAt(randomIndex);
            return finalObject;
        }

        [Button]
        public void DefaultSpawnFromPreview()
        {
            if (_DeleteObjectOnSpawn) KillAllChild.KillAllChildInTransform(_parentSpawnGacha);
            GameObject nowObj = GetObjectFromPreview();
            if (nowObj == null) return;
            nowObj.transform.SetParent(_parentSpawnGacha);
            nowObj.transform.localPosition = Vector3.zero;
            nowObj.transform.localRotation = Quaternion.identity;
            nowObj.transform.localScale = Vector3.one;
        }
    }
}
