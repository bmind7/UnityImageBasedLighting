using IkigaiGames.IBLDemo.Core;
using IkigaiGames.IBLDemo.RendererIBL;
using UnityEngine;

namespace IkigaiGames.IBLDemo.Lighting
{
    /// <summary>
    /// Manager responsible for creating MatCap snapshot from the provided lighting rig
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/LightingRigFactory")]
    public class LightingRigFactory : ScriptableObject
    {
        [Header("Runtime Vars")]
        [SerializeField, Tooltip("Overwriten during runtime")] private GameObject _activeLightingRigPrefab;

        [Header("Setup Varialbes")] 
        [SerializeField] private RendererBase _rendererIBL;
        [SerializeField] private MatCapSnapshot _matCapSnapshot;
        [SerializeField] private CameraRuntimeRef _cameraRef;

        /// <summary>
        /// Bake light textures from provided lighting rig
        /// </summary>
        /// <param name="lightingRigPrefab"></param>
        public void BuildRig(LightingRig lightingRigPrefab)
        {
            if (lightingRigPrefab == null)
            {
                Debug.LogError("LightingRigPrefab not provided");
            }
            
            _rendererIBL.Render(_matCapSnapshot, lightingRigPrefab, _cameraRef);
        }
    }
}
