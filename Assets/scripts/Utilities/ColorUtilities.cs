using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>Color utilities are a set of utilities for working with colors.</summary>
public static class ColorUtilities {

    /// <summary>
    /// Blends the colors.
    /// </summary>
    /// <returns>Black for an empty list, otherwise a blended color.</returns>
    /// <param name="colors">Colors.</param>
    public static Color BlendColors(List<ColorWeight> colors, float forceAlpha = -1f) {
        if (colors.Count == 0)
            return Color.black;
        float rWeight, gWeight, bWeight, aWeight, r, g, b, a;
        rWeight = gWeight = bWeight = aWeight = r = g = b = a = 0f;
        for (int i = 0; i < colors.Count; i++) {
            r += colors[i].color.r * colors[i].weight * colors[i].ChannelWeights.r;
            rWeight += colors[i].weight * colors[i].ChannelWeights.r;
            g += colors[i].color.g * colors[i].weight * colors[i].ChannelWeights.g;
            gWeight += colors[i].weight * colors[i].ChannelWeights.g;
            b += colors[i].color.b * colors[i].weight * colors[i].ChannelWeights.b;
            bWeight += colors[i].weight * colors[i].ChannelWeights.b;
            a += colors[i].color.a * colors[i].weight * colors[i].ChannelWeights.a;
            aWeight += colors[i].weight * colors[i].ChannelWeights.a;
        }
        rWeight = Mathf.Clamp(rWeight, 0.001f, Single.PositiveInfinity);
        gWeight = Mathf.Clamp(gWeight, 0.001f, Single.PositiveInfinity);
        bWeight = Mathf.Clamp(bWeight, 0.001f, Single.PositiveInfinity);
        aWeight = Mathf.Clamp(aWeight, 0.001f, Single.PositiveInfinity);
        return new Color(r / rWeight, g / gWeight, b / bWeight, Math.Abs(forceAlpha - (-1f)) > 0.01f ? forceAlpha : a / aWeight);
    }
    
    public static Color Grayscalify (Color color) {
        float dot = Vector3.Dot((Vector4)color, new Vector3(0.3f, 0.59f, 0.11f));
        return new Color(dot, dot, dot, color.a);
    }

    public static Color AddDab (this Color inputColor, Color dabColor, float dabStrength) {
        ColorWeight inputCW = new ColorWeight(inputColor, 1f);
        ColorWeight dabCW = new ColorWeight(dabColor, dabStrength);
        return (BlendColors(new List<ColorWeight> {inputCW, dabCW}));
    }
    
    public static Color ReplacePortion (this Color inputColor, Color replacingColor, float replacedPortion) {
        replacedPortion = Mathf.Clamp01(replacedPortion);
        ColorWeight inputCW = new ColorWeight(inputColor, 1f - replacedPortion);
        ColorWeight replaceCW = new ColorWeight(replacingColor, replacedPortion);
        return (BlendColors(new List<ColorWeight> {inputCW, replaceCW}));
    }

}

public class ColorWeight {
    public readonly Color color;
    public readonly float weight;
    public Color ChannelWeights = Color.white;
    public ColorWeight(Color color, float weight) {
        this.color = color; this.weight = weight;
    }
}

