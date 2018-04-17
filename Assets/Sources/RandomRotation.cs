using UnityEngine;

namespace IkigaiGames.IBLDemo
{
	public class RandomRotation : MonoBehaviour
	{
		[SerializeField, Range(10f, 100f)] private float _rotationSpeed = 10f;
		
		private Vector3 _rotationAxis;
		private Transform _transform;
			
		private void Start ()
		{
			_rotationAxis = Random.onUnitSphere;
			_transform = transform;
		}
		
		private void Update () {
			_transform.Rotate(_rotationAxis, _rotationSpeed * Time.deltaTime);
		}
	}
}
