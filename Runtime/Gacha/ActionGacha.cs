using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meangpu.Gacha
{
    public static class ActionGacha
    {
        public static Action OnRollGacha;
        public static Action<Dictionary<GameObject, int>> OnCurrentDictUpdate;
    }
}
