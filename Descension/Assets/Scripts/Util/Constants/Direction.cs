using System.Collections.Generic;
using UnityEngine;

namespace Util.Constants
{
    public static class Direction
    {
        public static readonly List<Vector2> Directions = new List<Vector2>()
        {
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(0,-1),
        };

        public static Vector2 North => new Vector2(0, 1);
        public static Vector2 East => new Vector2(1, 0);
        public static Vector2 South => new Vector2(0, -1);
        public static Vector2 West => new Vector2(-1, 0);

        // wrappers for directions 
        public static Vector2 Up => North;
        public static Vector2 Right => East;
        public static Vector2 Down => South;
        public static Vector2 Left => West;
    }
}