Shader "Capsule/Circle"
{
  Properties
  {
    _Color ("Color", Color) = (0,0,0,1)
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend One OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DUMMY
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float trunc(float x)
      {
          return (sign(x) * floor(abs(x)));
      }
      
      float2 trunc(float2 x)
      {
          return (sign(x) * floor(abs(x)));
      }
      
      float3 trunc(float3 x)
      {
          return (sign(x) * floor(abs(x)));
      }
      
      float4 trunc(float4 x)
      {
          return (sign(x) * floor(abs(x)));
      }
      
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xy = ((-in_f.texcoord.xy) + float2(0.5, 0.5));
          u_xlat0_d.x = length(u_xlat0_d.xy);
          u_xlat0_d.x = ((-u_xlat0_d.x) + 1);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat0_d.x);
          u_xlat0_d.w = trunc(u_xlat0_d.x);
          u_xlat0_d.xyz = (u_xlat0_d.www * _Color.xyz);
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
