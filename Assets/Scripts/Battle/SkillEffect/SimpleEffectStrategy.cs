using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	abstract public class SimpleEffectStrategy : ISkillEffectStrategy
	{
		public Skill parent;

		public virtual List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> reports = new List<EffectReport> ();

			parent.owner.currActionPoints -= parent.actionPoints;
			parent.turnsToActivate = parent.cooldown;
			if (parent.isMadMarksRequired)
				parent.owner.madMarksCount -= parent.madMarksRequired;

			if (!parent.IsHit(hitValue, target)) {
				EffectReport report = new EffectReport() {
					type = EffectReportType.OTHER,
					owner = this.parent.owner,
					isStopChild = true,
					text = "Промах",
					isMiss = true
				};
				this.parent.owner.reports.Add (report);
				reports.Add (report);

				return reports;
			}
			if (parent.IsDodge(hitValue, target)) {
				EffectReport report = new EffectReport() {
					type = EffectReportType.OTHER,
					owner = target,
					isDodge = true,
					isStopChild = true,
					text = "Уклон"
				};
				target.reports.Add (report);
				reports.Add (report);

				return reports;
			}

			if (!parent.isMadMarksRequired)
				parent.owner.madMarksCount++;

			return reports;
		}
	}
}

