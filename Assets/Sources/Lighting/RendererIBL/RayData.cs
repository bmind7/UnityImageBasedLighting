using IkigaiGames.IBLDemo.Lighting;
using UnityEngine;

namespace IkigaiGames.IBLDemo.RendererIBL
{
    public struct RayData
    {
        public Vector3 RayDirection;
        public LightingRig LightingRig;
        public Vector3 EyePosition;
        public float Epsilon;
    }
}
