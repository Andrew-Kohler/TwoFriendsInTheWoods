Shader "CustomRenderTexture/RenderTexIn"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
     }

     SubShader
     {
        //Blend One Zero

        Pass
        {
            Name "RenderTexIn"
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            #pragma shader_feature _RENDERING_CUTOUT
            #pragma shader_feature _SMOOTHNESS_ALBEDO
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            float4      _Color;
            sampler2D   _MainTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 color = tex2D(_MainTex, uv) * _Color;

                float alpha = tex2D(_MainTex, uv.xy).a;
                clip(alpha - 0.01);

                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(IN);
                // Set ShadowVal here
                if (shadow == 1) // we have achieved the absolute
                {
                    // If this fragment is fully lit, discard it
                    discard;
                }

                //fixed3 lighting = i.diff * shadow + i.ambient;
                //col.rgb *= lighting;

                return color;
            }
            ENDCG
        }
    }
}
