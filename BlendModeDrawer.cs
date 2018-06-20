using System;

using UnityEditor.Animations;

using UnityEngine;

namespace UnityEditor
{
	internal class BlendModeDrawer : MaterialPropertyDrawer
	{
		const string _ALPHATEST_ON = "_ALPHATEST_ON";
		const string _ALPHABLEND_ON = "_ALPHABLEND_ON";
		const string _ALPHAPREMULTIPLY_ON = "_ALPHAPREMULTIPLY_ON";

		const string RenderType = "RenderType";
		const string Transparent = "Transparent";
		const string TransparentCutout = "TransparentCutout";

		string srcblend;
		string dstblend;
		string zwrite;
		string cull;

		public BlendModeDrawer()
		{
			this.srcblend = "_SrcBlend";
			this.dstblend = "_DstBlend";
			this.zwrite = "_ZWrite";
			this.cull = "_Cull";
		}
		
		public BlendModeDrawer(string srcblend, string dstblend, string zwrite = "_ZWrite", string cull = "_Cull")
		{
			this.srcblend = srcblend;
			this.dstblend = dstblend;
			this.zwrite = zwrite;
			this.cull = cull;
		}

		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,       // Old school alpha-blending mode, fresnel does not affect amount of transparency
			Transparent, // Physically plausible transparency mode, implemented as alpha pre-multiply
		}
		
		private bool IsDefaultRenderQueue( int renderQueue )
		{
			return renderQueue == -1
					|| renderQueue == (int) UnityEngine.Rendering.RenderQueue.Geometry
					|| renderQueue == (int) UnityEngine.Rendering.RenderQueue.AlphaTest
					|| renderQueue == (int) UnityEngine.Rendering.RenderQueue.Transparent;
		}
		
		public void SetupMaterialWithBlendMode( Material material, BlendMode blend )
		{
			switch( blend )
			{
			case BlendMode.Opaque:
				material.SetOverrideTag( RenderType, string.Empty );
				material.SetInt( this.srcblend, (int) UnityEngine.Rendering.BlendMode.One );
				material.SetInt( this.dstblend, (int) UnityEngine.Rendering.BlendMode.Zero );
				material.SetInt( this.zwrite, 1 );
				material.SetInt( this.cull, (int) UnityEngine.Rendering.CullMode.Back );
				material.DisableKeyword( _ALPHATEST_ON );
				material.DisableKeyword( _ALPHABLEND_ON );
				material.DisableKeyword( _ALPHAPREMULTIPLY_ON );
				if( IsDefaultRenderQueue( material.renderQueue ) )
					material.renderQueue = -1; // 'From Shader'
				break;
			case BlendMode.Cutout:
				material.SetOverrideTag( RenderType, TransparentCutout );
				material.SetInt( this.srcblend, (int) UnityEngine.Rendering.BlendMode.One );
				material.SetInt( this.dstblend, (int) UnityEngine.Rendering.BlendMode.Zero );
				material.SetInt( this.zwrite, 1 );
				material.SetInt( this.cull, (int) UnityEngine.Rendering.CullMode.Back );
				material.EnableKeyword( _ALPHATEST_ON );
				material.DisableKeyword( _ALPHABLEND_ON );
				material.DisableKeyword( _ALPHAPREMULTIPLY_ON );
				if( IsDefaultRenderQueue( material.renderQueue ) )
					material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.AlphaTest;
				break;
			case BlendMode.Fade: //AlphaBlend
				material.SetOverrideTag( RenderType, Transparent );
				material.SetInt( this.srcblend, (int) UnityEngine.Rendering.BlendMode.SrcAlpha );
				material.SetInt( this.dstblend, (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha );
				material.SetInt( this.zwrite, 0 );
				material.SetInt( this.cull, (int) UnityEngine.Rendering.CullMode.Off );
				material.DisableKeyword( _ALPHATEST_ON );
				material.EnableKeyword( _ALPHABLEND_ON );
				material.DisableKeyword( _ALPHAPREMULTIPLY_ON );
				if( IsDefaultRenderQueue( material.renderQueue ) )
					material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
				break;
			case BlendMode.Transparent:
				material.SetOverrideTag( RenderType, Transparent );
				material.SetInt( this.srcblend, (int) UnityEngine.Rendering.BlendMode.One );
				material.SetInt( this.dstblend, (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha );
				material.SetInt( this.zwrite, 0 );
				material.SetInt( this.cull, (int) UnityEngine.Rendering.CullMode.Off );
				material.DisableKeyword( _ALPHATEST_ON );
				material.DisableKeyword( _ALPHABLEND_ON );
				material.EnableKeyword( _ALPHAPREMULTIPLY_ON );
				if( IsDefaultRenderQueue( material.renderQueue ) )
					material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
				break;
			}
		}

		// Draw the property inside the given rect
		public override void OnGUI( Rect position, MaterialProperty prop, String label, MaterialEditor editor )
		{
			EditorGUI.showMixedValue = prop.hasMixedValue;
			var mode = (BlendMode) prop.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (BlendMode)EditorGUI.EnumPopup( position, label, mode );
			if( EditorGUI.EndChangeCheck() )
			{
				editor.RegisterPropertyChangeUndo( prop.displayName );
				prop.floatValue = (float) mode;

				UnityEngine.Object[] targets = prop.targets;
				for( int i = 0; i < targets.Length; i++ )
				{
					Material material = (Material) targets[i];
					SetupMaterialWithBlendMode( material, mode );
				}
			}
			EditorGUI.showMixedValue = false;
		}
	}
}