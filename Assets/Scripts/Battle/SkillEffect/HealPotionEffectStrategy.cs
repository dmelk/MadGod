using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class HealPotionEffectStrategy : ISkillEffectStrategy
	{
		public Skill parent;

		public float healPercent;

		public virtual List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> reports = new List<EffectReport> ();

			parent.owner.currActionPoints -= parent.actionPoints;
			parent.turnsToActivate = parent.cooldown;
			if (parent.maxUsages > 0) {
				parent.maxUsages --;
			} else {
				return reports;
			}

			float heal = target.Heal (target.maxHealth * healPercent / 100f);

			EffectReport report = new EffectReport() {
				type = EffectReportType.HEAL,
				owner = target,
				value = heal
			};
			target.reports.Add (report);
			reports.Add (report);

			return reports;
		}
	}
}

