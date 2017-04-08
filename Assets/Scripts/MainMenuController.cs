using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using FullmetalKobzar.Core.Translation;
using Zenject;

public class MainMenuController : MonoBehaviour {

	public GameObject mainMenuPrefab;
	public GameObject settingsMenuPrefab;

	public Canvas canvas;

	[Inject]
	private DiContainer container;

	[Inject]
	private ITranslator translator;

	[Inject]
	private ITranslatorFactory translatorFactory;

	private string currentMenu = "main";
	private string previousMenu = "";
	private GameObject currentMenuObject;

	private string[] locales = { "ruRu", "enUs" };

	void Start () {
		this.currentMenuObject = null;
		// Instantiate main menu
		this.InstantiateCurrentMenu ();
	}

	private void InstantiateCurrentMenu () {
		if (this.currentMenuObject)
			Destroy (this.currentMenuObject);

		if (this.currentMenu == "main") {
			this.currentMenuObject = this.container.InstantiatePrefab (this.mainMenuPrefab);
			this.currentMenuObject.transform.SetParent (this.canvas.transform, false);
			this.currentMenuObject.GetComponent<RectTransform> ().localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			this.currentMenuObject.transform.localScale = new Vector3 (1, 1, 1);
			foreach (Button button in this.currentMenuObject.GetComponentsInChildren<Button> ()) {
				if (button.name == "PlayButton") {
					button.onClick.AddListener (StartGame);
				} else if (button.name == "SettingsButton") {
					button.onClick.AddListener (SettingsMenu);
				} else if (button.name == "ExitButton") {
					button.onClick.AddListener (ExitGame);
				}
			}

			return;
		}

		if (this.currentMenu == "settings") {
			this.currentMenuObject = this.container.InstantiatePrefab (this.settingsMenuPrefab);
			this.currentMenuObject.transform.SetParent (this.canvas.transform, false);
			this.currentMenuObject.GetComponent<RectTransform> ().localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
			this.currentMenuObject.transform.localScale = new Vector3 (1, 1, 1);
			foreach (Button button in this.currentMenuObject.GetComponentsInChildren<Button> ()) {
				if (button.name == "SaveButton") {
					button.onClick.AddListener (SaveSettings);
				} else if (button.name == "BackButton") {
					button.onClick.AddListener (Back);
				}
			}
			foreach (Dropdown dropdown in this.currentMenuObject.GetComponentsInChildren<Dropdown> ()) {
				if (dropdown.name == "LanguageDropdown") {
					var localeIndex = Array.IndexOf (this.locales, this.translator.GetCurrentLocale ());
					dropdown.value = localeIndex;
				}
			}

			return;
		}
	}

	public void SettingsMenu () {
		this.previousMenu = this.currentMenu;
		this.currentMenu = "settings";

		this.InstantiateCurrentMenu ();
	}

	public void Back () {
		this.currentMenu = this.previousMenu;
		this.previousMenu = "";

		this.InstantiateCurrentMenu ();
	}

	public void SaveSettings () {
		foreach (Dropdown dropdown in this.currentMenuObject.GetComponentsInChildren<Dropdown> ()) {
			if (dropdown.name == "LanguageDropdown") {
				var locale = this.locales [dropdown.value];
				PlayerPrefs.SetString ("locale", locale);
				PlayerPrefs.Save ();
				this.translatorFactory.GetTranslator (locale);
			}
		}

		this.Back ();
	}

	public void StartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
	}

	public void ExitGame () {
		Application.Quit ();
	}

}
