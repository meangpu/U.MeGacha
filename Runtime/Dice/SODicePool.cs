using UnityEngine;
using System.Collections.Generic;
using EasyButtons;

namespace Meangpu.Dice
{
    [CreateAssetMenu(fileName = "SO_DICE", menuName = "Meangpu/Gacha/SODice")]
    public class SODicePool : ScriptableObject
    {
        public List<Sprite> TemplateObject = new();
        public List<SpriteWithValue> DiceList;

        [Header("This two below is for debug only")]
        public List<SpriteWithValue> ObjectLootList;
        public List<SpriteWithValue> ObjectCanRemoveList;

        bool _isInitialized;

        [SerializeField] bool _isDeleteAfterGet;
        [SerializeField] bool _isAutoReset = true;

        [Button]
        public void CREATE_DICE_LIST_FROM_SPRITE()
        {
            DiceList = new();
            DiceList.Clear();
            for (var i = 0; i < TemplateObject.Count; i++)
            {
                DiceList.Add(new(TemplateObject[i], i + 1));
            }
            ResetInit();
        }

        [Button]
        public void ResetInit()
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
            ObjectLootList = new();
            ObjectLootList.Clear();
            ObjectLootList.AddRange(DiceList);
        }

        [Button]
        public SpriteWithValue GetRandomDice()
        {
            if (_isDeleteAfterGet) return GetRandomRemove();
            else return GetRandomNoRemove();
        }

        private SpriteWithValue GetRandomRemove()
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
                    return default;
                }
            }

            int randomIndex = Random.Range(0, ObjectCanRemoveList.Count);
            SpriteWithValue finalObject = ObjectCanRemoveList[randomIndex];
            ObjectCanRemoveList.RemoveAt(randomIndex);
            return finalObject;
        }

        private SpriteWithValue GetRandomNoRemove()
        {
            InitializeNormalPool();
            int randomIndex = Random.Range(0, ObjectLootList.Count);
            return ObjectLootList[randomIndex];
        }

        [Button]
        public void RandomManyTime(int count)
        {
            for (int i = 0; i < count; i++)
                GetRandomDice();
        }
    }
}