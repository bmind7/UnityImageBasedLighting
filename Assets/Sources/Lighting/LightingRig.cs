using System.Reflection.Emit;
using UnityEngine;

namespace IkigaiGames.IBLDemo.Lighting
{
	/// <summary>
	/// LightingRig data container which reference all needed data for texture baking
	/// it should not be instantiated during game
	/// </summary>
	[ExecuteInEditMode] 
	public class LightingRig : MonoBehaviour
	{
		private const string SupportedSkyboxShader = "Skybox/Cubemap";
		
		public Transform TargetObject;
		public Light[] Lights;
		public Cubemap AmbientLight;
		
		// Here we try to grab all changes of skybox in RenderSettings via [ExecuteInEditMode]
		// this way user not gonna need to set skybox in two different places during Lighting Rig setup
#if UNITY_EDITOR
		private Material _cachedSkyboxMat;

		private void Update()
		{
			// Update only for using in editor mode
			if (Application.isPlaying)
			{
				return;
			}
			
			if (RenderSettings.skybox == null || _cachedSkyboxMat == RenderSettings.skybox)
			{
				_cachedSkyboxMat = null;
				return;
			}
			
			_cachedSkyboxMat = RenderSettings.skybox;

			if (_cachedSkyboxMat.shader.name.Equals(SupportedSkyboxShader))
			{
				AmbientLight = (Cubemap) _cachedSkyboxMat.GetTexture("_Tex");
			}
			else
			{
				Debug.LogError("Unsupported Skybox material, please use one with cubemap");
			}
		}
#endif
	}
}
