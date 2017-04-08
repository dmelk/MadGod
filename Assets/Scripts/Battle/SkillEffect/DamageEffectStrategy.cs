using System.Collections.Generic;
using UnityEngine;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class DamageEffectStrategy : SimpleEffectStrategy
	{
		public bool ignoreDefence = false;

		public bool isMagical;

		public float damageMin;
		public float damageMax;

		protected float Damage (float damage, Character target) {
			ModifierType type = (this.isMagical) ? ModifierType.MAGICAL_DAMAGE : ModifierType.DAMAGE;

			// apply all modifiers
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != type || modifier.isMultiplier || !modifier.asEnemy)
					continue;
				damage = parent.ApplyModifier (modifier, damage, false);
			}
			foreach (Modifier modifier in parent.owner.modifiers) {
				if (modifier.type != type || modifier.isMultiplier || modifier.asEnemy)
					continue;
				damage = parent.ApplyModifier (modifier, damage, false);
			}

			// apply all multipliers
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != type || !modifier.isMultiplier || !modifier.asEnemy)
					continue;
				damage = parent.ApplyModifier (modifier, damage, true);
			}
			foreach (Modifier modifier in parent.owner.modifiers) {
				if (modifier.type != type || !modifier.isMultiplier || modifier.asEnemy)
					continue;
				damage = parent.ApplyModifier (modifier, damage, true);
			}

			return damage * parent.owner.damageModifier;
		}

		public float DamageWithDefence (float damage, Character target)
		{
			damage = this.Damage (damage, target);
			if (this.ignoreDefence)
				return damage;

			int defence = (this.isMagical) ? target.magicalDefence : target.physicalDefence;

			return damage * ((Character.MAX_DEFENCE - defence) / Character.MAX_DEFENCE);
		}

		public float DamageMin (Character target)
		{
			return this.DamageWithDefence (this.damageMin, target);
		}

		public float DamageMax (Character target)
		{
			return this.DamageWithDefence (this.damageMax, target);
		}

		public override List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> parentReports = base.Apply (hitValue, critValue, dodgeValue, target);
			List<EffectReport> reports = new List<EffectReport> ();
			reports.AddRange (parentReports);
			if (reports.Count > 0) {
				return reports;
			}

			float damage = 0f;
			bool isCrit = parent.IsCrit (critValue, target);
			damage = 1f * Random.Range ((int)(this.DamageMin (target) * 100), (int)(this.DamageMax (target) * 100) + 1) / 100f;
			if (isCrit) {
				damage = damage * 2.0f;
			}
			damage = target.Damage (damage);

			if (isCrit) {
				EffectReport critReport = new EffectReport () {
					type = EffectReportType.OTHER,
					owner = target,
					text = "Критический урон"
				};
				reports.Add (critReport);
			}

			EffectReport report = new EffectReport() {
				type = EffectReportType.DAMAGE,
				owner = target,
				value = damage,
				isCrit = isCrit
			};
			target.reports.Add (report);
			reports.Add (report);

			return reports;
		}
	}
}

