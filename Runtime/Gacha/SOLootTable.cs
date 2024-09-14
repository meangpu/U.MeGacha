using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VInspector;

namespace Meangpu.Gacha
{
    [CreateAssetMenu(fileName = "SOLootTable", menuName = "Meangpu/Gacha/SOLootTable")]
    public class SOLootTable : ScriptableObject
    {
        public List<GameObject> TemplateObject = new();
        public SerializedDictionary<GameObject, int> DropRate = new();

        [Header("This two below is for debug only")]
        public List<GameObject> ObjectLootList = new();
        public List<GameObject> ObjectCanRemoveList = new();

        bool _isInitialized;

        [SerializeField] bool _isDeleteAfterGet;
        [SerializeField] bool _isAutoReset = true;


        [Button]
        public void INIT_OBJ_POOL()
        {
            _isInitialized = false;
            InitializeNormalPool();
        }

        void InitializeNormalPool()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                SetupLootList();
                ObjectCanRemoveList = new(ObjectLootList);
            }
        }

        private void SetupLootList()
        {
            ObjectLootList.Clear();
            foreach (KeyValuePair<GameObject, int> item in DropRate)
            {
                for (int i = 0; i < item.Value; i++) ObjectLootList.Add(item.Key);
            }
        }

        [Button]
        public GameObject GetRandomObject()
        {
            if (_isDeleteAfterGet) return GetRandomRemove();
            else return GetRandomNoRemove();
        }

        private GameObject GetRandomRemove()
        {
            InitializeNormalPool();
            if (ObjectCanRemoveList.Count == 0)
            {
                if (_isAutoReset)
                {
                    ObjectCanRemoveList = new(ObjectLootList);
                }
                else
                {
                    Debug.Log("<color=red>Object is not set to auto reset</color>");
                    return null;
                }
            }

            int randomIndex = Random.Range(0, ObjectCanRemoveList.Count);
            GameObject finalObject = ObjectCanRemoveList[randomIndex];

            int thisCount = ObjectCanRemoveList.Count(x => x.name == finalObject.name);
            float percentToGetThis = thisCount / (float)ObjectCanRemoveList.Count();
            ActionGacha.OnLastRollInfo?.Invoke(percentToGetThis, finalObject.name);


            ObjectCanRemoveList.RemoveAt(randomIndex);
            return finalObject;
        }

        private GameObject GetRandomNoRemove()
        {
            InitializeNormalPool();
            int randomIndex = Random.Range(0, ObjectLootList.Count);

            GameObject finalObject = ObjectLootList[randomIndex];

            int thisCount = ObjectCanRemoveList.Count(x => x.name == finalObject.name);
            float percentToGetThis = thisCount / (float)ObjectCanRemoveList.Count();
            ActionGacha.OnLastRollInfo?.Invoke(percentToGetThis, finalObject.name);

            return finalObject;
        }

        [Button]
        public void RandomManyTime(int count)
        {
            for (int i = 0; i < count; i++)
                GetRandomObject();
        }

        [Button]
        public void INIT_DROP_RATE_BY_OBJECT()
        {
            DropRate.Clear();
            foreach (GameObject obj in TemplateObject)
            {
                DropRate.Add(obj, 1);
            }
        }
    }
}