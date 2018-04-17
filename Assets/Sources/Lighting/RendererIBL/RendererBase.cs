using IkigaiGames.IBLDemo.Core;
using IkigaiGames.IBLDemo.Lighting;
using UnityEngine;

namespace IkigaiGames.IBLDemo.RendererIBL
{
	public abstract class RendererBase : ScriptableObject
	{
		public abstract void Render(MatCapSnapshot matCapSnapshot, LightingRig lightingRig, CameraRuntimeRef cameraRef);
	}
}
