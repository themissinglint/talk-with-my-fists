using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public static class GradientExtensions {
    
    public static Gradient Copy(this Gradient grad) {
        Gradient copy = new Gradient {
            alphaKeys = grad.alphaKeys,
            colorKeys = grad.colorKeys
        };
        return copy;
    }
    
}
