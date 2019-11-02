using Godot;

namespace RainDrop
{
    static class Window
    {
        public static float Width
        {
            get
            {
                return OS.GetRealWindowSize().x;
            }
        }

        public static float Height
        {
            get
            {
                return OS.GetRealWindowSize().y;
            }
        }
    }
}
