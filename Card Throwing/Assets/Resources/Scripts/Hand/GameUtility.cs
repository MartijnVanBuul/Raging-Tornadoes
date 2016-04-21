using UnityEngine;
using System.Collections;

/// <summary> Directional enum </summary>
public enum Direction {
    left, up, right, down
}

public enum Axis
{
    x,
    y,
    z
}

/// <summary> Script containing generic and useful static functions </summary>
public class GameUtility {

    /// <summary> Creates a string of the given time. </summary>
    /// <param name="time">What time in seconds.</param>
    /// <param name="twoDigitMinutes">If true, the minute part of the string always has at least 2 digits.</param>
    /// <returns>The string of the time</returns>
    public static string TimeString(float time, bool twoDigitMinutes = true) {
        int min = (int)(time / 60);
        int sec = (int)(time - (min * 60));

        string minStr = min + "";
        if (twoDigitMinutes && min < 10) minStr = "0" + minStr;
        string secStr = sec + "";
        if (sec < 10) secStr = "0" + secStr;

        return minStr + ":" + secStr;
    }

    /// <summary> Checks to see if given object is within layer mask. </summary>
    /// <param name="obj">What object to check.</param>
    /// <param name="mask">What layermask</param>
    /// <returns></returns>
    public static bool IsInLayerMask(GameObject obj, LayerMask mask) {
        return ((mask.value & (1 << obj.layer)) > 0);
    }

    /// <summary> Checks if given object is of the given type, or a subclass of </summary>
    /// <param name="obj"> What object to check </param>
    /// <param name="type"> What type to check for </param>
    /// <returns> True if it matches </returns>
    public static bool IsOfType(object obj, System.Type type) {
        if (obj == null) return false;
        System.Type objType = obj.GetType();
        if (objType == type || objType.IsSubclassOf(type)) {
            return true;
        }
        return false;
    }

    /// <summary> Gets the angle of a certain vector delta </summary>
    /// <param name="delta"> what delta vector to get the angle from </param>
    /// <returns> The angle </returns>
    public static float GetAngle(Vector3 delta) {
        float angle = Vector2.Angle(new Vector2(0, 1), delta);
        if (delta.x > 0) {
            angle *= -1;
        }
        return angle;
    }

    /// <summary> Rotates given vector by given degrees </summary>
    /// <param name="v">The vector to rotate</param>
    /// <param name="degrees">The amount of degrees to rotate it with</param>
    /// <returns> The rotated vector2 </returns>
    public static Vector2 RotateVector2(Vector2 v, float degrees) {
        if (degrees == 0) return v;
        float radians = degrees * Mathf.Deg2Rad;
        float ca = Mathf.Cos(radians);
        float sa = Mathf.Sin(radians);
        return new Vector2(v.x * ca - v.y * sa, v.x * sa + v.y * ca);
    }

    /// <summary> Brings the given angle back to 0 - 360 range  </summary>
    /// <param name="angle"> The angle to get into the correct range </param>
    /// <returns> Angle within a 0 - 360 range </returns>
    public static float CorrectAngle(float angle) {
        while (angle > 360) { angle -= 360; }
        while (angle < 0) { angle += 360; }
        return angle;
    }

    /// <summary> Brings the given angle to a range able to properly compare to given comparison </summary>
    /// <param name="angle"> The angle to get into the correct range </param>
    /// <param name="comparison"> The comparison to use </param>
    /// <returns> The angle within a -180 - 180 range to comparison </returns>
    public static float ComparisonAngle(float angle, float comparison) {
        comparison = CorrectAngle(comparison);
        while (angle > comparison + 180) { angle -= 360; };
        while (angle < comparison - 180) { angle += 360; };
        return angle;
    }

    /// <summary> Returns random value between min and max, but never less than min value </summary>
    /// <param name="min">Minimum </param>
    /// <param name="max">Maximum</param>
    /// <returns> Randomized value </returns>
    public static float PositiveRandom(float min, float max) {
        return (max > min) ? Random.Range(min, max) : min;
    }

    /// <summary> Returns random value between min and max, but never less than min value </summary>
    /// <param name="min">Minimum </param>
    /// <param name="max">Maximum</param>
    /// <returns> Randomized value </returns>
    public static int PositiveRandom(int min, int max) {
        return (max > min) ? Random.Range(min, max) : min;
    }

    /// <summary> Sets the alpha of this color </summary>
    /// <param name="c">What color</param>
    /// <param name="a">What alpha</param>
    /// <returns>Changed the alpha of the given color</returns>
    public static Color SetAlpha(Color c, float a) {
        c.a = a;
        return c;
    }
}
