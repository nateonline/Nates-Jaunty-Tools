using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NatesJauntyTools
{
	public class Model : MonoBehaviour
	{
		[SerializeField] bool debugPointsOnDraw;
		[SerializeField] GameObject pointDebuggerPrefab;
		List<GameObject> instantiatedPointDebuggers = new List<GameObject>();


		#region Public Functions

		public Model(MeshFilter meshFilter)
		{
			this.meshFilter = meshFilter;
			mesh = new Mesh();
			this.meshFilter.mesh = mesh;
		}

		public void Clear()
		{
			ClearMesh();
			UpdateMesh();
		}

		#endregion


		#region Mesh Management

		protected MeshFilter meshFilter;
		protected Mesh mesh;
		protected List<Vector3> vertices = new List<Vector3>();
		protected List<int> triangles = new List<int>();
		protected List<Vector2> uvs = new List<Vector2>();

		protected void ClearMesh()
		{
			vertices.Clear();
			triangles.Clear();
			uvs.Clear();

			foreach (GameObject pointDebugger in instantiatedPointDebuggers) { Destroy(pointDebugger); }
			instantiatedPointDebuggers.Clear();
		}

		protected void UpdateMesh()
		{
			mesh.Clear();

			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();

			mesh.RecalculateNormals();
		}

		enum Facing { Up, Down, Left, Right, Forward, Back }

		Facing GetFacing(Vector3 dirA, Vector3 dirB, bool flipFace)
		{
			Vector3 cross = Vector3.Cross(dirA, dirB);

			Dictionary<Facing, float> angles = new Dictionary<Facing, float>();
			angles.Add(Facing.Down, Vector3.Angle(cross, Vector3.up));
			angles.Add(Facing.Up, Vector3.Angle(cross, Vector3.down));
			angles.Add(Facing.Left, Vector3.Angle(cross, Vector3.right));
			angles.Add(Facing.Right, Vector3.Angle(cross, Vector3.left));
			angles.Add(Facing.Back, Vector3.Angle(cross, Vector3.forward));
			angles.Add(Facing.Forward, Vector3.Angle(cross, Vector3.back));

			if (flipFace == false)
			{
				return angles.OrderBy(i => i.Value).First().Key;
			}
			else
			{
				return angles.OrderBy(i => i.Value).Last().Key;
			}
		}

		protected void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3, bool flipFace = false)
		{
			int vertIndex = vertices.Count;

			vertices.Add(p1);
			vertices.Add(p2);
			vertices.Add(p3);

			switch (GetFacing(p3 - p1, p2 - p1, flipFace))
			{
				case Facing.Up:
				case Facing.Down:
					uvs.Add(new Vector2(p1.x, p1.z));
					uvs.Add(new Vector2(p2.x, p2.z));
					uvs.Add(new Vector2(p3.x, p3.z));
					break;

				case Facing.Right:
				case Facing.Left:
					uvs.Add(new Vector2(p1.z, p1.y));
					uvs.Add(new Vector2(p2.z, p2.y));
					uvs.Add(new Vector2(p3.z, p3.y));
					break;

				case Facing.Forward:
				case Facing.Back:
					uvs.Add(new Vector2(p1.x, p1.y));
					uvs.Add(new Vector2(p2.x, p2.y));
					uvs.Add(new Vector2(p3.x, p3.y));
					break;
			}

			if (flipFace == false)
			{
				triangles.Add(vertIndex + 0);
				triangles.Add(vertIndex + 1);
				triangles.Add(vertIndex + 2);
			}
			else
			{
				triangles.Add(vertIndex + 0);
				triangles.Add(vertIndex + 2);
				triangles.Add(vertIndex + 1);
			}
		}

		protected void DrawQuad(List<Vector3> points, bool flipFace = false)
		{
			if (points.Count != 4) { throw new Exception($"AddQuad can't handle {points.Count} points"); }
			else { AddQuad(points[0], points[1], points[2], points[3], flipFace); }
		}

		protected void AddQuad(Vector3 topRightPoint, Vector3 bottomRightPoint, Vector3 bottomLeftPoint, Vector3 topLeftPoint, bool flipFace = false)
		{
			AddTriangle(topRightPoint, bottomRightPoint, bottomLeftPoint, flipFace);
			AddTriangle(topRightPoint, bottomLeftPoint, topLeftPoint, flipFace);
		}

		protected List<Vector3> AddRing(float radius, float height, int resolution = 300)
		{
			List<Vector3> newRing = new List<Vector3>();
			for (int i = 0; i < resolution; i++)
			{
				float angle = (i * Mathf.PI * 2f) / resolution;
				newRing.Add(new Vector3(Mathf.Sin(angle) * radius, height, Mathf.Cos(angle) * radius));
			}
			return newRing;
		}

		/// <summary> Both edge loops must be in clock-wise order </summary>
		protected void BridgeEdgeLoops(List<Vector3> bridgeFrom, List<Vector3> bridgeTo)
		{
			List<Vector3> smallLoop, largeLoop;
			bool invertTris;
			if (bridgeFrom.Count <= bridgeTo.Count)
			{
				smallLoop = bridgeFrom;
				largeLoop = bridgeTo;
				invertTris = false;
			}
			else
			{
				smallLoop = bridgeTo;
				largeLoop = bridgeFrom;
				invertTris = true;
			}

			float ratio = (float)(largeLoop.Count) / (smallLoop.Count);

			int lastFixerTriangle = -1;
			for (int largeLoopIndex = 0; largeLoopIndex < largeLoop.Count; largeLoopIndex++)
			{
				int assignment = (int)(largeLoopIndex / ratio);
				if (assignment - lastFixerTriangle >= 1)
				{
					AddTriangle(
						largeLoop.AtWrap(largeLoopIndex),
						smallLoop.AtWrap(lastFixerTriangle),
						smallLoop.AtWrap(lastFixerTriangle + 1),
						invertTris
					);
					lastFixerTriangle++;
				}

				AddTriangle(
					largeLoop.AtWrap(largeLoopIndex + 1),
					largeLoop.AtWrap(largeLoopIndex),
					smallLoop.AtWrap((int)(largeLoopIndex / ratio)),
					invertTris
				);
			}
		}

		void DebugPoints(List<Vector3> points, string label = "")
		{
			if (debugPointsOnDraw)
			{
				for (int i = 0; i < points.Count; i++)
				{
					GameObject pointDebugger = Instantiate(pointDebuggerPrefab, points[i], Quaternion.identity, transform);
					pointDebugger.name = $"{label} {i}";
					instantiatedPointDebuggers.Add(pointDebugger);
				}
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			foreach (GameObject pointDebugger in instantiatedPointDebuggers)
			{
				UnityEditor.Handles.color = Color.black;
				UnityEditor.Handles.Label(pointDebugger.transform.position, pointDebugger.name);
			}
		}
#endif

		#endregion
	}
}
