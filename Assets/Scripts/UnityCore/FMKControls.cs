using UnityEngine;
using System;

public static class FMKControls
{
	public struct Resources
	{
		public Sprite standard;
		public Sprite background;
		public Sprite inputField;
		public Sprite knob;
		public Sprite checkmark;
		public Sprite dropdown;
		public Sprite mask;
	}

	private const float  kWidth       = 160f;
	private const float  kThickHeight = 30f;
	private const float  kThinHeight  = 20f;
	private static Vector2 s_ThickElementSize       = new Vector2(kWidth, kThickHeight);
	private static Vector2 s_ThinElementSize        = new Vector2(kWidth, kThinHeight);
	private static Vector2 s_ImageElementSize       = new Vector2(100f, 100f);
	private static Color   s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
	private static Color   s_PanelColor             = new Color(1f, 1f, 1f, 0.392f);
	private static Color   s_TextColor              = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

	private static GameObject CreateUIElementRoot(string name, Vector2 size)
	{
		GameObject child = new GameObject(name);
		RectTransform rectTransform = child.AddComponent<RectTransform>();
		rectTransform.sizeDelta = size;
		return child;
	}

	private static void SetDefaultTextValues(TranslatedUIText lbl)
	{
		// Set text values we want across UI elements in default controls.
		// Don't set values which are the same as the default values for the Text component,
		// since there's no point in that, and it's good to keep them as consistent as possible.
		lbl.color = s_TextColor;

		// Reset() is not called when playing. We still want the default font to be assigned
		lbl.AssignDefaultFont();
	}

	public static GameObject CreateText(Resources resources)
	{
		GameObject go = CreateUIElementRoot("TranslatedText", s_ThickElementSize);

		TranslatedUIText lbl = go.AddComponent<TranslatedUIText>();
		lbl.text = "New Text";
		SetDefaultTextValues(lbl);

		return go;
	}
}

