using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 ToVector3XY0(this Vector2 vector2) => new Vector3(vector2.x, vector2.y, 0);
    public static Vector3 ToVector3X0Y(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);
    public static Vector3 ToVector30XY(this Vector2 vector2) => new Vector3(0, vector2.x, vector2.y);
    public static Vector3 ToVector3YX0(this Vector2 vector2) => new Vector3(vector2.y, vector2.x, 0);
    public static Vector3 ToVector3Y0X(this Vector2 vector2) => new Vector3(vector2.y, 0, vector2.x);
    public static Vector3 ToVector30YX(this Vector2 vector2) => new Vector3(0, vector2.y, vector2.x);

    public static Vector3 Multiplication(this Vector3 v1, Vector3 v2) =>
        new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
}
