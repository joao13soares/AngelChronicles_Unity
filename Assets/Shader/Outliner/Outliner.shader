Shader "Unlit/Outliner"
{
	Properties
	{
		_OutlineColor("OutlineColor", Color) = (255,255,255,255)
		_OutlineWidth("OutlineWidth", Range(0,0.001)) = 0



		_ColorMap("Color Map", 2D) = "" {}
	    _NormalMap("Normal Map", 2D) = "" {}
	    _Color("Color", color) = (0.38,0.38,0.38,1)
		_DivNdot("DivNdot",Range(0,1)) = 0.3
		_Brightness("Brightness",Range(0,1)) = 0.3 //Ambient Light
		_TextureStength("TextureStength",Range(0,1)) = 0.5
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
				"Queue" = "Geometry+100"
			}

			ZTest Always
			ZWrite Off

			// front line

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
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				float _OutlineWidth;

				v2f vert(appdata v)
				{
					v2f o;


					float3 worldNormal = mul(float4(v.normal, 0), unity_WorldToObject);


					o.vertex = UnityObjectToClipPos(v.vertex + _OutlineWidth * v.normal);
					return o;
				}


				float4 _OutlineColor;

				fixed4 frag(v2f i) : SV_Target
				{
					return _OutlineColor;
				}
				ENDCG

			}




				ZTest Less
				ZWrite On


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
					float3 ldir : TEXCOORD1;
					float3 eye  : TEXCOORD2;
				};

				uniform float4 _Color;
				uniform sampler2D _ColorMap;
				uniform sampler2D _NormalMap;
				uniform float _DivNdot;
				uniform float _Brightness;
				uniform float _TextureStength;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;

					o.ldir = normalize(_WorldSpaceLightPos0.xyz);
					o.eye = normalize(_WorldSpaceCameraPos.xyz - mul(UNITY_MATRIX_M, v.vertex).xyz);

					float3 n = normalize(mul(UNITY_MATRIX_M, v.normal));
					float3 t = normalize(mul(UNITY_MATRIX_M, v.tangent));
					float3 b = normalize(cross(n, t));
					float3x3 tbn = float3x3(t, b, n); // World to tangent space

					o.ldir = mul(tbn, o.ldir);
					o.eye = mul(tbn, o.eye);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
					normal = normalize(normal);

					i.eye = normalize(i.eye);
					float3 h = normalize(i.ldir + i.eye);
					float ndoth = max(0.0, dot(normal, h));

					float4 col = tex2D(_ColorMap, i.uv);
					float ndotl = dot(normal, i.ldir);
					ndotl = max(0.0f, ndotl);
					float Ndot = floor(ndotl / _DivNdot);
					col *= Ndot * _TextureStength * _Color + _Brightness;
					return col;
				}
					ENDCG
				}
			
				Pass
		{
			Name "CastShadow"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}

		}
}