namespace Utils
{
    public static class AnimatorParametersNames
    {
        public static readonly string DirectionX = "DirectionX";
        public static readonly string DirectionY = "DirectionY";
        internal static readonly string IsRunning = "IsRunning";
        internal static readonly string IsCrouching = "IsCrouching";
        internal static readonly string IsMoving = "IsMoving";
    }

    public static class AnimatorLayerIndexes
    {
        public static readonly int Base = 0;
        public static readonly int Lantern = 1;
    }

    public static class InputAxesNames
    {
        public static readonly string Horizontal = "Horizontal";
        public static readonly string Vertical = "Vertical";
        public static readonly string CameraX = "Mouse X";
        public static readonly string CameraY = "Mouse Y";
        public static readonly string Run = "Run";
        internal static readonly string Crouch = "Crouch";
        internal static readonly string Lantern = "Lantern";
        internal static readonly string Pick = "Pick";
    }

    public static class SurfaceSoundNames
    {
        public static readonly string Default = "Default";
    }

    public static class StepNames
    {
        public static readonly string LeftFoot = "Left";
        public static readonly string RightFoot = "Right";
    }
}