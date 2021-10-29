namespace Client
{
    public struct ChangeBulletEvent
    {
        public BulletType Type;
    }
    public enum BulletType
    {
        Red,
        Yellow,
        Violet,
        Silver
    }
}