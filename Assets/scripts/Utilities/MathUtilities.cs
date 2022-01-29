using UnityEngine;
using System.Collections.Generic;

/// <summary>Math utilities are a set of utilities for working with numbers. A number is an arithmetical value representing a particular quantity that is used in counting and making calculations. If you didn't know that, though, you probably shouldn't be using this class.</summary>
public static class MathUtilities
{
	
	/**
	 * Returns true for numbers 0, 1, 2, 4, 8, 16, 32, 64...
	 * 
	 * @param i A number to test. Must be positive for this method to work.
	 * @return True if it is a power of 2. False otherwise.
	 */
	public static bool IsPowerOfTwo(int i)
	{
		return (i & (i - 1)) == 0;
	}

	/// <summary>
	/// Returns the number with the lowest absolute value.
	/// </summary>
	public static float LowestAbs(float t1, float t2 = Mathf.Infinity, float t3 = Mathf.Infinity, float t4 = Mathf.Infinity, float t5 = Mathf.Infinity) {
		float v = Mathf.Min(Mathf.Abs(t1), Mathf.Abs(t2), Mathf.Abs(t3), Mathf.Abs(t4), Mathf.Abs(t5));
		if (v == t1 || v == -t1) return t1;
		if (v == t2 || v == -t2) return t2;
		if (v == t3 || v == -t3) return t3;
		if (v == t4 || v == -t4) return t4;
		return t5;
	}
	
	/// <summary>
	/// Returns the number with the lowest absolute value.
	/// </summary>
	public static float HighestAbs(float t1, float t2 = 0f, float t3 = 0f, float t4 = 0f, float t5 = 0f) {
		float v = Mathf.Max(Mathf.Abs(t1), Mathf.Abs(t2), Mathf.Abs(t3), Mathf.Abs(t4), Mathf.Abs(t5));
		if (v == t1 || v == -t1) return t1;
		if (v == t2 || v == -t2) return t2;
		if (v == t3 || v == -t3) return t3;
		if (v == t4 || v == -t4) return t4;
		return t5;
	}
	
	/// <summary>
	/// The average of a list of numbers, weighted using exponentiation toward one side. Typically used to bias
	/// a rolling average toward most recent results.
	/// </summary>
	/// <param name="exponent">A multiplier by which each successive value is more heavily weighted than the last.</param>
	/// <param name="numbers">The set of numbers being evaluated.</param>
	/// <returns>An exponentially-weighted average.</returns>
	public static float ExponentiallyWeightedAverage(float exponent, List<float> numbers) {
		if (numbers.Count == 0) return 0f;
		float valueSoFar = 0f;
		float weightSoFar = 0f;
		for (int i = 0; i < numbers.Count; i++) {
			valueSoFar *= exponent;
			weightSoFar *= exponent;
			valueSoFar += numbers[i];
			weightSoFar++;
		}
		return valueSoFar / weightSoFar;
	}

    /// <summary>
    /// Decay the specified value over a given time at a given speed.
    /// Note that while time will most commonly be seconds, it doesn't
    /// matter as long as t and s use the same unit for time.
    /// </summary>
    /// <returns>A value somewhat closer to 0.</returns>
    /// <param name="v">The value being decayed.</param>
    /// <param name="t">The time during which the decay takes place.</param>
    /// <param name="s">The rate of decay per unit time.</param>
    public static float Decay(float v, float t, float s) {
        return v / Mathf.Exp(t * s);
    }
	
	/// <summary>
	/// Decay the specified value toward a target value.
	/// </summary>
	/// <returns>A calue somewhat closer to the targetValue.</returns>
	/// <param name="value">The value being decayed.</param>
	/// <param name="targetValue">The target aproached by the value.</param>
	/// <param name="t">The time during which the decay takes place.</param>
	/// <param name="s">The rate of decay per unit time.</param>
	public static float DecayToward(float value, float targetValue, float t, float s) {
		return targetValue - Decay(targetValue - value, t, s);
	}

    /// <summary>
    /// Returns clockwise if Vector "v" is clockwise from reference vector "refV"
    /// </summary>
    /// <returns><c>true</c>, if clockwise <c>false</c> otherwise.</returns>
    /// <param name="v">The assessed vector.</param>
    /// <param name="refV">The reference vector.</param>
    public static bool IsClockwise(Vector2 v, Vector2 refV) {
        if (v.y * refV.x > v.x * refV.y)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Rotates the vector2.
    /// </summary>
    /// <returns>The the vector after rotation.</returns>
    /// <param name="v">The rotated vector.</param>
    /// <param name="degreeRotation">Degree rotation.</param>
    public static Vector2 RotateVector2(Vector2 v, float degreeRotation) {
        float radians = degreeRotation * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = v.x;
        float ty = v.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static float ArcBetweenAngles(float angleA, float angleB) {
	    Vector2 a = RotateVector2(Vector2.right, angleA);
	    Vector2 b = RotateVector2(Vector2.right, angleB);
	    return Vector2.Angle(a, b);
    }
    
    public static float RotationToLookAtVector (Vector2 currentVector, Vector2 targetVector) {
	    float s = 1F;
	    if (IsClockwise(currentVector, targetVector)) s *= -1F;
	    return -s * Vector2.Angle(currentVector, targetVector);
    }
	
}
