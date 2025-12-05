Shader "Skybox/UltraboltGradient"
{
    Properties
    {
        _SkyGradientTop ("Sky Gradient Top", Color) = (0.02,0.06,0.15,1)
        _SkyGradientBottom ("Sky Gradient Bottom", Color) = (0.8,0.5,0.2,1)
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" }
        Cull Off
        ZWrite Off
        Fog { Mode Off }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _SkyGradientTop;
            fixed4 _SkyGradientBottom;

            struct v2f {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // view direction in world space (approx)
                o.dir = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // use normalized view direction's y to blend (range -1..1 -> 0..1)
                float3 viewDir = normalize(i.dir);
                float factor = saturate((viewDir.y + 1.0) * 0.5); // 0 => down, 1 => up
                // invert factor so top = up (1), bottom = down (0)
                // we want top color at top of sky (viewDir.y near 1)
                float t = factor;
                fixed4 col = lerp(_SkyGradientBottom, _SkyGradientTop, t);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
