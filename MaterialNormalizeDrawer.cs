using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaterialNormalizeDrawer : MaterialPropertyDrawer
	{
		int count = 3;

		public MaterialNormalizeDrawer()
		{}

		public MaterialNormalizeDrawer( int count )
		{
			this.count = count;
		}
		
		public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
		{
			float height = base.GetPropertyHeight( prop, label, editor );

			if( prop.type == MaterialProperty.PropType.Vector )
				return height * 2;
			else
				return height;
		}

		private Vector4 Round( Vector4 v, int digits )
		{
			float d = Mathf.Pow( 10, digits );
			float rcp_d = 1f / d;
			v.x = Mathf.Round( v.x * d ) * rcp_d;
			v.y = Mathf.Round( v.y * d ) * rcp_d;
			v.z = Mathf.Round( v.z * d ) * rcp_d;
			v.w = Mathf.Round( v.w * d ) * rcp_d;
			return v;
		}

		public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
		{
			if( prop.type == MaterialProperty.PropType.Vector )
			{
				position = EditorGUI.IndentedRect( position );
				var v = prop.vectorValue;

				EditorGUI.BeginChangeCheck();
				switch( count )
				{
				case 2:
					v = EditorGUI.Vector2Field( position, label, v );
					break;
				case 3:
					v = EditorGUI.Vector3Field( position, label, v );
					break;
				default:
					v = EditorGUI.Vector4Field( position, label, v );
					break;
				}
				if( EditorGUI.EndChangeCheck() )
				{
					v = Round( v.normalized, 5 );
					prop.vectorValue = v;
				}
			}
			else
				editor.DefaultShaderProperty( prop, label.text );
		}
	}
}
