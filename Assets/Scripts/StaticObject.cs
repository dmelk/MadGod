using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullmetalKobzar.Core.Translation;

public class StaticObject : MonoBehaviour {

	private ITranslatorFactory m_translatorFactory;
	private ITranslator m_translator;
		
	public ITranslatorFactory translatorFactory { 
		get {
			if (this.m_translatorFactory == null) {
				this.m_translatorFactory = new UnityTranslatorFactory ();
			}

			return this.m_translatorFactory;
		}
	}
	public ITranslator translator { 
		get {
			if (this.m_translator == null) {
				string locale = PlayerPrefs.GetString ("locale", "ruRu");
				this.m_translator = translatorFactory.GetTranslator (locale);
			}
			return this.m_translator;
		}
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

}
