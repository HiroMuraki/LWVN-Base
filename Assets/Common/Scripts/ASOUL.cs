using UnityEngine;
using System.Collections.Generic;

namespace LWVNFramework {
    public enum ASOUL {
        Ava = 0b_0000_0001,
        Bella = 0b_0000_0010,
        Carol = 0b_0000_0100,
        Diana = 0b_0000_1000,
        Eileen = 0b_0001_0000
    }

    public static class ASColor {
        public static readonly Color Team = new Color(0.604f, 0.784f, 0.886f, 1);
        public static readonly Color Ava = new Color(0.604f, 0.784f, 0.886f, 1);
        public static readonly Color Bella = new Color(0.858f, 0.490f, 0.455f, 1);
        public static readonly Color Carol = new Color(0.722f, 0.651f, 0.851f, 1);
        public static readonly Color Diana = new Color(0.906f, 0.600f, 0.690f, 1);
        public static readonly Color Eileen = new Color(0.341f, 0.400f, 0.565f, 1);
    }
}
