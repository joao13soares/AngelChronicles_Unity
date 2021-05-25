Shader "Unlit/ToonWater"
{
	Properties
	{
		_DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)


	    _FoamDistance("Foam Distance", Float) = 0.4



	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" }
			LOD 100

			Pass
			{

			// Add inside the Pass, just above the CGPROGRAM's start.
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"





			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 noiseUV : TEXCOORD0;
				float4 screenPosition : TEXCOORD2;

			};

			float4 _DepthGradientShallow;
			float4 _DepthGradientDeep;

			float _DepthMaxDistance;

			sampler2D _CameraDepthTexture;


			sampler2D _SurfaceNoise;
			float4 _SurfaceNoise_ST;
			float _SurfaceNoiseCutoff;

			float _FoamDistance;

			float2 _SurfaceNoiseScroll;




			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.screenPosition = ComputeScreenPos(o.vertex);

				o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);

				return o;
			}



			float4 frag(v2f i) : Color
			{
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
				float existingDepthLinear = LinearEyeDepth(existingDepth01);

				float depthDifference = existingDepthLinear - i.screenPosition.w;

				float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
				float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);



				// Animate uv for noise 
				float2 noiseUV = float2(i.noiseUV.x - _Time.y * _SurfaceNoiseScroll.x, i.noiseUV.y - _Time.y * _SurfaceNoiseScroll.y);
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;


				float foamDepthDifference01 = saturate(depthDifference / _FoamDistance); // saturate- clamps between 0 and 1
				float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

				float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;

				return waterColor + surfaceNoise;
			}
			ENDCG
		}
		}
}
