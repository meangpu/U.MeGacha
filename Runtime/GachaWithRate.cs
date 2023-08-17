using UnityEngine;

namespace Meangpu.Gacha
{
    [System.Serializable]
    public struct GachaWithRate
    {
        public GameObject Object;
        public int Rate;
        public GachaWithRate(GameObject obj, int rate)
        {
            Object = obj;
            Rate = rate;
        }
    }
}