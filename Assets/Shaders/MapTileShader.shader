Shader "Custom/MapTileShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _ValidOverlayTex ("Valid Overlay Texture", 2D) = "white" {}
        _InvalidOverlayTex ("Invalid Overlay Texture", 2D) = "white" {}
        _Blend ("Blend Amount", Range(0,1)) = 0.0
        _Valid ("Valid", Float) = 0
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
            sampler2D _ValidOverlayTex;
            sampler2D _InvalidOverlayTex;

            float _Blend;
            float _Valid;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol   = tex2D(_MainTex, i.uv);
                fixed4 validCol  = tex2D(_ValidOverlayTex, i.uv);
                fixed4 invalidCol= tex2D(_InvalidOverlayTex, i.uv);

                float validMask = step(0.5, _Valid);
                fixed4 overlayCol = lerp(invalidCol, validCol, validMask);

                fixed blendFactor = overlayCol.a * _Blend;
                fixed3 finalRGB = lerp(baseCol.rgb, overlayCol.rgb, blendFactor);

                return fixed4(finalRGB, baseCol.a);
            }

            ENDCG
        }
    }
}