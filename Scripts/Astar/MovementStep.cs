using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zain.Astar
{
    public class MovementStep
    {
        public string sceneName;

        //时间戳
        public int hour;
        public int minute;
        public int second;

        public Vector2Int gridCoordinate;   //网格坐标,不是数组坐标
    }
}

