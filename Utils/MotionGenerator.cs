// Decompiled with JetBrains decompiler
// Type: Utils.MotionGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Utils
{
  public static class MotionGenerator
  {
    public static IEnumerator MoveWithEasing(
      Transform theTransform,
      Vector3 targetPosition,
      float durationSeconds,
      MotionGenerator.EasingType easingType,
      MotionGenerator.Axis axis)
    {
      if (!((Object) theTransform == (Object) null))
      {
        Vector3 startPos = theTransform.localPosition;
        float startTime = Time.time;
        float elapsed = 0.0f;
        while ((double) elapsed < (double) durationSeconds)
        {
          if ((Object) theTransform == (Object) null)
          {
            yield break;
          }
          else
          {
            elapsed = Time.time - startTime;
            float easedValue = MotionGenerator.GetEasedValue(Mathf.Clamp01(elapsed / durationSeconds), easingType);
            theTransform.localPosition = MotionGenerator.SetAxisValue(startPos, axis, Mathf.Lerp(MotionGenerator.GetAxisValue(startPos, axis), MotionGenerator.GetAxisValue(targetPosition, axis), easedValue));
            yield return (object) null;
          }
        }
        theTransform.localPosition = MotionGenerator.SetAxisValue(theTransform.localPosition, axis, MotionGenerator.GetAxisValue(targetPosition, axis));
      }
    }

    private static float GetEasedValue(float t, MotionGenerator.EasingType easingType)
    {
      float easedValue;
      switch (easingType)
      {
        case MotionGenerator.EasingType.Linear:
          easedValue = t;
          break;
        case MotionGenerator.EasingType.EaseIn:
          easedValue = t * t;
          break;
        case MotionGenerator.EasingType.EaseOut:
          easedValue = (float) (1.0 - (1.0 - (double) t) * (1.0 - (double) t));
          break;
        case MotionGenerator.EasingType.EaseInOut:
          easedValue = (float) (3.0 * (double) t * (double) t - 2.0 * (double) t * (double) t * (double) t);
          break;
        default:
          easedValue = t;
          break;
      }
      return easedValue;
    }

    private static float GetAxisValue(Vector3 vector, MotionGenerator.Axis axis)
    {
      float axisValue;
      switch (axis)
      {
        case MotionGenerator.Axis.X:
          axisValue = vector.x;
          break;
        case MotionGenerator.Axis.Y:
          axisValue = vector.y;
          break;
        case MotionGenerator.Axis.Z:
          axisValue = vector.z;
          break;
        default:
          axisValue = vector.x;
          break;
      }
      return axisValue;
    }

    private static Vector3 SetAxisValue(Vector3 vector, MotionGenerator.Axis axis, float value)
    {
      switch (axis)
      {
        case MotionGenerator.Axis.X:
          vector.x = value;
          break;
        case MotionGenerator.Axis.Y:
          vector.y = value;
          break;
        case MotionGenerator.Axis.Z:
          vector.z = value;
          break;
      }
      return vector;
    }

    public enum EasingType
    {
      Linear,
      EaseIn,
      EaseOut,
      EaseInOut,
    }

    public enum Axis
    {
      X,
      Y,
      Z,
    }
  }
}
