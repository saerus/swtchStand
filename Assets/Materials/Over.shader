Shader "Custom/z_BlendTex_Test" { Properties
 {
 //we're not using color in this shader
 //_Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGBA)", 2D) = "white" {}
 _DecalTex("Decal Texture (RGB)", 2D) = "white" {}
 _BlendAmount("Blend Amount", Range(0.0, 1.0)) = 1.0
 
 }
 SubShader
 {
 //rendertype opaque means we're not using a transparent shader
 Tags { "RenderType"="Opaque" }
 LOD 250
  
 CGPROGRAM
 #pragma surface surf Lambert
  
 sampler2D _MainTex;
 sampler2D _DecalTex;
 fixed _BlendAmount;
 //fixed4 _Color;
  
 struct Input
 {
 float2 uv_MainTex;
 float2 uv_DecalTex;
 };
  
 void surf (Input IN, inout SurfaceOutput o)
 {
 fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
 fixed4 cd = tex2D(_DecalTex, IN.uv_DecalTex);
 //switch the order of c and cd in the lerp function to change the cutout effect
 o.Albedo = lerp(cd, c.rgb, c.a * _BlendAmount);
 //we don't need to pass the alpha since we're not using transparency
 //o.Alpha = 1;
 }
 ENDCG
 }
 Fallback "Diffuse"
 }