using System.Collections.Generic;

namespace FullmetalKobzar.Core.Translation
{
	public class Translator : ITranslator
	{
		private Dictionary <string, Dictionary<string, string>> messages;

		public string locale { private get; set; }

		public Translator () {
			this.messages = new Dictionary<string, Dictionary<string, string>> ();
		}

		public string Translate (string message, string resource) {
			return (this.messages.ContainsKey (resource) && this.messages [resource].ContainsKey (message)) ? this.messages [resource] [message] : message;
		}

		public void LoadMessages (Dictionary<string, string> messages, string resource) {
			this.messages [resource] = messages;
		}

		public string GetCurrentLocale () {
			return this.locale;
		}
	}
}

