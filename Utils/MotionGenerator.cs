using System.Collections;
using UnityEngine;

namespace Utils;

public static class MotionGenerator
{
	public enum EasingType
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut
	}

	public enum Axis
	{
		X,
		Y,
		Z
	}

	public static IEnumerator MoveWithEasing(Transform theTransform, Vector3 targetPosition, float durationSeconds, EasingType easingType, Axis axis)
	{
		if (theTransform == null)
		{
			yield break;
		}
		Vector3 startPos = theTransform.localPosition;
		float startTime = Time.time;
		float elapsed = 0f;
		while (elapsed < durationSeconds)
		{
			if (theTransform == null)
			{
				yield break;
			}
			elapsed = Time.time - startTime;
			float easedValue = GetEasedValue(Mathf.Clamp01(elapsed / durationSeconds), easingType);
			Vector3 localPosition = SetAxisValue(startPos, axis, Mathf.Lerp(GetAxisValue(startPos, axis), GetAxisValue(targetPosition, axis), easedValue));
			theTransform.localPosition = localPosition;
			yield return null;
		}
		Vector3 localPosition2 = theTransform.localPosition;
		localPosition2 = SetAxisValue(localPosition2, axis, GetAxisValue(targetPosition, axis));
		theTransform.localPosition = localPosition2;
	}

	private static float GetEasedValue(float t, EasingType easingType)
	{
		return easingType switch
		{
			EasingType.Linear => t, 
			EasingType.EaseIn => t * t, 
			EasingType.EaseOut => 1f - (1f - t) * (1f - t), 
			EasingType.EaseInOut => 3f * t * t - 2f * t * t * t, 
			_ => t, 
		};
	}

	private static float GetAxisValue(Vector3 vector, Axis axis)
	{
		return axis switch
		{
			Axis.X => vector.x, 
			Axis.Y => vector.y, 
			Axis.Z => vector.z, 
			_ => vector.x, 
		};
	}

	private static Vector3 SetAxisValue(Vector3 vector, Axis axis, float value)
	{
		switch (axis)
		{
		case Axis.X:
			vector.x = value;
			break;
		case Axis.Y:
			vector.y = value;
			break;
		case Axis.Z:
			vector.z = value;
			break;
		}
		return vector;
	}
}
