using UnityEngine;

namespace Meangpu.Dice
{
    [System.Serializable]
    public struct SpriteWithValue
    {
        public Sprite Sprite;
        public int Value;
        public SpriteWithValue(Sprite image, int value)
        {
            Sprite = image;
            Value = value;
        }
    }
}