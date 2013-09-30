using UnityEngine;
using System.Collections;

public static class Calc
{
	/// <summary>
	/// Returns true if the value is an even number.
	/// </summary>
	public static bool IsEven(this int value)
	{
		return (value % 2) == 0;
	}

	/// <summary>
	/// Returns true if the value is an odd number.
	/// </summary>
	public static bool IsOdd(this int value)
	{
		return !IsEven(value);
	}
	
	/// <summary>
	/// Returns true if the two points are approximately equal.
	/// </summary>
	public static bool Approximately(this Vector3 point, Vector3 other)
	{
		return Mathf.Approximately(point.x, other.x) && Mathf.Approximately(point.y, other.y) && Mathf.Approximately(point.z, other.z);
	}
	/// <summary>
	/// Returns true if the two quaternions are approximately equal.
	/// </summary>
	public static bool Approximately(this Quaternion rotation, Quaternion other)
	{
		return Quaternion.Angle(rotation, other) < 0.0001f;
	}

	/// <summary>
	/// Returns true if the point is approximately zero.
	/// </summary>
	public static bool IsZero(this Vector3 point)
	{
		return Approximately(point, Vector3.zero);
	}

	/// <summary>
	/// Sets the vector's magnitude.
	/// </summary>
	public static Vector3 SetMagnitude(this Vector3 vector, float magnitude)
	{
		return vector.normalized * magnitude;
	}

	/// <summary>
	/// Returns the polar direction around the z axis.
	/// </summary>
	public static Vector3 PolarZ(float angle, float magnitude)
	{
		return new Vector3(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude, 0);
	}

	/// <summary>
	/// Returns the polar direction around the y axis.
	/// </summary>
	public static Vector3 PolarY(float angle, float magnitude)
	{
		return new Vector3(Mathf.Cos(angle) * magnitude, 0, Mathf.Sin(angle) * magnitude);
	}

	/// <summary>
	/// Returns true if any of the vector's components are NaN.
	/// </summary>
	public static bool HasNan(this Vector3 point)
	{
		return float.IsNaN(point.x) || float.IsNaN(point.y) || float.IsNaN(point.z);
	}

	/// <summary>
	/// Clamps the value within a range.
	/// </summary>
	public static int Clamp(this int value, int min, int max)
	{
		if (max > min)
			return Mathf.Min(max, Mathf.Max(min, value));
		else
			return Mathf.Min(min, Mathf.Max(max, value));
	}
	/// <summary>
	/// Clamps the value within a range.
	/// </summary>
	public static float Clamp(this float value, float min, float max)
	{
		if (max > min)
			return Mathf.Min(max, Mathf.Max(min, value));
		else
			return Mathf.Min(min, Mathf.Max(max, value));
	}
	/// <summary>
	/// Clamps the vector components within a range.
	/// </summary>
	public static Vector3 Clamp(this Vector3 val, Vector3 min, Vector3 max)
	{
		val.x = Clamp(val.x, min.x, max.x);
		val.y = Clamp(val.y, min.y, max.y);
		val.z = Clamp(val.z, min.z, max.z);
		return val;
	}

	/// <summary>
	/// Maps a value from one range to another.
	/// </summary>
	public static float Map(this float value, float min, float max, float min2, float max2)
	{
		return min2 + ((value - min) / (max - min)) * (max2 - min2);
	}

	/// <summary>
	/// Maps a value from one range to another, clamping it to the range.
	/// </summary>
	public static float MapClamp(this float value, float min, float max, float min2, float max2)
	{
		return Clamp(Map(value, min, max, min2, max2), min2, max2);
	}

	/// <summary>
	/// Interpolates from one value to another.
	/// </summary>
	public static float Lerp(float from, float to, float t)
	{
		return from + (to - from) * t;
	}
	/// <summary>
	/// Interpolates from one vector to another.
	/// </summary>
	public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
	{
		return from + (to - from) * t;
	}

	/// <summary>
	/// Calculates a value along a bezier curve.
	/// </summary>
	public static float Bezier(float from, float control, float to, float t)
    {
        return from * (1 - t) * (1 - t) + control * 2 * (1 - t) * t + to * t * t;
    }
	/// <summary>
	/// Calculates a position along a bezier curve.
	/// </summary>
    public static Vector3 Bezier(Vector3 from, Vector3 control, Vector3 to, float t)
    {
        from.x = Bezier(from.x, control.x, to.x, t);
        from.y = Bezier(from.y, control.y, to.y, t);
		from.z = Bezier(from.z, control.z, to.z, t);
        return from;
    }

	/// <summary>
	/// Returns the control point of a bezier curve with the specified end points and arc height.
	/// </summary>
	public static Vector3 BezierControl(Vector3 from, Vector3 to, float arcHeight, Vector3 upDirection)
	{
		return Vector3.Lerp(from, to, 0.5f) + upDirection * arcHeight * 2;
	}
	/// <summary>
	/// Returns the control point of a bezier curve with the specified end points and arc height. Assumes Vector3.up is the up vector.
	/// </summary>
	public static Vector3 BezierControl(Vector3 from, Vector3 to, float arcHeight)
	{
		return Vector3.Lerp(from, to, 0.5f) + Vector3.up * arcHeight * 2;
	}

	/// <summary>
	/// Snaps the value to the nearest unit.
	/// </summary>
	public static float Snap(this float value)
	{
		return Mathf.Round(value);
	}
	/// <summary>
	/// Snaps the value to the nearest grid unit.
	/// </summary>
	public static float Snap(this float value, float grid)
	{
		return Mathf.Round(value / grid) * grid;
	}
	/// <summary>
	/// Snaps the vector to the nearest unit.
	/// </summary>
	public static Vector3 Snap(this Vector3 value)
	{
		value.x = Mathf.Round(value.x);
		value.y = Mathf.Round(value.y);
		value.z = Mathf.Round(value.z);
		return value;
	}
	/// <summary>
	/// Snaps the vector to the nearest grid unit.
	/// </summary>
	public static Vector3 Snap(this Vector3 value, float grid)
	{
		value.x = Snap(value.x, grid);
		value.y = Snap(value.y, grid);
		value.z = Snap(value.z, grid);
		return value;
	}
}
