Shader "My/ToonShaderNoNormalMap"
{
	Properties
	{
		_ColorMap("Color Map", 2D) = "" {}
		_Color("Color", color) = (0.38,0.38,0.38,1)
		_DivNdot("DivNdot",Range(0,1)) = 0.3
		_Brightness("Brightness",Range(0,1)) = 0.3 //Ambient Light
		_TextureStength("TextureStength",Range(0,1)) = 0.5
	}
		SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv   : TEXCOORD0;
				float3 WorldNormal : Normal;
				float3 ldir : TEXCOORD1;
				float3 eye  : TEXCOORD2;
			};

			uniform float4 _Color;
			uniform sampler2D _ColorMap;
			uniform float _DivNdot;
			uniform float _Brightness;
			uniform float _TextureStength;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.WorldNormal = normalize(UnityObjectToWorldNormal(v.normal));

				o.ldir = normalize(_WorldSpaceLightPos0.xyz);


				

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_ColorMap, i.uv);
				float ndotl = dot(i.WorldNormal, i.ldir);
				ndotl = max(0.0f, ndotl);
				float Ndot = floor(ndotl / _DivNdot);
				col *= Ndot * _TextureStength * _Color + _Brightness;
				return col;
			}
			ENDCG
		}
	}
}
