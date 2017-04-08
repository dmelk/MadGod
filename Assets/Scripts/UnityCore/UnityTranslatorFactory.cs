using System.Collections.Generic;
using UnityEngine;

namespace FullmetalKobzar.Core.Translation
{
	public class UnityTranslatorFactory : ITranslatorFactory
	{
		private string[] resources = {"dialog", "ui"};

		private Translator translator;

		public ITranslator GetTranslator (string locale)
		{
			if (this.translator == null) {
				this.translator = new Translator ();
			}

			string translationPath = "Translations/" + locale + "/";
			foreach (string resource in this.resources) {
				TextAsset messages = Resources.Load <TextAsset> (translationPath + resource);
				if (messages != null) {
					JSONObject jsonObject = new JSONObject(messages.text);
					Dictionary <string, string> dict = new Dictionary <string, string> ();
					for (int i = 0; i < jsonObject.list.Count; i++)
						dict [(string)jsonObject.keys [i]] = jsonObject.list [i].str;
					this.translator.LoadMessages (dict, resource);
				} else {
					this.translator.LoadMessages (new Dictionary <string, string> (), resource);
				}
			}
			this.translator.locale = locale;

			return this.translator;
		}
	}
}

