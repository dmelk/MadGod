using System;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.SkillEffect
{
	public class DrugEffectStrategy : ISkillEffectStrategy
	{
		public Skill parent;

		public List<float> modifierValue;
		public List<ModifierType> modifierType;
		public List<bool> isMultiplier;

		public int maxTurns;

		public string drugName;

		public DrugEffectStrategy () {
			this.modifierValue = new List<float> ();
			this.modifierType = new List<ModifierType> ();
			this.isMultiplier = new List<bool> ();
		}

		public virtual List<EffectReport> Apply (float hitValue, float critValue, float dodgeValue, Character target)
		{
			List<EffectReport> reports = new List<EffectReport> ();

			parent.owner.currActionPoints -= parent.actionPoints;
			parent.turnsToActivate = parent.cooldown;
			if (parent.maxUsages > 0) {
				parent.maxUsages--;
			} else {
				return reports;
			}

			EffectReport report;

			// check if target has overdose debuff
			foreach (Effect debuff in target.debuffs) {
				if (debuff.isOverdose) {
					// kill target
					float damage = target.Damage (target.maxHealth);
					report = new EffectReport() {
						type = EffectReportType.DAMAGE,
						owner = target,
						value = damage
					};

					return reports;
				}
			}

			// check if target already used drugs and using another drug
			if (target.drugUsed.Count > 0 && !target.drugUsed.Contains (this.drugName)) {
				// create overdose debuff
				Effect overdoseEffect = new Effect (target) {
					permanent = true,
					stackable = false,
					isOverdose = true,
					name = "Передоз"
				};
				target.debuffs.Add (overdoseEffect);

				// create stun effect
				Effect stunEffect = new Effect (target) {
					permanent = false,
					maxTurns = 2,
					allowAct = false,
					stackable = false,
					name = "Оглушение"
				};
				target.debuffs.Add (stunEffect);

				report = new EffectReport() {
					type = EffectReportType.OTHER,
					owner = target,
					effectName = "Передоз",
					text = "Передоз",
					isEffectAdd = true
				};
				target.reports.Add (report);
				reports.Add (report); 
				return reports;
			}

			target.drugUsed.Add (drugName);

			// remove drug effect if exists
			foreach (Effect effect in target.buffs) {
				if (effect.name == this.drugName) {
					effect.RemoveModifiers ();
					target.buffs.Remove (effect);
					break;
				}
			}

			Effect drugEffect = new Effect (target) {
				maxTurns = this.maxTurns,
				permanent = false,
				stackable = false,
				useModifier = true,
				name = this.drugName
			};
			drugEffect.modifierType.AddRange (this.modifierType);
			drugEffect.modifierValue.AddRange (this.modifierValue);
			drugEffect.isMultiplier.AddRange (this.isMultiplier);

			drugEffect.Init ();

			target.buffs.Add (drugEffect);

			report = new EffectReport() {
				type = EffectReportType.OTHER,
				owner = target,
				text = "Использован наркотик " + this.drugName,
				isEffectAdd = true,
				effectName = this.drugName
			};
			target.reports.Add (report);
			reports.Add (report); 

			return reports;
		}
	}
}

