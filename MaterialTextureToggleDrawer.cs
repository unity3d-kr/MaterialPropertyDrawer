using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaterialTextureToggleDrawer : MaterialPropertyDrawer
	{
		protected readonly string keyword;

		public MaterialTextureToggleDrawer()
		{
		}

		public MaterialTextureToggleDrawer(string keyword)
		{
			this.keyword = keyword;
		}

		private static bool IsPropertyTypeSuitable(MaterialProperty prop)
		{
			return prop.type == MaterialProperty.PropType.Texture;
		}

		protected virtual void SetKeyword(MaterialProperty prop, Texture target)
		{
			this.SetKeywordInternal(prop, target, "_ON");
		}

		protected void SetKeywordInternal(MaterialProperty prop, Texture target, string defaultKeywordSuffix)
		{
			string text = (!string.IsNullOrEmpty(this.keyword)) ? this.keyword : (prop.name.ToUpperInvariant() + defaultKeywordSuffix);
			UnityEngine.Object[] targets = prop.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				Material material = (Material)targets[i];
				bool on = material.GetTexture( target.name );
				if (on)
				{
					material.EnableKeyword(text);
				}
				else
				{
					material.DisableKeyword(text);
				}
			}
		}

		public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
		{
			if (!MaterialTextureToggleDrawer.IsPropertyTypeSuitable(prop))
			{
				GUIContent label2 = new GUIContent("Toggle used on a non-texture property: " + prop.name);
				EditorGUI.LabelField(position, label2, EditorStyles.helpBox);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				prop.textureValue = editor.TextureProperty(position, prop, label.text);
				if (EditorGUI.EndChangeCheck())
				{
					this.SetKeyword(prop, prop.textureValue);
				}
			}
		}

		public override void Apply(MaterialProperty prop)
		{
			base.Apply(prop);
			if (MaterialTextureToggleDrawer.IsPropertyTypeSuitable(prop))
			{
				this.SetKeyword(prop, prop.textureValue);
			}
		}
	}
}
