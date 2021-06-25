using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Changes a single material for a renderer's list of materials. </summary>
		public static void ChangeMaterial(this Renderer renderer, int index, Material newMaterial)
		{
			Material[] materials = renderer.materials;

			if (0 <= index && index < materials.Length)
			{
				materials[index] = newMaterial;
				renderer.materials = materials;
			}
			else
			{
				Debug.LogWarning($"Couldn't set material: Materials has no index {index}", renderer.gameObject);
			}
		}

		/// <summary> Changes a single material for a renderer's list of materials. </summary>
		public static void ChangeSharedMaterial(this Renderer renderer, int index, Material newMaterial)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;

			if (0 <= index && index < sharedMaterials.Length)
			{
				sharedMaterials[index] = newMaterial;
				renderer.sharedMaterials = sharedMaterials;
			}
			else
			{
				Debug.LogWarning($"Couldn't set material: Materials has no index {index}", renderer.gameObject);
			}
		}
	}
}
