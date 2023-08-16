using System.Collections.Generic;
using UnityEngine;

namespace Meangpu.Gacha
{
    public class GachaManager : MonoBehaviour
    {
        [Expandable]
        [SerializeField] SOLootTable _startTable;

        private void Start()
        {
            // _nowGachaPool = new(_startTable.DropRate);
        }

        public void GetFromGachaPool()
        {
            // if (_isDeleteOnGet)
            // {
            //     // _nowGachaPool
            // }
        }
    }
}
