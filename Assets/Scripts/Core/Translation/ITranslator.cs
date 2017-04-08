using System.Collections.Generic;

namespace FullmetalKobzar.Core.Translation 
{

	public interface ITranslator 
	{
		string Translate (string message, string resource);

		void LoadMessages (Dictionary<string, string> messages, string resource);

		string GetCurrentLocale ();
	}

}
