// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "3DM/meshterrain"
{
	Properties
	{
		_basecolor("basecolor", 2D) = "white" {}
		_baseNormal("baseNormal", 2D) = "bump" {}
		_normalstrength("normalstrength", Range( 1 , 2)) = 0
		_splatmap("splatmap", 2D) = "white" {}
		_greencolor("greencolor", 2D) = "white" {}
		_greenNormal("greenNormal", 2D) = "bump" {}
		_greenspecular("greenspecular", 2D) = "white" {}
		_redcolor("redcolor", 2D) = "white" {}
		_redNormal("redNormal", 2D) = "bump" {}
		_redspecular("redspecular", 2D) = "white" {}
		_blueColor("blueColor", 2D) = "white" {}
		_blueNormal("blueNormal", 2D) = "bump" {}
		_bluespecular("bluespecular", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _normalstrength;
		uniform sampler2D _redNormal;
		uniform float4 _redNormal_ST;
		uniform sampler2D _greenNormal;
		uniform float4 _greenNormal_ST;
		uniform sampler2D _splatmap;
		uniform float4 _splatmap_ST;
		uniform sampler2D _blueNormal;
		uniform float4 _blueNormal_ST;
		uniform sampler2D _baseNormal;
		uniform float4 _baseNormal_ST;
		uniform sampler2D _basecolor;
		uniform float4 _basecolor_ST;
		uniform sampler2D _redcolor;
		uniform float4 _redcolor_ST;
		uniform sampler2D _greencolor;
		uniform float4 _greencolor_ST;
		uniform sampler2D _blueColor;
		uniform float4 _blueColor_ST;
		uniform sampler2D _redspecular;
		uniform float4 _redspecular_ST;
		uniform sampler2D _greenspecular;
		uniform float4 _greenspecular_ST;
		uniform sampler2D _bluespecular;
		uniform float4 _bluespecular_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_redNormal = i.uv_texcoord * _redNormal_ST.xy + _redNormal_ST.zw;
			float2 uv_greenNormal = i.uv_texcoord * _greenNormal_ST.xy + _greenNormal_ST.zw;
			float2 uv_splatmap = i.uv_texcoord * _splatmap_ST.xy + _splatmap_ST.zw;
			float4 tex2DNode27 = tex2D( _splatmap, uv_splatmap );
			float3 lerpResult10 = lerp( UnpackScaleNormal( tex2D( _redNormal, uv_redNormal ), _normalstrength ) , UnpackScaleNormal( tex2D( _greenNormal, uv_greenNormal ), _normalstrength ) , tex2DNode27.g);
			float2 uv_blueNormal = i.uv_texcoord * _blueNormal_ST.xy + _blueNormal_ST.zw;
			float3 lerpResult14 = lerp( lerpResult10 , UnpackScaleNormal( tex2D( _blueNormal, uv_blueNormal ), _normalstrength ) , tex2DNode27.b);
			float2 uv_baseNormal = i.uv_texcoord * _baseNormal_ST.xy + _baseNormal_ST.zw;
			o.Normal = BlendNormals( lerpResult14 , UnpackScaleNormal( tex2D( _baseNormal, uv_baseNormal ), _normalstrength ) );
			float2 uv_basecolor = i.uv_texcoord * _basecolor_ST.xy + _basecolor_ST.zw;
			float2 uv_redcolor = i.uv_texcoord * _redcolor_ST.xy + _redcolor_ST.zw;
			float2 uv_greencolor = i.uv_texcoord * _greencolor_ST.xy + _greencolor_ST.zw;
			float4 lerpResult5 = lerp( tex2D( _redcolor, uv_redcolor ) , tex2D( _greencolor, uv_greencolor ) , tex2DNode27.g);
			float2 uv_blueColor = i.uv_texcoord * _blueColor_ST.xy + _blueColor_ST.zw;
			float4 lerpResult13 = lerp( lerpResult5 , tex2D( _blueColor, uv_blueColor ) , tex2DNode27.b);
			float4 blendOpSrc39 = tex2D( _basecolor, uv_basecolor );
			float4 blendOpDest39 = lerpResult13;
			o.Albedo = ( saturate( (( blendOpDest39 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest39 - 0.5 ) ) * ( 1.0 - blendOpSrc39 ) ) : ( 2.0 * blendOpDest39 * blendOpSrc39 ) ) )).rgb;
			float2 uv_redspecular = i.uv_texcoord * _redspecular_ST.xy + _redspecular_ST.zw;
			float2 uv_greenspecular = i.uv_texcoord * _greenspecular_ST.xy + _greenspecular_ST.zw;
			float lerpResult25 = lerp( tex2D( _redspecular, uv_redspecular ).a , tex2D( _greenspecular, uv_greenspecular ).a , tex2DNode27.g);
			float2 uv_bluespecular = i.uv_texcoord * _bluespecular_ST.xy + _bluespecular_ST.zw;
			float lerpResult26 = lerp( lerpResult25 , tex2D( _bluespecular, uv_bluespecular ).a , tex2DNode27.g);
			o.Smoothness = ( 1.0 - lerpResult26 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
315;180;2682;1191;1450.195;-70.00742;1.3;False;True
Node;AmplifyShaderEditor.RangedFloatNode;41;-1536.771,786.568;Float;False;Property;_normalstrength;normalstrength;2;0;Create;True;0;0;False;0;0;1.116;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-808.8755,1130.637;Float;True;Property;_greenspecular;greenspecular;6;0;Create;True;0;0;False;0;None;109e72286ce2a134ab375b37682dc118;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-525.5496,214.2428;Float;True;Property;_splatmap;splatmap;3;0;Create;True;0;0;False;0;None;4909c99986dfb0148bc961cf7bd986e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-520.7751,1325.136;Float;True;Property;_redspecular;redspecular;9;0;Create;True;0;0;False;0;None;2f966a8dcff487542b57a10b970bcc14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-1084.545,694.838;Float;True;Property;_greenNormal;greenNormal;5;0;Create;True;0;0;False;0;None;f4c144e58dd913d4ca6315717b77f431;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-767.5449,779.838;Float;True;Property;_redNormal;redNormal;8;0;Create;True;0;0;False;0;None;5fef53b85295bef48b1df944e4a15678;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-782.5449,534.838;Float;True;Property;_redcolor;redcolor;7;0;Create;True;0;0;False;0;None;1f502d03b3380454185f4bd4924c3140;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-1111.545,447.838;Float;True;Property;_greencolor;greencolor;4;0;Create;True;0;0;False;0;None;dbaf52ac83c320e40b8a263573409cbd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-456.545,604.838;Float;True;Property;_blueColor;blueColor;10;0;Create;True;0;0;False;0;None;9a8fc414cea627d4d87766a0646e99a9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;10;-118.5449,571.838;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;5;-119.5449,424.838;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;25;8.043198,1155.639;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-32.67535,1287.337;Float;True;Property;_bluespecular;bluespecular;12;0;Create;True;0;0;False;0;None;0b6aad57e4aec864994c0dbc6647ab59;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-435.545,833.838;Float;True;Property;_blueNormal;blueNormal;11;0;Create;True;0;0;False;0;None;9fa7980987afc6f4da9ee099fae9bfc7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;26;457.78,1109.009;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;57.229,211.5682;Float;True;Property;_baseNormal;baseNormal;1;0;Create;True;0;0;False;0;None;48900847198e9ae408b86d3242e4a2c3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;13;233.9887,624.3633;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;28;347.229,98.56818;Float;True;Property;_basecolor;basecolor;0;0;Create;True;0;0;False;0;None;1f555b864dac80b48b2522aca776b5de;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;222.905,773.5502;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;43;1121.229,499.0682;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;31;467.229,903.5682;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendOpsNode;39;713.229,614.5682;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1302.688,714.7265;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;3DM/meshterrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;5;41;0
WireConnection;9;5;41;0
WireConnection;10;0;9;0
WireConnection;10;1;7;0
WireConnection;10;2;27;2
WireConnection;5;0;8;0
WireConnection;5;1;6;0
WireConnection;5;2;27;2
WireConnection;25;0;20;4
WireConnection;25;1;22;4
WireConnection;25;2;27;2
WireConnection;12;5;41;0
WireConnection;26;0;25;0
WireConnection;26;1;21;4
WireConnection;26;2;27;2
WireConnection;30;5;41;0
WireConnection;13;0;5;0
WireConnection;13;1;11;0
WireConnection;13;2;27;3
WireConnection;14;0;10;0
WireConnection;14;1;12;0
WireConnection;14;2;27;3
WireConnection;43;0;26;0
WireConnection;31;0;14;0
WireConnection;31;1;30;0
WireConnection;39;0;28;0
WireConnection;39;1;13;0
WireConnection;0;0;39;0
WireConnection;0;1;31;0
WireConnection;0;4;43;0
ASEEND*/
//CHKSM=AEBDCE5BC4AA36E50E90789905EC3B2243C75B42