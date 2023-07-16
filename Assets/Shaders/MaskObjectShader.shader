Shader "Custom/MaskObjectShader"    
{
 Properties
 {
     [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
     _Color ("Tint", Color) = (1,1,1,1)
     [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
 }

 SubShader
 {
     Tags
     {
         "Queue"="AlphaTest"
         "IgnoreProjector"="True"
         "RenderType"="Transparent"
         "PreviewType"="Plane"
         "CanUseSpriteAtlas"="True"
     } 

     Cull Off
     Lighting Off
     ZWrite Off
     Fog { Mode Off }
     Blend One OneMinusSrcAlpha

     Pass
     {
         Tags {"LightMode" = "ForwardBase"}
         Stencil
         {
             Ref 1
             Comp equal
         }

     CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"
         #include "Lighting.cginc"
         #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight DUMMY PIXELSNAP_ON
         #include "AutoLight.cginc"

         struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };
            v2f vert (appdata_base v)
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
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                if (col.a<0.1) discard;
                col.rgb *= col.a;
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
     ENDCG
     }
 }
}