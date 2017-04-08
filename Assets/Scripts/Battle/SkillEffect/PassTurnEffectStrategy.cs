using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class PassTurnEffectStrategy : ISkillEffectStrategy
	{
		public List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			target.turnEnded = true;

			EffectReport report = new EffectReport() {
				type = EffectReportType.OTHER,
				owner = target,
				text = "Пропуск хода"
			};
			List<EffectReport> reports = new List<EffectReport> ();
			reports.Add (report);

			return reports;
		}
	}
}

