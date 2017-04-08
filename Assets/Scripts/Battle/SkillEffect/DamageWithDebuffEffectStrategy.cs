using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class DamageWithDebuffEffectStrategy : DamageEffectStrategy
	{
		public Effect debuffTemplate;

		public override List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> parentReports = base.Apply (hitValue, critValue, dodgeValue, target);
			List<EffectReport> reports = new List<EffectReport> ();
			reports.AddRange (parentReports);

			for (var i = 0; i < parentReports.Count; i++) {
				if (parentReports [i].isStopChild) {
					return reports;
				}
			}

			// check if effect is stackable. If not - find same effect and remove it
			if (!this.debuffTemplate.stackable) {
				foreach (Effect effect in target.debuffs) {
					if (effect.name == this.debuffTemplate.name) {
						target.debuffs.Remove (effect);
						break;
					}
				}
			}

			// create debuff from template and init it
			Effect debuff = new Effect(target);
			debuff.name = this.debuffTemplate.name;
			debuff.stackable = this.debuffTemplate.stackable;
			debuff.allowAct = this.debuffTemplate.allowAct;
			debuff.isDamage = this.debuffTemplate.isDamage;
			debuff.damageMin = this.Damage(this.debuffTemplate.damageMin, target);
			debuff.damageMax = this.Damage(this.debuffTemplate.damageMax, target);
			debuff.ignoreDefence = this.debuffTemplate.ignoreDefence;
			debuff.permanent = this.debuffTemplate.permanent;
			debuff.maxTurns = this.debuffTemplate.maxTurns;
			debuff.isMagical = this.debuffTemplate.isMagical;

			debuff.Init ();

			target.debuffs.Add(debuff);

			EffectReport report = new EffectReport() {
				type = EffectReportType.OTHER,
				owner = target,
				text = "Наложен дебафф " + debuff.name,
				isEffectAdd = true,
				effectName = debuff.name
			};
			target.reports.Add (report);
			reports.Add (report);

			return reports;
		}
	}
}

