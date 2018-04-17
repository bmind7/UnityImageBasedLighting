using System;
using IkigaiGames.IBLDemo.Core;
using IkigaiGames.IBLDemo.Lighting;
using UnityEngine;

namespace IkigaiGames.IBLDemo.RendererIBL
{
	[CreateAssetMenu(menuName = "ScriptableObjects/CPURenderer")]
	public class CPURenderer : RendererBase
	{
		private const int MaxMarchingSteps = 20;
		private const float SphereRadius = 0.5f;
		
		[Tooltip("Can be treated as camera field of view. The bigger distance the fewer FoV")]
		[SerializeField, Range(1.4f, 10f)] private float _distanceToSphere = 1.4f;

		// Min distance to spere, calculated according texture resolution
		private float _epsilon;

		// TODO: implement split texture rendering for Diffuse, Alpha & Specular (now supported only Combined)
		// TODO: implement viriable view position (now supported only along Z axis) 
		public override void Render(MatCapSnapshot matCapSnapshot, LightingRig lightingRig, CameraRuntimeRef cameraRef)
		{
			Texture2D combined = matCapSnapshot.Combined;

			Color[] pixels = combined.GetPixels();

			// Min distance to  sphere caluclated according to texel size
			float epsilon = GetEpsilon(combined.width, combined.height); 
			
			// Pixels layed out left to right, bottom to top
			for (int row = combined.height-1; row >= 0; row--)
			{
				for (int col = 0; col < combined.width; col++)
				{
					var rayData = new RayData();
					rayData.RayDirection = GetRayDirection(combined.width, col, combined.height, row, cameraRef);
					rayData.LightingRig = lightingRig;
					rayData.EyePosition = new Vector3(0, 0, -_distanceToSphere);
					rayData.Epsilon = epsilon;
					
					// TODO: use new Unity data oriented desing with ECS and Burst
					// This task could be parallelized
					int index = row * combined.width + col;
					pixels[index] = RenderRay(rayData);
				}
			}
			
			combined.SetPixels(pixels);
			combined.Apply();
		}

		private float GetEpsilon(float width, float height)
		{
			float epsilonX = 2f * SphereRadius / width;
			float epsilonY = 2f * SphereRadius / height;
			return Mathf.Sqrt(epsilonX * epsilonX + epsilonY * epsilonY);
		}

		private Vector3 GetRayDirection(float width, float col, float height, float row, CameraRuntimeRef cameraRef)
		{
			return new Vector3(col/width - 0.5f, row/height - 0.5f, _distanceToSphere).normalized;			
		}
		
		private Color RenderRay(RayData rayData)
		{
			float depth = 0;

			for (int step = 0; step < MaxMarchingSteps; step++)
			{
				Vector3 samplePoint = rayData.EyePosition + depth * rayData.RayDirection;
				float distance = SphereSDF(samplePoint);

				if (distance < rayData.Epsilon)
				{
					// Since our sphere located in the center of the world we can easlity get normal of
					// the point where we touching sphere by simple normalization
					Vector3 normal = samplePoint.normalized;
					
					// TODO: implement Specualr and Ambient sampling
					Color color = GetDiffuseColor(normal, rayData.LightingRig, hitPoint: samplePoint);
					return color;
				}

				depth += distance;
				
				// Make sure we are not going too far from sphere
				if (depth > _distanceToSphere * 2)
				{
					break;
				}
			}
			
			return Color.black;
		}

		private float SphereSDF(Vector3 worldPosition)
		{
			// We assuming that sphere located in the center of the world
			return worldPosition.magnitude - SphereRadius;
		}

		//#TODO: implement Spotlight and Area light support 
		private Color GetDiffuseColor(Vector3 normal, LightingRig lightingRig, Vector3 hitPoint)
		{
			Color color = Color.black;
			// We can use "foreach" because Array doesn't create garbage
			foreach (Light light in lightingRig.Lights)
			{
				Vector3 direction;
				float intensity;
				
				switch (light.type)
				{
					case LightType.Directional:
						intensity = light.intensity;
						// inverse  because we need direction from sampling point to light
						direction = -light.transform.forward;
						break;
					case LightType.Point:
						var transform = light.transform;
						Vector3 distance = transform.localPosition - hitPoint;
						intensity = light.intensity * Mathf.Sqrt(distance.magnitude);
						direction = distance.normalized;
						break;
					default:
						continue;
				}

				// Lambert lighting
				color += light.color * intensity * Math.Max(0, Vector3.Dot(normal, direction));
			}

			return color;
		}
	}
}
