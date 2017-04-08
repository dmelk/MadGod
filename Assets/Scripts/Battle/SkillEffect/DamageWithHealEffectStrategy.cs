using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class DamageWithHealEffectStrategy : DamageEffectStrategy
	{
		public float heal;
		public bool requireDeathToHeal = false;

		public override List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> parentReports = base.Apply (hitValue, critValue, dodgeValue, target);
			List<EffectReport> reports = new List<EffectReport> ();
			reports.AddRange (parentReports);

			if (requireDeathToHeal && !target.IsDead ()) {
				return reports;
			}
			for (var i = 0; i < parentReports.Count; i++) {
				if (parentReports [i].isStopChild) {
					return reports;
				}
			}

			float damage = 0f;
			for (var i = 0; i < parentReports.Count; i++) {
				if (parentReports [i].type == EffectReportType.DAMAGE) {
					damage += parentReports [i].value;
				}
			}

			float heal = this.parent.owner.Heal (damage * this.heal / 100f);

			if (heal > 0f) {
				EffectReport report = new EffectReport() {
					type = EffectReportType.HEAL,
					owner = this.parent.owner,
					value = heal
				};
				this.parent.owner.reports.Add (report);
				reports.Add (report);
			}

			return reports;
		}
	}
}

