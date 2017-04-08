using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FullmetalKobzar.Core.Translation;

[AddComponentMenu("FullmetalKobzar/TranslatedText", 10)]
public class TranslatedUIText : Text
{

	[Inject]
	private ITranslator translator;

	[SerializeField]
	private string resource = "ui";

	private string translatedText = "";

	private string GetTranslatedText () 
	{
		if (this.translator == null)
			return m_Text;
		string tmpTranslatedText = this.translator.Translate (m_Text, this.resource);
		if ((this.translatedText != "") && (this.translatedText != tmpTranslatedText)) {
			SetLayoutDirty ();
		}
		this.translatedText = tmpTranslatedText;
		return this.translatedText;
	}

	public override string text
	{
		get
		{
			return this.GetTranslatedText ();
		}
		set
		{
			if (String.IsNullOrEmpty(value))
			{
				if (String.IsNullOrEmpty(m_Text))
					return;
				m_Text = "";
				SetVerticesDirty();
			}
			else if (m_Text != value)
			{
				m_Text = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public override float preferredWidth
	{
		get
		{
			var settings = GetGenerationSettings(Vector2.zero);
			return cachedTextGeneratorForLayout.GetPreferredWidth(this.GetTranslatedText (), settings) / pixelsPerUnit;
		}
	}

	public override float preferredHeight
	{
		get
		{
			var settings = GetGenerationSettings(new Vector2(rectTransform.rect.size.x, 0.0f));
			return cachedTextGeneratorForLayout.GetPreferredHeight(this.GetTranslatedText (), settings) / pixelsPerUnit;
		}
	}

	#if UNITY_EDITOR
	protected override void Reset()
	{
		AssignDefaultFont();
	}

	#endif
	internal void AssignDefaultFont()
	{
		font = Resources.GetBuiltinResource<Font>("Arial.ttf");
	}

}
