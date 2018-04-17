using UnityEngine;

namespace IkigaiGames.IBLDemo.Lighting
{
	public class LightingDataListener : MonoBehaviour
	{
		[SerializeField] private MatCapSnapshot _matCapSnapshot;

		private MaterialPropertyBlock _matPropertyBlock;
		private Renderer _renderer;

		private readonly int _diffuseHash = Shader.PropertyToID("_DiffuseLight");
		private readonly int _specularHash = Shader.PropertyToID("_SpecularLight");
		private readonly int _ambientHash = Shader.PropertyToID("_AmbientLight");
		private readonly int _combinedHash = Shader.PropertyToID("_CombinedLight");

		private void Awake()
		{
			_matPropertyBlock = new MaterialPropertyBlock();
			_renderer = GetComponent<Renderer>();
		}

		private void OnEnable()
		{
			_matCapSnapshot.OnMatCapUpdate += UpdateLightingData;
			
			// Need to update data on material in case LightingRig was updated while object was inactive
			UpdateLightingData();
		}

		private void OnDisable()
		{
			_matCapSnapshot.OnMatCapUpdate -= UpdateLightingData;
		}

		private void UpdateLightingData()
		{
			_renderer.GetPropertyBlock(_matPropertyBlock);
			
			_matPropertyBlock.SetTexture(_diffuseHash, _matCapSnapshot.Diffuse);
			_matPropertyBlock.SetTexture(_specularHash, _matCapSnapshot.Specular);
			_matPropertyBlock.SetTexture(_ambientHash, _matCapSnapshot.Ambient);
			_matPropertyBlock.SetTexture(_combinedHash, _matCapSnapshot.Combined);
			
			_renderer.SetPropertyBlock(_matPropertyBlock);
		}
	}
}
