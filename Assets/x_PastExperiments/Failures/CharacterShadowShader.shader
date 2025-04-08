Shader "Example/CharacterShadowShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Light Texture", 2D) = "white" {}
        [NoScaleOffset] _MainTex2("Dark Texture", 2D) = "white" {}
        [NoScaleOffset] _ShadowVal("Shadow Val", Float) = 0
    }
        SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _RENDERING_CUTOUT
            #pragma shader_feature _SMOOTHNESS_ALBEDO
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

        // compile shader into multiple variants, with and without shadows
        // (we don't care about any lightmaps yet, so skip these variants)
        #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
        // shadow helper functions and macros
        #include "AutoLight.cginc"



        struct v2f
        {
            float2 uv : TEXCOORD0;
            SHADOW_COORDS(1) // put shadows data into TEXCOORD1
            fixed3 diff : COLOR0;
            fixed3 ambient : COLOR1;
            float4 pos : SV_POSITION;
        };
        v2f vert(appdata_base v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.diff = nl * _LightColor0.rgb;
            o.ambient = ShadeSH9(half4(worldNormal,1));
            // compute shadows data
            TRANSFER_SHADOW(o)
            return o;
        }

        sampler2D _MainTex;
        sampler2D _MainTex2;

        fixed4 frag(v2f i) : SV_Target
        {
            
        fixed4 col = tex2D(_MainTex, i.uv);

        // Alpha clipping without properly accounting for shadow casting
        float alpha = tex2D(_MainTex, i.uv.xy).a;
        clip(alpha - 0.5);
        

        // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
        fixed shadow = SHADOW_ATTENUATION(i);
        // Set ShadowVal here
        if (shadow < 1) // we have achieved the absolute
        {
            shadow = 1;
            // Set ShadowVal here
            col = tex2D(_MainTex2, i.uv);
        }
        // darken light's illumination with shadow, keep ambient intact
        fixed3 lighting = i.diff * shadow + i.ambient;
        col.rgb *= lighting;
        return col;
    }
    ENDCG
}

// shadow casting support
UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
