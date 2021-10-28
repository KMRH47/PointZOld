using System;

namespace PointZ.Models.CustomEditor
{
    [Flags]
    public enum TextInputTypes
    {
        ClassText = 1,
        TextFlagMultiLine = 131072,
        TextVariationVisiblePassword = 144
    }
}