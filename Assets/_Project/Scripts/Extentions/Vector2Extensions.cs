using UnityEngine;

public static class Vector2Extensions
{
	public static float SqrDistance(this Vector2 start, Vector2 end)
	{
		return (end - start).sqrMagnitude;
	}

	public static bool IsEnoughClose(this Vector2 start, Vector2 end, float distance)
	{
		return start.SqrDistance(end) <= distance * distance;
	}
	
	public static Vector2 WithX(this Vector2 vector, float x)
	{
		return new Vector2(x, vector.y);
	}

	public static Vector2 WithY(this Vector2 vector, float y)
	{
		return new Vector2(vector.x, y);
	}
}
