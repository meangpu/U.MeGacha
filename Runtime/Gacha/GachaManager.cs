using VInspector;
using UnityEngine;
using Meangpu.Pool;
using System.Collections.Generic;
using Meangpu.Util;
using System.Linq;
using UnityEngine.Events;

namespace Meangpu.Gacha
{
    // if use feel, extend from this base class and use GetObjectFromPreview as start function
    public class GachaManager : MonoBehaviour
    {
        [Expandable]
        [SerializeField] protected SOLootTable _startTable;
        [SerializeField] protected Transform _parentPreview;
        [SerializeField] protected Transform _parentSpawnGacha;

        [SerializeField] protected List<GameObject> _previewObjectList = new();
        [SerializeField] protected bool _refillOnPreviewEmpty;
        [SerializeField] protected Vector3 _spawnOffset = new();

        [SerializeField] protected SerializedDictionary<GameObject, int> _dictionaryGachaCount = new();
        public SerializedDictionary<GameObject, int> NowDictData => _dictionaryGachaCount;

        [SerializeField] protected bool _ParentDeleteChildObjectOnSpawn;
        [SerializeField] protected UnityEvent _OnDoGachaEvent;
        [SerializeField] bool _updateDictOnStart = true;

        private void Start()
        {
            if (_updateDictOnStart) ActionGacha.OnCurrentDictUpdate(_dictionaryGachaCount);
        }

        public void GetRandomFromLootTable() => _startTable.GetRandomObject();

        public void CheckIfListError()
        {
            if (_previewObjectList.Count != _dictionaryGachaCount.Values.Sum())
            {
                Debug.Log($"<color=#4ec9b0>Dict Count Is not same as preview!!!</color>");
            }
        }

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
                    previewObj.transform.localPosition += (_spawnOffset * (i + 1)) - (_dictionaryGachaCount.Values.Sum() * .5f * _spawnOffset);
                    _previewObjectList.Add(previewObj);
                    i += 1;
                }
            }
        }


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
            AddObjValueInDict(finalObject);
            CheckIfListError();
            _OnDoGachaEvent?.Invoke();
            ActionGacha.OnRollGacha?.Invoke();
            return finalObject;
        }

        [Button]
        public void SetToSpawnParentFromPreview()
        {
            if (_ParentDeleteChildObjectOnSpawn) KillAllChild.KillAllChildInTransform(_parentSpawnGacha);
            GameObject nowObj = GetObjectFromPreview();
            if (nowObj == null) return;
            nowObj.transform.SetParent(_parentSpawnGacha);
            nowObj.transform.localPosition = Vector3.zero;
            nowObj.transform.localRotation = Quaternion.identity;
            nowObj.transform.localScale = Vector3.one;
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
            ActionGacha.OnCurrentDictUpdate?.Invoke(_dictionaryGachaCount);
        }

        public void AddObjValueInDict(GameObject targetObj, int count = -1)
        {
            foreach (GameObject key in _dictionaryGachaCount.Keys)
            {
                if (key.name == targetObj.name)
                {
                    _dictionaryGachaCount[key] += count;
                    ActionGacha.OnCurrentDictUpdate?.Invoke(_dictionaryGachaCount);
                    return;
                }
            }
        }

        public void SetObjValueInDict(GameObject targetObj, int newValue)
        {
            foreach (GameObject key in _dictionaryGachaCount.Keys)
            {
                if (key.name == targetObj.name)
                {
                    _dictionaryGachaCount[key] = newValue;
                    ActionGacha.OnCurrentDictUpdate?.Invoke(_dictionaryGachaCount);
                    return;
                }
            }
        }


        public void UpdateDictAndPreviewData(GameObject targetObj, int newValue)
        {
            SetObjValueInDict(targetObj, newValue);
            ReSpawnObjectFromDict();
        }
    }
}
