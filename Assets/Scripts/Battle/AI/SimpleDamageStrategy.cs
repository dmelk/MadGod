using UnityEngine;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;

namespace FullmetalKobzar.MadGod.Battle.AI
{
	public class SimpleDamageStrategy : IActingStrategy
	{
		public Character owner;

		public Skill battleSkill;

		public Skill passSkill;

		public Skill healSkill;

		public Skill drugSkill;

		public List<EffectReport> Act (BattleField battleField, bool left) 
		{
			float hitValue = Random.Range (0, 10001) * 1.0f / 100f;
			float critValue = Random.Range (0, 10001) * 1.0f / 100f;
			float dodgeValue = Random.Range (0, 10001) * 1.0f / 100f;

			float ownerHealthPercents = this.owner.currHealth / this.owner.maxHealth * 100f;
			if (this.healSkill != null && this.healSkill.CanBeUsed () && ownerHealthPercents <= 60f) {
				return this.healSkill.ApplyEffect (hitValue, critValue, dodgeValue, this.owner);
			}

			if (this.drugSkill != null && this.drugSkill.CanBeUsed () && battleField.turn % 5 == 3) {
				return this.drugSkill.ApplyEffect (hitValue, critValue, dodgeValue, this.owner);
			}

			if (this.battleSkill.CanBeUsed ()) {
				Character[] possibleTargets = battleField.GetPossibleTargets (this.owner, left, this.battleSkill.targetType);
				if (possibleTargets.Length > 0) {
					return this.battleSkill.ApplyEffect (hitValue, critValue, dodgeValue, possibleTargets[0]);
				}
			}
			return this.passSkill.ApplyEffect (hitValue, critValue, dodgeValue, this.owner);
		}
	}
}

