Shader "IBL/MatCap_01"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DiffuseLight ("Diffuse Light", 2D) = "black" {}
		_SpecularLight ("Specular Light", 2D) = "black" {}
		_AmbientLight ("Ambient Light", 2D) = "black" {}
		_CombinedLight ("Combined Light", 2D) = "black" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normalView : TEXCOORD1; // Noramls in viewspace
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CombinedLight;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normalView = mul(UNITY_MATRIX_V, float4(UnityObjectToWorldNormal(v.normal), 0.0)).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float3 normalView = normalize(i.normalView);
				col *= tex2D(_CombinedLight, normalView.xy * 0.5 + 0.5);
				return col;
			}
			ENDCG
		}
	}
}
