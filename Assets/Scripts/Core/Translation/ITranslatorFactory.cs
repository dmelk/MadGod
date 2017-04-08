using System;

namespace FullmetalKobzar.Core.Translation
{
	public interface ITranslatorFactory
	{
		ITranslator GetTranslator (string locale);
	}
}

