using UnityEngine;

namespace Utils
{
    #region Classes
    public static class ActionsUtils
    {
        public static void Noop()
        {
        }
        public static void Noop1<P>(P _)
        {
        }
    }
    
    public static class SurfaceSoundNames
    {
        public static readonly string Default = "Default";
    }

    public static class StepNames
    {
        public static readonly string LeftFootWalk = "LeftWalk";
        public static readonly string RightFootWalk = "RightWalk";
        public static readonly string LeftFootCrouch = "LeftCrouch";
        public static readonly string RightFootCrouch = "RightCrouch";
        public static readonly string LeftFootRun = "LeftRun";
        public static readonly string RightFootRun = "RightRun";
        public static bool IsLeft(string foot)
        {
            return foot.StartsWith("Left");
        }
        public static bool IsRight(string foot)
        {
            return foot.StartsWith("Right");
        }
    }

    public static class Extensions
    {
        public static bool Includes(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
    #endregion

    #region Interfaces
    public interface IUpdateState<T>
    {
        void OnIn(T reference);
        void OnOut(T reference);
        void Update(T reference);

        void FixedUpdate(T reference);
    }
    #endregion

    #region Enums
    public enum AnimatorLayerIndexes
    {
        Base,
        Lantern
    }
    public enum AnimatorParametersNames
    {
        DirectionX,
        DirectionY,
        IsRunning,
        IsCrouching,
        IsMoving,
        Action
    }
    public enum PlayerActionsNames
    {
        Die,
        Confine,
        Exorcise
    }

    public enum EnemyActionsNames
    {
        Die,
        Attack
    }
    public enum InputAxesNames
    {
        Horizontal,
        Vertical,
        CameraX,
        CameraY,
        Run,
        Crouch,
        Lantern,
        PrimaryAction,
        Confine
    }
    #endregion
}