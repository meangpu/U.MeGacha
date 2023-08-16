using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;

namespace Meangpu.Gacha
{
    [CreateAssetMenu(fileName = "SOLootTable", menuName = "Meangpu/Gacha/SOLootTable")]
    public class SOLootTable : ScriptableObject
    {
        public List<GachaWithRate> DropRate;
        public List<GachaWithRate> DropRateCanDelete;

        float _totalWeight;
        bool _isInitializedNormal;
        bool _isInitializedPoolCanDelete;

        public bool IsDeleteOnGet;
        public bool ResetPoolOnReachZero;
        public int DeleteNumber = 1;

        void InitializeNormalPool()
        {
            if (!_isInitializedNormal)
            {
                _totalWeight = DropRate.Sum(Object => Object.Rate);
                _isInitializedNormal = true;
            }
        }

        void InitializePoolCanDelete()
        {
            if (!_isInitializedPoolCanDelete)
            {
                DropRateCanDelete.Clear();
                DropRateCanDelete = new(DropRate);
                _isInitializedPoolCanDelete = true;
            }
        }

        [Button]
        public GameObject GetRandomObject()
        {
            if (IsDeleteOnGet)
                return GetRandomRollAndDelete();
            else
                return GetRandomRoll();
        }

        private GameObject GetRandomRollAndDelete()
        {
            InitializePoolCanDelete();
            _totalWeight = DropRateCanDelete.Sum(Object => Object.Rate);
            if (_totalWeight <= 0)
            {
                _isInitializedPoolCanDelete = false;
                InitializePoolCanDelete();
            }

            float diceRoll = Random.Range(0, _totalWeight);
            foreach (GachaWithRate item in DropRateCanDelete)
            {
                if (item.Rate >= diceRoll)
                {
                    item.Rate -= DeleteNumber;
#if UNITY_EDITOR
                    Debug.Log($"{item.Object}");
#endif
                    return item.Object;
                }
                diceRoll -= item.Rate;
            }
            throw new System.Exception("Fail generate reward");
        }

        private GameObject GetRandomRoll()
        {
            InitializeNormalPool();
            float diceRoll = Random.Range(0, _totalWeight);

            foreach (GachaWithRate item in DropRate)
            {
                if (item.Rate >= diceRoll)
                {
#if UNITY_EDITOR
                    Debug.Log($"{item.Object}");
#endif
                    return item.Object;
                }

                diceRoll -= item.Rate;
            }
            throw new System.Exception("Fail generate reward");
        }

        [Button]
        public void RandomManyTime(int count)
        {
            for (int i = 0; i < count; i++)
                GetRandomObject();
        }
    }
}