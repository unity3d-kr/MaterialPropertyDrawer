using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaterialAlphaRangeDrawer : MaterialPropertyDrawer
	{
		private readonly GUIContent alphaLabel;
		private readonly float minAlpha, maxAlpha;
		private float height;

		public MaterialAlphaRangeDrawer() : this(null) {}
		public MaterialAlphaRangeDrawer(string a) : this(a, 1) {}
		public MaterialAlphaRangeDrawer(string a, float maxA) : this(a, 0, maxA) {}

		public MaterialAlphaRangeDrawer(string a, float minA, float maxA)
		{
			alphaLabel = new GUIContent( a );
			minAlpha = minA;
			maxAlpha = maxA;
		}
			
		
		public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
		{
			height = base.GetPropertyHeight( prop, label, editor );

			if( prop.type == MaterialProperty.PropType.Color )
				return height * 2 + 1;
			else
				return height;
		}

		public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
		{
			if( prop.type == MaterialProperty.PropType.Color )
			{
				position = EditorGUI.IndentedRect(position);
				var v = prop.colorValue;
				position.height = height;
				GUI.Label( position, label );

				EditorGUI.BeginChangeCheck();
				v = EditorGUI.ColorField( position, label, v, true, false, false, null);

				position.y += height;
				v.a = EditorGUI.Slider( position, alphaLabel, v.a, minAlpha, maxAlpha );

				if( EditorGUI.EndChangeCheck() )
					prop.colorValue = v;
			}
			else
				editor.DefaultShaderProperty( prop, label.text );
		}
	}
}
