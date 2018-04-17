using System;
using UnityEngine;

namespace IkigaiGames.IBLDemo.Lighting
{
    /// <summary>
    /// Latest MatCap snapshot with all needed texture for PBR lighting 
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/MatCapSnapshot")]
    public class MatCapSnapshot : ScriptableObject
    {
        // Should be in POT
        private const int Resolution = 32;
        
        [Header("Runtime Only Values")]
        public Texture2D Ambient;
        public Texture2D Diffuse;
        public Texture2D Specular;
        // Abmient, Diffuse & Specular all cobmined in one texture
        // Specular goes in Alpha channel and can't transfer color
        public Texture2D Combined;

        public event Action OnMatCapUpdate;

        // Needed to be sure we never have empty fields to avoid NullRef when using this data structure
        private void OnEnable()
        {
            Ambient = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false, false);
            Diffuse = new Texture2D(Resolution, Resolution);
            Specular = new Texture2D(Resolution, Resolution);
            Combined = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, true, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                anisoLevel = 2
            };
        }

        public void UpdateTextures(Texture2D diffuse, Texture2D combined, Texture2D specular = null, Texture2D ambient = null)
        {
            if (diffuse != null)
            {
                Diffuse = diffuse;
            }

            if (combined != null)
            {
                Combined = combined;
            }

            if (specular != null)
            {
                Specular = specular;
            }

            if (ambient != null)
            {
                Ambient = ambient;
            }
            
            if (OnMatCapUpdate != null)
            {
                OnMatCapUpdate();
            }
        }
    }
}
