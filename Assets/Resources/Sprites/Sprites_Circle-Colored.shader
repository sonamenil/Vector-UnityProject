Shader "Sprites/Circle-Colored"
{
  Properties
  {
    _Color ("Color", Color) = (0,0,0,1)
    _BackgoundColor ("BackgoundColor", Color) = (0,0,0,1)
    _Radius ("Radius", float) = 1
    _Border ("Border", float) = 0
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
      uniform float4 _BackgoundColor;
      uniform float _Radius;
      uniform float _Border;
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
      int u_xlatb0;
      float u_xlat1_d;
      int u_xlatb1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xy = ((-in_f.texcoord.xy) + float2(0.5, 0.5));
          u_xlat0_d.x = length(u_xlat0_d.xy);
          u_xlatb1 = (0.5<u_xlat0_d.x);
          if(u_xlatb1)
          {
              if((-1!=0))
              {
                  discard;
              }
          }
          else
          {
              u_xlat1_d = (_Border / _Radius);
              u_xlat1_d = ((-u_xlat1_d) + 0.5);
              u_xlatb0 = (u_xlat1_d<u_xlat0_d.x);
              if(u_xlatb0)
              {
                  u_xlat0_d.xyz = (_Color.www * _Color.xyz);
                  u_xlat0_d.w = _Color.w;
                  out_f.color = u_xlat0_d;
                  return out_f;
              }
          }
          u_xlat0_d.xyz = (_BackgoundColor.www * _BackgoundColor.xyz);
          u_xlat0_d.w = _BackgoundColor.w;
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
