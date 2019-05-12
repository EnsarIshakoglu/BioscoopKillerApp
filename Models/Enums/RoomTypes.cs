using System.ComponentModel;

namespace Models.Enums
{
    public enum RoomTypes
    {
        [Description("3D")]
        Normal3D,
        [Description("2D")]
        Normal2D,
        [Description("4DX")]
        Imax4D,
        [Description("IMAX3D")]
        Imax3D,
        [Description("IMAX2D")]
        Imax2D
    }
}