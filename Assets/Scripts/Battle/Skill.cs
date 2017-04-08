using System.Collections.Generic;
using UnityEngine;
using FullmetalKobzar.MadGod.Battle.SkillEffect;

namespace FullmetalKobzar.MadGod.Battle
{
	public enum TargetType {
		SELF,
		ENEMY_ALL,
		ENEMY_FIRST_LINE,
		FRIEND
	}

	public class Skill
	{
		public Character owner;

		public string title;

		public TargetType targetType;

		public bool autoHit;
		public float toHit;

		public float crit = 0f;

		public bool allowCrit = true;
		public bool allowDodge = true;

		public int cooldown;
		public int turnsToActivate = 0;

		public int actionPoints;

		public bool onlyTaggedModifier = false;
		public List<string> modifierTags;

		public List<string> ignoredTags;

		public bool isMagical = false;

		public bool isUnlimited = true;
		public int maxUsages = 0;

		public ISkillEffectStrategy effectStrategy;

		public bool isMadMarksRequired = false;
		public int madMarksRequired = 0;

		public Skill()
		{
			this.modifierTags = new List<string> ();
			this.ignoredTags = new List<string> ();
		}

		public virtual void Update() 
		{
			if (this.turnsToActivate > 0)
				this.turnsToActivate--;
		}

		public virtual bool CanBeUsed() 
		{
			if (!this.isUnlimited && this.maxUsages == 0)
				return false;
			if (this.turnsToActivate > 0)
				return false;
			if (this.actionPoints > owner.currActionPoints)
				return false;
			if (this.isMadMarksRequired && this.madMarksRequired > owner.madMarksCount)
				return false;
			return true;
		}

		public float ToHitValue(Character target) {
			if (this.autoHit)
				return 100f;

			float toHitValue = this.toHit;

			// apply all modifiers
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != ModifierType.TO_HIT || modifier.isMultiplier || !modifier.asEnemy)
					continue;
				toHitValue = this.ApplyModifier (modifier, toHitValue, false);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != ModifierType.TO_HIT || modifier.isMultiplier || modifier.asEnemy)
					continue;
				toHitValue = this.ApplyModifier (modifier, toHitValue, false);
			}

			// apply all multiplier
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != ModifierType.TO_HIT || !modifier.isMultiplier || !modifier.asEnemy)
					continue;
				toHitValue = this.ApplyModifier (modifier, toHitValue, true);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != ModifierType.TO_HIT || !modifier.isMultiplier || modifier.asEnemy)
					continue;
				toHitValue = this.ApplyModifier (modifier, toHitValue, true);
			}

			return toHitValue;
		}

		public float CritValue (Character target)
		{
			if (!this.allowCrit)
				return -1f;

			float critValue = this.crit;

			// apply all modifiers
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != ModifierType.CRIT || modifier.isMultiplier || !modifier.asEnemy)
					continue;
				critValue = this.ApplyModifier (modifier, critValue, false);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != ModifierType.CRIT || modifier.isMultiplier || modifier.asEnemy)
					continue;
				critValue = this.ApplyModifier (modifier, critValue, false);
			}

			// apply all multiplier
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != ModifierType.CRIT || !modifier.isMultiplier || !modifier.asEnemy)
					continue;
				critValue = this.ApplyModifier (modifier, critValue, true);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != ModifierType.CRIT || !modifier.isMultiplier || modifier.asEnemy)
					continue;
				critValue = this.ApplyModifier (modifier, critValue, true);
			}

			return critValue;
		}

		public float DodgeValue (Character target)
		{
			if (!this.allowDodge)
				return -1f;

			float dodgeValue = (this.isMagical) ? target.magicalDodge : target.dodge;
			ModifierType type = (this.isMagical) ? ModifierType.MAGICAL_DODGE : ModifierType.DODGE;

			float hitValue = this.ToHitValue (target);
			if (hitValue > 100f) {
				dodgeValue = dodgeValue - (hitValue - 100f) / 3.0f;
			}

			// apply all modifiers
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != type || modifier.isMultiplier || !modifier.asEnemy)
					continue;
				dodgeValue = this.ApplyModifier (modifier, dodgeValue, false);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != type || modifier.isMultiplier || modifier.asEnemy)
					continue;
				dodgeValue = this.ApplyModifier (modifier, dodgeValue, false);
			}

			// apply all multiplier
			foreach (Modifier modifier in target.modifiers) {
				if (modifier.type != type || !modifier.isMultiplier || !modifier.asEnemy)
					continue;
				dodgeValue = this.ApplyModifier (modifier, dodgeValue, true);
			}
			foreach (Modifier modifier in owner.modifiers) {
				if (modifier.type != type || !modifier.isMultiplier || modifier.asEnemy)
					continue;
				dodgeValue = this.ApplyModifier (modifier, dodgeValue, true);
			}

			return dodgeValue;
		}

		public float ApplyModifier (Modifier modifier, float value, bool isMultiplier)
		{
			// don't apply modifier if skill limited to tags and modifier doesn't have this tag
			if (this.onlyTaggedModifier && !this.modifierTags.Contains (modifier.tag)) {
				return value;
			}

			// don't apply modifier if skill ignores it's tag
			if (this.ignoredTags.Contains (modifier.tag)) {
				return value;
			}

			return (isMultiplier)? (value * modifier.value) : (value + modifier.value);
		}

		public bool IsHit (float hitValue, Character target)
		{
			return hitValue <= this.ToHitValue (target);
		}

		public bool IsCrit (float critValue, Character target)
		{
			return critValue <= this.CritValue (target);
		}

		public bool IsDodge (float dodgeValue, Character target)
		{
			return dodgeValue <= this.DodgeValue (target);
		}

		public List<EffectReport> ApplyEffect (float hitValue, float critValue, float dodgeValue, Character target)
		{
			return this.effectStrategy.Apply (hitValue, critValue, dodgeValue, target);
		}
	}
}

