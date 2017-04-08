using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public interface ISkillEffectStrategy
	{
		List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target);
	}
}

