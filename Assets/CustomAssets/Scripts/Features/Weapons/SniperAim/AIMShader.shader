Shader "Unlit/AIMShader"
{
    Properties
    {
        _AIMTex ("Render Texture", 2D) = "white" {}
        _AIMDot ("Dot", 2D) = "white" {}
        _AIMMask ("Mask", 2D) = "white" {}
		_Aperture ("Aperture", Range (0,180)) = 180
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "PreviewType"="Plane"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

			#define PI 3.1415926535897932384626433832795

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
			
			float _Aperture;
            sampler2D _AIMTex;
            sampler2D _AIMDot;
            sampler2D _AIMMask;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float aperture = radians(_Aperture);
				float apertureHalf = aperture * 0.5;
				float maxFactor = sin(apertureHalf);
				
				float2 xy = 2.0 * i.uv - 1.0;
				float d = length(xy * maxFactor);
				float z = sqrt(1.0 - d * d);
				float r = atan2(d, z) / PI;
				float phi = atan2(xy.y, xy.x);
    
				float2 resUV;
				resUV.x = r * cos(phi) + 0.5;
				resUV.y = r * sin(phi) + 0.5;

                fixed4 tex = tex2D(_AIMTex, resUV);
				fixed4 dot = tex2D(_AIMDot, i.uv);
				fixed4 mask = tex2D(_AIMMask, i.uv);
				float dist = length(i.uv - float2(0.5, 0.5));
				float vignette = 1 - clamp(dist / 0.5, 0, 1);
				fixed4 col;
				col.rgb = ((dot.rgb * dot.a) + (tex.rgb * (1 - dot.a))) * vignette;
				col.a = mask.r;
                return col;
            }
            ENDCG
        }
    }
}
