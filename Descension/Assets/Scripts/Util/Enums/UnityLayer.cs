namespace Assets.Scripts.Util.Enums
{
    public enum UnityLayer : int
    {
        Default       = 1 << 0,
        TransparentFX = 1 << 1,
        IgnoreRaycast = 1 << 2,
        Player        = 1 << 3,
        Water         = 1 << 4,
        UI            = 1 << 5,
        Enemy         = 1 << 6,
    }
}