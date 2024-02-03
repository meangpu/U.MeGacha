using VInspector;
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

        public SerializedDictionary<GameObject, int> _dictionaryGachaCount = new();

        [SerializeField] protected bool _ParentDeleteObjectOnSpawn;

        public void GetRandomFromLootTable() => _startTable.GetRandomObject();

        [Button]
        public void CreatePreviewGameObject()
        {
            InitDictionary();
            _startTable.INIT_OBJ_POOL();

            KillAllChild.KillAllChildInTransform(_parentPreview);
            _previewObjectList.Clear();

            for (int i = 0; i < _startTable.ObjectLootList.Count; i++)
            {
                GameObject previewObj = PoolManager.SpawnObject(_startTable.ObjectLootList[i], _parentPreview);
                previewObj.name = _startTable.ObjectLootList[i].name;
                previewObj.transform.localPosition += (_spawnOffset * (i + 1)) - (_startTable.ObjectLootList.Count * .5f * _spawnOffset);
                _previewObjectList.Add(previewObj);
            }
        }

        [Button]
        public void ReSpawnObjectFromDict()
        {
            KillAllChild.KillAllChildInTransform(_parentPreview);
            _previewObjectList.Clear();

            int i = 0;
            foreach (GameObject key in _dictionaryGachaCount.Keys)
            {
                for (int j = 0, length = _dictionaryGachaCount[key]; j < length; j++)
                {
                    GameObject previewObj = PoolManager.SpawnObject(key, _parentPreview);
                    previewObj.name = key.name;
                    previewObj.transform.localPosition += (_spawnOffset * (i + 1)) - (_startTable.ObjectLootList.Count * .5f * _spawnOffset);
                    _previewObjectList.Add(previewObj);
                    i += 1;
                }
            }
        }


        public void InitDictionary()
        {
            _dictionaryGachaCount = new();
            foreach (KeyValuePair<GameObject, int> item in _startTable.DropRate)
            {
                if (_dictionaryGachaCount.ContainsKey(item.Key))
                {
                    _dictionaryGachaCount[item.Key] = item.Value;
                }
                else
                {
                    _dictionaryGachaCount.Add(item.Key, item.Value);
                }
            }
        }

        public void RemoveObjectFromDict(GameObject targetObj, int count = -1)
        {
            foreach (GameObject key in _dictionaryGachaCount.Keys)
            {
                if (key.name == targetObj.name)
                {
                    _dictionaryGachaCount[key] += count;
                    return;
                }
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
            RemoveObjectFromDict(finalObject);

            return finalObject;
        }

        [Button]
        public void SetToSpawnParentFromPreview()
        {
            if (_ParentDeleteObjectOnSpawn) KillAllChild.KillAllChildInTransform(_parentSpawnGacha);
            GameObject nowObj = GetObjectFromPreview();
            if (nowObj == null) return;
            nowObj.transform.SetParent(_parentSpawnGacha);
            nowObj.transform.localPosition = Vector3.zero;
            nowObj.transform.localRotation = Quaternion.identity;
            nowObj.transform.localScale = Vector3.one;
        }

    }
}
