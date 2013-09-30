using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Ext
{
	#region Transform

	/// <summary>
	/// Resets the transform to identity values.
	/// </summary>
	public static void SetIdentity(this Transform transform)
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	/// <summary>
	/// Sets the local x position of the transform.
	/// </summary>
	public static void SetX(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.x = value;
		transform.localPosition = p;
	}

	/// <summary>
	/// Sets the local y position of the transform.
	/// </summary>
	public static void SetY(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.y = value;
		transform.localPosition = p;
	}

	/// <summary>
	/// Sets the local z position of the transform.
	/// </summary>
	public static void SetZ(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.z = value;
		transform.localPosition = p;
	}

	/// <summary>
	/// Returns true if the transform is at the specified local position.
	/// </summary>
	public static bool IsAt(this Transform transform, Vector3 position)
	{
		return position.Approximately(transform.localPosition);
	}
	/// <summary>
	/// Returns true if the transform is at the specified local rotation.
	/// </summary>
	public static bool IsAt(this Transform transform, Quaternion rotation)
	{
		return rotation.Approximately(transform.localRotation);
	}
	/// <summary>
	/// Returns true if the transform is at the specified local position and rotation.
	/// </summary>
	public static bool IsAt(this Transform transform, Vector3 position, Quaternion rotation)
	{
		return position.Approximately(transform.localPosition) && rotation.Approximately(transform.localRotation);
	}
	/// <summary>
	/// Returns true if the two transforms are at the same position.
	/// </summary>
	public static bool IsAt(this Transform transform, Transform other)
	{
		return transform.IsAt(other.localPosition, other.localRotation);
	}
	/// <summary>
	/// Returns true if the two transforms are at the same position (and rotation, optionally).
	/// </summary>
	public static bool IsAt(this Transform transform, Transform other, bool compareRotation)
	{
		return transform.IsAt(other.localPosition) && (!compareRotation || transform.IsAt(other.localRotation));
	}

	/// <summary>
	/// Returns the normalized direction from the transform to the specified position.
	/// </summary>
	public static Vector3 DirectionTo(this Transform transform, Vector3 position)
	{
		return Vector3.Normalize(position - transform.position);
	}
	/// <summary>
	/// Returns the normalized direction from the transform to the specified transform.
	/// </summary>
	public static Vector3 DirectionTo(this Transform transform, Transform other)
	{
		return Vector3.Normalize(other.position - transform.position);
	}

	/// <summary>
	/// Returns the distance between the transform and the specified position.
	/// </summary>
	public static float DistanceTo(this Transform transform, Vector3 position)
	{
		return Vector3.Distance(transform.position, position);
	}
	/// <summary>
	/// Returns the distance between two transform.
	/// </summary>
	public static float DistanceTo(this Transform transform, Transform other)
	{
		return Vector3.Distance(transform.position, other.position);
	}

	/// <summary>
	/// Moves the transform towards the target position by the specified amount.
	/// </summary>
	public static void MoveTowards(this Transform transform, Vector3 target, float amount)
	{
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, amount);
	}
	/// <summary>
	/// Moves the transform towards the target transform by the specified amount.
	/// </summary>
	public static void MoveTowards(this Transform transform, Transform target, float amount)
	{
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, amount);
	}

	/// <summary>
	/// Rotates the transform towards the target rotation by the specified angle.
	/// </summary>
	public static void RotateTowards(this Transform transform, Quaternion target, float angle)
	{
		transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target, angle);
	}
	/// <summary>
	/// Rotates the transform towards the target transform's rotation by the specified angle.
	/// </summary>
	public static void RotateTowards(this Transform transform, Transform target, float angle)
	{
		transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target.localRotation, angle);
	}

	/// <summary>
	/// Sets the transform's parent.
	/// </summary>
	public static void SetParent(this Transform transform, Transform parent)
	{
		transform.parent = parent;
	}
	/// <summary>
	/// Sets the transform's parent and local position within that parent.
	/// </summary>
	public static void SetParent(this Transform transform, Transform parent, Vector3 position)
	{
		transform.parent = parent;
		transform.localPosition = position;
	}
	/// <summary>
	/// Sets the transform's parent and local position/rotation within that parent.
	/// </summary>
	public static void SetParent(this Transform transform, Transform parent, Vector3 position, Quaternion rotation)
	{
		transform.parent = parent;
		transform.localPosition = position;
		transform.localRotation = rotation;
	}
	
	#endregion

	#region Array & List

	/// <summary>
	/// Returns the first item in the array.
	/// </summary>
	public static T First<T>(this T[] items)
	{
		return items[0];
	}
	/// <summary>
	/// Returns the first item in the list.
	/// </summary>
	public static T First<T>(this List<T> items)
	{
		return items[0];
	}

	/// <summary>
	/// Returns the last item in the array.
	/// </summary>
	public static T Last<T>(this T[] items)
	{
		return items[items.Length - 1];
	}
	/// <summary>
	/// Returns the last item in the list.
	/// </summary>
	public static T Last<T>(this List<T> items)
	{
		return items[items.Count - 1];
	}

	/// <summary>
	/// Removes the last item from the list and returns it.
	/// </summary>
	public static T Pop<T>(this List<T> items)
	{
		var item = items[items.Count - 1];
		items.RemoveAt(items.Count - 1);
		return item;
	}
	/// <summary>
	/// Removes the item from the list at the index and returns it.
	/// </summary>
	public static T Pop<T>(this List<T> items, int index)
	{
		var item = items[index];
		items.RemoveAt(index);
		return item;
	}

	/// <summary>
	/// Returns true if the array contains the item.
	/// </summary>
	public static bool Contains<T>(this T[] items, T item)
	{
		for (int i = 0; i < items.Length; i++)
			if (item.Equals(items[i]))
				return true;
		return false;
	}
	/// <summary>
	/// Returns true if the array contains the item and assigns it to the specified index.
	/// </summary>
	public static bool Contains<T>(this T[] items, T item, out int index)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (item.Equals(items[i]))
			{
				index = i;
				return true;
			}
		}
		index = -1;
		return false;
	}
	/// <summary>
	/// Returns true if the list contains the item and assigns it to the specified index.
	/// </summary>
	public static bool Contains<T>(this List<T> items, T item, out int index)
	{
		index = items.IndexOf(item);
		return index >= 0;
	}

	/// <summary>
	/// Returns the index of the item in the array, or -1 if the item is not in the array.
	/// </summary>
	public static int IndexOf<T>(this T[] items, T item)
	{
		for (int i = 0; i < items.Length; i++)
			if (item.Equals(items[i]))
				return i;
		return -1;
	}

	/// <summary>
	/// Removes all null objects from the list.
	/// </summary>
	public static void RemoveNulls<T>(this List<T> items) where T : Object
	{
		if (items.Contains(null))
		{
			for (int i = 0; i < items.Count; i++)
				if (items[i] == null)
					items.RemoveAt(i--);
		}
	}

	/// <summary>
	/// Returns a random value from the array.
	/// </summary>
	public static T Choose<T>(this T[] items)
	{
		return items[Random.Range(0, items.Length)];
	}
	/// <summary>
	/// Returns a random value from the list.
	/// </summary>
	public static T Choose<T>(this List<T> items)
	{
		return items[Random.Range(0, items.Count)];
	}

	#endregion
}
