using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaterialVectorFieldDrawer : MaterialPropertyDrawer
	{
		private readonly GUIContent[] labels;
		private float height;

		public MaterialVectorFieldDrawer(string x) : this(new string[] { x }) {}
		public MaterialVectorFieldDrawer(string x, string y) : this(new string[] { x, y }) {}
		public MaterialVectorFieldDrawer(string x, string y, string z) : this(new string[] { x, y, z }) {}
		public MaterialVectorFieldDrawer(string x, string y, string z, string w) : this(new string[] { x, y, z, w}) {}
		public MaterialVectorFieldDrawer(params string[] labels)
		{
			this.labels = new GUIContent[labels.Length];
			for (int i = 0; i < labels.Length; i++)
			{
				this.labels[i] = new GUIContent(labels[i]);
			}
		}
		
		public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
		{
			height = base.GetPropertyHeight( prop, label, editor );

			if( prop.type == MaterialProperty.PropType.Vector )
				return height * (this.labels.Length + 1);
			else
				return height;
		}

		public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
		{
			if( prop.type == MaterialProperty.PropType.Vector )
			{
				position = EditorGUI.IndentedRect(position);
				var v = prop.vectorValue;
				position.height = height;
				GUI.Label( position, label );

				EditorGUI.BeginChangeCheck();
				EditorGUI.indentLevel += 1;
				for( int i = 0; i < this.labels.Length; i++ )
				{
					position.y += height;
					v[i] = EditorGUI.FloatField( position, this.labels[i], v[i] );
				}
				EditorGUI.indentLevel -= 1;
				if( EditorGUI.EndChangeCheck() )
					prop.vectorValue = v;
			}
			else
				editor.DefaultShaderProperty( prop, label.text );
		}
	}
}
