using System;

namespace Util.Enums
{
    [Flags]
    public enum UnityLayer : int
    {
        Default       = 1 << 0,
        TransparentFX = 1 << 1,
        IgnoreRaycast = 1 << 2,
        Player        = 1 << 3,
        Water         = 1 << 4,
        UI            = 1 << 5,
        UI2           = 1 << 6,
        Enemy         = 1 << 7,
        PostProcessing= 1 << 8,
        Boulder       = 1 << 9,
        Ground        = 1 << 10,
        Walls         = 1 << 11,
    }
}