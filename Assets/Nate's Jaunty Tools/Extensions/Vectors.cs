using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Converts a Vector2 from the XY plane to the XZ plane. </summary>
		/// <param name="y"> The height (y) value of the new XZ vector. </param>
		public static Vector3 ToXZPlane(this Vector2 xyVector, float y = 0f)
		{
			return new Vector3(xyVector.x, y, xyVector.y);
		}

		/// <summary> Converts a Vector2 from the XY plane to the YZ plane. </summary>
		/// <param name="y"> The width (x) value of the new YZ vector. </param>
		public static Vector3 ToYZPlane(this Vector2 xyVector, float x = 0f)
		{
			return new Vector3(x, xyVector.y, xyVector.x);
		}

		/// <summary> Creates a random Vector3 on the unit sphere, meaning it will always have a magnitude of exactly one unit. </summary>
		/// <param name="allowInsideRadius"> Allows the result to have a magnitude of less than one. </param>
		public static Vector3 RandomUnitVector3(bool allowInsideRadius = false)
		{
			float radius;
			if (allowInsideRadius) { radius = Random.value; }
			else { radius = 1f; }

			float angleAround = Random.Range(0, 360);
			float angleDown = Random.Range(0, 360);

			float x = radius * Mathf.Cos(angleAround) * Mathf.Sin(angleDown);
			float y = radius * Mathf.Cos(angleDown);
			float z = radius * Mathf.Sin(angleAround) * Mathf.Sin(angleDown);

			return new Vector3(x, y, z);
		}

		/// <summary> Creates a random Vector2 on the unit circle, meaning it will always have a magnitude of exactly one unit. </summary>
		/// <param name="allowInsideRadius"> Allows the result to have a magnitude of less than one. </param>
		public static Vector2 RandomUnitVector2(bool allowInsideRadius = false)
		{
			float radius;
			if (allowInsideRadius) { radius = Random.value; }
			else { radius = 1f; }

			float angle = Random.Range(0, 360);

			float x = radius * Mathf.Cos(angle);
			float y = radius * Mathf.Sin(angle);

			return new Vector2(x, y);
		}

		public static Vector3 RotateAroundPivot(this ref Vector3 point, Vector3 pivot, Vector3 angles)
		{
			Vector3 dir = point - pivot; // get point direction relative to pivot
			dir = Quaternion.Euler(angles) * dir; // rotate it
			point = dir + pivot; // calculate rotated point
			return point; // return it
		}

		public static Vector3 RotateAroundPivot(this ref Vector3 point, Vector3 pivot, float yAngle)
		{
			Vector3 dir = point - pivot; // get point direction relative to pivot
			dir = Quaternion.Euler(new Vector3(0, yAngle, 0)) * dir; // rotate it
			point = dir + pivot; // calculate rotated point
			return point; // return it
		}
	}
}
