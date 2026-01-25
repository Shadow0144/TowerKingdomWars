Shader "Custom/MapTileShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _OverlayTex ("Overlay Texture", 2D) = "white" {}
        _Blend ("Blend Amount", Range(0,1)) = 0.0
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

            sampler2D _MainTex;
            sampler2D _OverlayTex;
            float _Blend;
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 overlayCol = tex2D(_OverlayTex, i.uv);

                // Multiply overlay RGB by its alpha
                fixed overlayAlpha = overlayCol.a * _Blend;
                fixed3 blendedRGB = lerp(baseCol.rgb, overlayCol.rgb, overlayAlpha);

                return fixed4(blendedRGB, baseCol.a); // keep base alpha
            }
            ENDCG
        }
    }
}
