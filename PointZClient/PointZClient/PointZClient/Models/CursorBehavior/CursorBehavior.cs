namespace PointZClient.Models.CursorBehavior
{
    public struct CursorBehavior
    {
        public CursorBehavior(float posX, float posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public float PosX { get; }
        public float PosY { get; }
    }
}