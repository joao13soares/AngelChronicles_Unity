Shader "Unlit/MyGrass"
{
    Properties
    {
		_Height("Height",Range(0,10)) = 2 //Grass Height
		_Widht("Widht",Range(0,10)) = 2 //GrassWidht
		_GrassBottomColor("GrassLeaves Bottom Color", Color) = (0, .5, 0, 1) //Grass Bottom Color
		_GrassTopColor("GrassLeaves Top Color", Color) = (0, 1, 0, 1) //Grass Top Color
		_Thickness("Thickness",Range(0,5)) = 1 //Grass Thickness

		_Wind("Wind",2D) = "white" {}//Wind Map
		_WindSpeed("WindSpeed",Range(0,10)) = 1// Wind Speed
		_WindMin("WindMin",Range(0,10)) = 0 //Minimum wind
		_WindMax("WindMax",Range(0,10)) = 1//Maximum wind

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
				float3 normal : NORMAL;
				float4 wind : TEXCOORD1;
			};

            struct g2f
            {
				float4 grassLeafColor : COLOR;
                float4 vertex : SV_POSITION;
            };

			float _Height;
			float _Widht;
			float4 _GrassBottomColor;
			float4 _GrassTopColor;
			float _Thickness;

			sampler2D _Wind;
			float4 _Wind_ST;
			float _WindSpeed;
			float _WindMin;
			float _WindMax;

			v2g  vert (appdata v)
            {
				v2g o;
				o.vertex = mul(unity_ObjectToWorld, v.vertex);
				o.wind = tex2Dlod(_Wind, float4(v.uv, 0, 0));//2D texture lookup with specified level of detail and optional texel offset.
				o.normal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
                return o;
            }

			//function to gain random values
			float rand(float2 seed) {
				return frac(sin(dot(seed, float2(12.85384, 78.232))) * 451353.53245);
			}

			
			//Since we want the grass to have thickness we need 8 vertex (4 tris for the front nd 4 tris for the back)
			[maxvertexcount(8)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream) 
			{		
				float4 v0 = i[0].vertex;//Vertice 1
				float4 v1 = i[1].vertex;//Vertice 2
				float4 v2 = i[2].vertex;//Vertice 3

				float3 n0 = i[0].normal;//Normal 1
				float3 n1 = i[1].normal;//Normal 2
				float3 n2 = i[2].normal;//Normal 3

				//center will be the bottom of each grass blade
				float4 center = (v0 + v1 + v2) / 3;
				//up vector
				float4 normal = float4((n0 + n1 + n2) / 3, 0);
				//Side Vector
				float4 tangent = (center - v0)  * _Widht;


				float r1 = rand(i[0].vertex.xz);//Random 1
				float r2 = rand(i[0].vertex.xz * float2(12.4234, 1.0123));//Random 2

				//A vector perpendicular to the tangent and the normal to give the blades thickness
				float4 thickness = float4(normalize(cross(normal, tangent))*_Thickness, 0);
					
				//distortion texture that increase variability between each blade
				float wind = (i[0].wind.xy + i[1].wind.xy + i[2].wind.xy) * r2 ;

				//convert our wind to a vector in the [_WindMin,_WindMax] range.				
				float4 windNormal = wind * lerp(_WindMin, _WindMax, abs(sin(_Time.y)));

				//Our final normal
				normal = (normal * _Height * r1) + tangent * sin((center.x + center.z + windNormal + _Time.y) * _WindSpeed);


				g2f o;

				//side vertices are calculated by subtracting(for one side) the tangent to the center
				o.vertex = mul(UNITY_MATRIX_VP, center - tangent);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				//side vertices are calculated by the tangent plus the center (for the oposite side)
				o.vertex = mul(UNITY_MATRIX_VP, center + tangent);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				//the up vector is the center plus the normal 
				o.vertex = mul(UNITY_MATRIX_VP, center + normal);
				o.grassLeafColor = _GrassTopColor;
				triStream.Append(o);

				o.vertex = mul(UNITY_MATRIX_VP, center + thickness);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				o.vertex = mul(UNITY_MATRIX_VP, center - tangent);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				o.vertex = mul(UNITY_MATRIX_VP, center + tangent);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				o.vertex = mul(UNITY_MATRIX_VP, center - thickness);
				o.grassLeafColor = _GrassBottomColor;
				triStream.Append(o);

				o.vertex = mul(UNITY_MATRIX_VP, center + normal);
				o.grassLeafColor = _GrassTopColor;
				triStream.Append(o);
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
