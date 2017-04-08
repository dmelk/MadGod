using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.AI
{
	public interface IActingStrategy
	{
		List<EffectReport> Act (BattleField battleField, bool left);
	}
}

