Shader "My/GrassShader"
{
    Properties
    {
		_MainTex("Grass Heigh Map", 2D) = "white" {}
		_Height("Height",Range(0,10)) = 2
		_Widht("Widht",Range(0,10)) = 2
		_LeafBottomColor("GrassLeaves Bottom Color", Color) = (0, .5, 0, 1)
		_LeafTopColor("GrassLeaves Top Color", Color) = (0, 1, 0, 1)
		_Thickness("Thickness",Range(0,5)) = 1
		_Gravity("Gravity",Range(0,10)) = 1
		_DirectionIntensity("DirectionIntensity",Range(0,10)) = 1
		_LengthIntensity("LengthIntensity",Range(0,10)) = 1

		_Noise("Noise",2D) = "white" {}

		_Wind("Wind",2D) = "white" {}
		_WindFrequency("WindFrequency",Range(0,10)) = 1
		_WindMin("WindMin",Range(0,10)) = 1
		_WindMax("WindMax",Range(0,10)) = 10


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma geometry geom
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct appdata
            {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

			struct v2g
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 grassOrientation : TEXCOORD0;
				float4 wind : TEXCOORD1;
			};

            struct g2f
            {
				float4 grassLeafColor : COLOR;
                float4 vertex : SV_POSITION;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Height;
			float _Widht;
			float4 _LeafBottomColor;
			float4 _LeafTopColor;
			float _Thickness;


			sampler2D _Noise;
			sampler2D _Noise_ST;
			float _DirectionIntensity;
			float _LengthIntensity;


			sampler2D _Wind;
			float4 _Wind_ST;
			float _WindFrequency;
			float _WindMin;
			float _WindMax;

			v2g  vert (appdata v)
            {
				v2g o;
				o.vertex = mul(unity_ObjectToWorld, v.vertex);
				v.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.grassOrientation = tex2Dlod(_Noise, float4(v.uv, 0, 0));
				o.wind = tex2Dlod(_Wind, float4(v.uv, 0, 0));
				o.normal = mul(v.normal, unity_WorldToObject);
                return o;
            }

			float rand(float2 seed) {
				return frac(sin(dot(seed, float2(12.85384, 78.232))) * 451353.53245);
			}

			float remap(float s, float a1, float a2, float b1, float b2)
			{
				return b1 + (s - a1)*(b2 - b1) / (a2 - a1);
			}

			[maxvertexcount(8)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {

				float4 v0 = IN[0].vertex;
				float4 v1 = IN[1].vertex;
				float4 v2 = IN[2].vertex;

				float3 n0 = IN[0].normal;
				float3 n1 = IN[1].normal;
				float3 n2 = IN[2].normal;

				/*float r1 = rand(iN[0].vertex.xz);
				float r2 = rand(iN[0].vertex.xz * float2(12.4234, 1.0123));*/

				//center will be the bottom of each grass blade
				float4 center = (v0 + v1 + v2) / 3;
				//basicly the up vector
				float4 normal = float4((n0 + n1 + n2) / 3, 0);
				//basicly the bottom vector that defines both the width and the orientation of the triangle/blade
				float4 tangent = (center - v0) * _Widht;

				float r1 = rand(IN[0].vertex.xz);
				float r2 = rand(IN[0].vertex.xz * float2(12.4234, 1.0123));

				float4 thickness = float4(normalize(cross(normal, tangent))*_Thickness, 0);

				float randomAngle = (IN[0].grassOrientation.r + IN[1].grassOrientation.r + IN[2].grassOrientation.r) / 3;


				float wind = (IN[0].wind.xy + IN[1].wind.xy + IN[2].wind.xy) * r1;

				float3 windNormal = (wind * 2 - 1) * remap(sin(_Time.w*_WindFrequency), -1, 1, _WindMin, _WindMax);


				float3 noiseNormal = (randomAngle * 2 - 1) * _DirectionIntensity; // Convert the noise sample to a vector and scale it with the intensity.


				normal = normalize(float4((normal + noiseNormal + windNormal).xyz, 0)) * _Height;




				g2f pIn;

				pIn.vertex = mul(UNITY_MATRIX_VP, center - tangent);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);

				//top vertex of the triangle, multiply the normal vector with a rotation matrix create with the crush texture map
				//also add a sideways vector and multiply it with a sin function in order to animate wind
				pIn.vertex = mul(UNITY_MATRIX_VP, center + normal);
				pIn.grassLeafColor = _LeafTopColor;
				triStream.Append(pIn);

				pIn.vertex = mul(UNITY_MATRIX_VP, center + thickness);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);

				//take advantage of triangle strip
				pIn.vertex = mul(UNITY_MATRIX_VP, center + tangent);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);
				triStream.RestartStrip();


				//backside triangles

				pIn.vertex = mul(UNITY_MATRIX_VP, center - tangent);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);

				pIn.vertex = mul(UNITY_MATRIX_VP, center - thickness);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);

				pIn.vertex = mul(UNITY_MATRIX_VP, center + normal);
				pIn.grassLeafColor = _LeafTopColor;
				triStream.Append(pIn);

				//take advantage of triangle strip
				pIn.vertex = mul(UNITY_MATRIX_VP, center + tangent);
				pIn.grassLeafColor = _LeafBottomColor;
				triStream.Append(pIn);
				triStream.RestartStrip();

			}



            fixed4 frag (g2f i) : SV_Target
            {
				fixed4 col = i.grassLeafColor;
				return col;
            }
            ENDCG
        }
    }
}
