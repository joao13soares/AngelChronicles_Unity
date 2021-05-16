Shader "Unlit/Outliner"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _OutlineColor ("OutlineColor", Color) = (255,255,255,255)
        _OutlineWidth("OutlineWidth", Range(0,1)) = 0
         [Enum(UnityEngine.Rendering.ZTest)] _ZTest ("ZTest", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry+100"
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
        
        
       

        ZTest LEqual 
        ZWrite Off

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                return col;
            }
            ENDCG
        }


    }
}