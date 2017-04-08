using System;
using System.Collections.Generic;
using UnityEngine;
using FullmetalKobzar.MadGod.Battle.AI;

namespace FullmetalKobzar.MadGod.Battle
{
	public class Character
	{
		public static float MAX_DEFENCE = 100f;

		public int maxModifierId = 0;

		public string name;

		public bool isPlayer = false;

		public Vector2 position;

		public float toHit;

		public float crit;

		public float dodge;

		public float magicalDodge;

		public float maxHealth;
		public float currHealth;

		public int physicalDefence;
		public int magicalDefence;

		public int maxActionPoints;
		public int currActionPoints;

		public int initiative;

		public float damageModifier;

		public List<Modifier> modifiers;

		public float meleeDamageMin;
		public float meleeDamageMax;

		public float rangeDamageMin;
		public float rangeDamageMax;

		public float bloodMagicDamageMin;
		public float bloodMagicDamageMax;

		public float madMagicDamageMin;
		public float madMagicDamageMax;

		public bool turnEnded;

		public List<Effect> buffs;
		public List<Effect> debuffs;

		public List<Skill> skills;

		public List<string> drugUsed;

		public IActingStrategy actingStrategy = null;

		public int madMarksCount = 0;

		public List<EffectReport> reports;

		public Character() 
		{
			this.buffs = new List<Effect> ();
			this.debuffs = new List<Effect> ();
			this.skills = new List<Skill> ();
			this.modifiers = new List<Modifier> ();
			this.drugUsed = new List<string> ();
			this.reports = new List<EffectReport> ();
		}

		public List<EffectReport> Update() 
		{
			List<EffectReport> reports = new List<EffectReport> ();

			this.turnEnded = false;
			this.currActionPoints = this.maxActionPoints;
			foreach (Skill skill in this.skills) {
				skill.Update ();
			}

			foreach (Effect buff in this.buffs) {
				List<EffectReport> result = buff.Update ();
				reports.AddRange (result);
			}
			foreach (Effect debuff in this.debuffs) {
				List<EffectReport> result = debuff.Update ();
				reports.AddRange (result);
			}

			List<Effect> effectsToRemove = new List<Effect> ();

			foreach (Effect buff in this.buffs) {
				if (buff.IsEnded ()) {
					EffectReport report = new EffectReport() {
						type = EffectReportType.OTHER,
						owner = this,
						text = "Бафф закончился: " + buff.name
					};
					reports.Add (report);
					buff.End ();
					effectsToRemove.Add (buff);
				}
			}
			foreach (Effect effect in effectsToRemove) {
				this.buffs.Remove (effect);
			}
			effectsToRemove.Clear ();
			foreach (Effect debuff in this.debuffs) {
				if (debuff.IsEnded ()) {
					EffectReport report = new EffectReport() {
						type = EffectReportType.OTHER,
						owner = this,
						text = "Дебафф закончился: " + debuff.name
					};
					reports.Add (report);
					debuff.End ();
					effectsToRemove.Add (debuff);
				}
			}
			foreach (Effect effect in effectsToRemove) {
				this.debuffs.Remove (effect);
			}
			effectsToRemove.Clear ();

			return reports;
		}

		public List<Skill> PossibleSkills()
		{
			List<Skill> skills = new List<Skill> ();
			foreach (Skill skill in this.skills) {
				if (skill.CanBeUsed ()) {
					skills.Add (skill);
				}
			}
			return skills;
		}

		public bool CanAct(int initiative) 
		{
			if (this.turnEnded || this.IsDead () || this.initiative != initiative)
				return false;
			foreach (Effect debuf in this.debuffs) {
				if (!debuf.allowAct)
					return false;
			}
			if (this.PossibleSkills().Count == 0)
				return false;
			if (this.PossibleSkills ().Count == 1 && this.currActionPoints == 0)
				return false;
			return true;
		}

		public float ChangeHealth(float amount, bool add = true)
		{
			float value = amount;
			if (add) {
				value = Math.Min (value, this.maxHealth - this.currHealth);
				this.currHealth += value;
				return value;
			}
			value = Math.Min (value, this.currHealth);
			this.currHealth -= value;
			return value;
		}

		public float Damage (float amount)
		{
			return this.ChangeHealth (amount, false);
		}

		public float Heal (float amount)
		{
			return this.ChangeHealth (amount);
		}

		public bool IsDead() 
		{
			return this.currHealth == 0;
		}

		public List<EffectReport> Act (BattleField battleField, bool left) 
		{
			return this.actingStrategy.Act (battleField, left);
		}
	}
}

