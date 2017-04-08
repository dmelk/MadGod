using System;

namespace FullmetalKobzar.MadGod.Battle
{
	public interface ICharacterFactory
	{
		Character create (string name, string template);
	}
}

