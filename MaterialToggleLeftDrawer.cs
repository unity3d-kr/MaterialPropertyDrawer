using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaterialToggleLeftDrawer : MaterialPropertyDrawer
	{
		protected readonly string keyword;

		public MaterialToggleLeftDrawer()
		{
		}

		public MaterialToggleLeftDrawer(string keyword)
		{
			this.keyword = keyword;
		}

		private static bool IsPropertyTypeSuitable(MaterialProperty prop)
		{
			return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range;
		}

		protected virtual void SetKeyword(MaterialProperty prop, bool on)
		{
			this.SetKeywordInternal(prop, on, "_ON");
		}

		protected void SetKeywordInternal(MaterialProperty prop, bool on, string defaultKeywordSuffix)
		{
			string text = (!string.IsNullOrEmpty(this.keyword)) ? this.keyword : (prop.name.ToUpperInvariant() + defaultKeywordSuffix);
			UnityEngine.Object[] targets = prop.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				Material material = (Material)targets[i];
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

		public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
		{
			float result;
			if (!MaterialToggleLeftDrawer.IsPropertyTypeSuitable(prop))
			{
				result = 40f;
			}
			else
			{
				result = base.GetPropertyHeight(prop, label, editor);
			}
			return result;
		}

		public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
		{
			if (!MaterialToggleLeftDrawer.IsPropertyTypeSuitable(prop))
			{
				EditorGUI.HelpBox(position, "Toggle used on a non-float property: " + prop.name, MessageType.Error);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				bool flag = Math.Abs(prop.floatValue) > 0.001f;
				EditorGUI.showMixedValue = prop.hasMixedValue;
				flag = EditorGUI.ToggleLeft(position, label, flag);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					prop.floatValue = ((!flag) ? 0f : 1f);
					this.SetKeyword(prop, flag);
				}
			}
		}

		public override void Apply(MaterialProperty prop)
		{
			base.Apply(prop);
			if (MaterialToggleLeftDrawer.IsPropertyTypeSuitable(prop))
			{
				if (!prop.hasMixedValue)
				{
					this.SetKeyword(prop, Math.Abs(prop.floatValue) > 0.001f);
				}
			}
		}
	}
} // namespace UnityEditor
