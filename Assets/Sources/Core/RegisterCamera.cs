using UnityEngine;

namespace IkigaiGames.IBLDemo.Core
{ 
	/// <summary>
	/// Helper script which provide active camera data to CameraRuntimeRef scriptable object
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class RegisterCamera : MonoBehaviour
	{
		[SerializeField] private CameraRuntimeRef _cameraRuntimeRef;

		private Camera _camera;

		private void Awake()
		{
			_camera = GetComponent<Camera>();
		}

		private void OnEnable()
		{
			_cameraRuntimeRef.Register(_camera);
		}

		private void OnDisable()
		{
			_cameraRuntimeRef.Unregister(_camera);
		}
	}
}
