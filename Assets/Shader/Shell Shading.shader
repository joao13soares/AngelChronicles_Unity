Shader "Unlit/Shell Shading"
{
	Properties
	{
		_ColorMap("ColorMap",2D)=""{}
		_NormalMap("Normal Map",2D)=""{}
		_DepthMap("Depth Map",2D)=""{}
		_NormalIntesity("Normal  Intesity", Range(0,2))=1
		_Color("Color", color) = (1,1,1,1)

		_Brightness("Brightness",Range(0,1)) = 0.4
		_Power("Power",Range(0,1))=0.5
		_Devisor("Devisor", Range(0,1))=0.5

		_Scale("Scale" ,Range(0,0.2))=0.04
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
				float4 vertex  : POSITION;
				float3 normal  : NORMAL;
				float3 tangent : TANGENT;
				float2 uv      : TEXCOORD0;

			};

			struct v2f
			{
				float2 uv     : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 ldir   : TEXCOORD1;
				float3 eye    : TEXCOORD2;
			};

			uniform float4 _Color;

			uniform sampler2D _ColorMap;
			uniform sampler2D _NormalMap; //para usar as normais feitas 
			uniform sampler2D _DepthMap;  //para usar as normais feitas 

			uniform float2 _NormalIntensity;
			uniform float _Scale;

			float4 _ColorMap_ST;
			float  _Brightness;
			float _Power;
			float _Devisor;


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _ColorMap);

				/*o.normal = UnityObjectToWorldNormal(v.normal);*/
				
				o.ldir = normalize(_WorldSpaceLightPos0.xyz);
				o.eye = normalize(_WorldSpaceCameraPos.xyz - (-mul(UNITY_MATRIX_M, v.vertex).xyz));  //para usar as normais feitas 

				float3 n = normalize(mul(UNITY_MATRIX_M, v.normal));  //para usar as normais feitas 
				float3 t = normalize(mul(UNITY_MATRIX_M, v.tangent)); //para usar as normais feitas 
				float3 b = normalize(cross(n, t));

				float3x3 tbn = float3x3(t, b, n); // world to tangent space //para usar as normais feitas 

				o.ldir = mul(tbn, o.ldir); //para usar as normais feitas 
				o.eye = mul(tbn, o.eye);  //para usar as normais feitas 
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				i.eye = normalize(i.eye);
				float depth = tex2D(_DepthMap, i.uv).r;
				float2 offset = i.eye.xy / i.eye.z * depth *_Scale;
				i.uv.y += offset.y;
				i.uv.x -= offset.x;

				float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));

				normal.xy *= _NormalIntensity;
				i.normal = normalize(normal);

				float3 h = normalize(i.ldir + i.eye);
				float ndoth = max(0.2f, dot(normal, h));

				float specular = float4(1, 1, 1, 1) * pow(ndoth, _Brightness);

				float ndotl = dot(normal, i.ldir);
				ndotl = max(0.2f, ndotl);
				ndotl = floor(ndotl / _Devisor);
				fixed4 col = tex2D(_ColorMap,i.uv);
				col *= ndotl * _Power * _Color + specular;
				return col;
			}
			ENDCG
		}
	}
}
