Shader "Unlit/ToonWater"
{
	Properties
	{
		// Shallow and Deep water color (minimum and maximum in gradient)
		_DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
		
		//Max depth which defines when its the most deep it can go in terms of color(max: _DepthGradientDeep)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		//Noise Texture
		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		
		//Value which cuts the noise completely to just show "top parts"
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		
		//Surface Animation motion
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

		// MaxDistance Foam can reach from the shore
	    _FoamDistance("Foam Distance", Float) = 0.4
	}
	
		SubShader
		{
			Tags { "RenderType" = "Transparent" }

			Pass
			{

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

				// Gets screen position of the vertex
				o.screenPosition = ComputeScreenPos(o.vertex);

				// Allows tiling and offset of texture 
				 o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
				

				return o;
			}


			float4 frag(v2f i) : Color
			{


				// Gets depth value from the position of the screen position according to camera
				float nonLinearDepthValueToCamera = tex2D(_CameraDepthTexture,i.screenPosition / i.screenPosition.w).r;
				// only needs one channel since its greyscale
				
				float linearDepthValueToCamera = LinearEyeDepth(nonLinearDepthValueToCamera);

				
				float depthValueToWater = linearDepthValueToCamera - i.screenPosition.w;
				// .w is the depth of the water according to the camera
				// ExistingDepthLinear- waterDepth gives us the depth from the object according to water instead of according to camera

				

				// Gets percentage from the actual depth til the max depth value and clamps it between 0-1
				float percentageOfDepthToWaterToMaxDistance = saturate(depthValueToWater / _DepthMaxDistance);

				// Gets water color by linear interpolation of the eprcentage between the colors chosen for the shallow and deep water
				float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, percentageOfDepthToWaterToMaxDistance);



				// Animate uv for noise 
				float2 noiseUV = float2(i.noiseUV.x - _Time.y * _SurfaceNoiseScroll.x, i.noiseUV.y - _Time.y * _SurfaceNoiseScroll.y);
				// Gets noise value in red channel since its greyscale
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;


				
				float clampedFoamDepth = saturate(depthValueToWater / _FoamDistance); // saturate- clamps between 0 and 1

				
				float surfaceNoiseCutoff = clampedFoamDepth * _SurfaceNoiseCutoff;
				float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;

				return waterColor + surfaceNoise;
			}
			ENDCG
		}
		}
}
