using System.Collections.Generic;
using UnityEngine;

namespace FullmetalKobzar.MadGod.Battle
{
	public class Effect
	{
		private Character owner;

		public string name;

		public bool stackable;

		public bool allowAct = true;

		public bool permanent;

		public bool isDamage;
		public bool ignoreDefence = false;
		public float damageMin;
		public float damageMax;
		public bool isMagical;

		public int maxTurns;
		private int turnsLeft;

		public bool isOverdose = false;

		private List<int> modifierId;

		public bool useModifier = false;
		public List<bool> isMultiplier;
		public List<float> modifierValue;
		public List<ModifierType> modifierType;

		public Effect ()
		{
			this.modifierId = new List<int> ();
			this.modifierValue = new List<float> ();
			this.modifierType = new List<ModifierType> ();
			this.isMultiplier = new List<bool> ();
		}

		public Effect (Character owner) : this ()
		{
			this.owner = owner;
		}

		public void Init()
		{
			this.turnsLeft = this.maxTurns;
			if (this.useModifier) {
				this.AddModifiers ();
			}
		}

		public List<EffectReport> Update() 
		{
			List<EffectReport> reports = new List<EffectReport> ();
			this.turnsLeft --;
			if (this.isDamage)
				reports.Add (this.Damage ());
			if (this.useModifier && this.IsEnded ()) {
				this.RemoveModifiers ();
			}

			if (this.IsEnded ()) {
				EffectReport report = new EffectReport() {
					type = EffectReportType.OTHER,
					owner = owner,
					isEffectRemove = true,
					effectName = this.name
				};
				owner.reports.Add (report);
			}

			return reports;
		}

		public EffectReport Damage() {
			float damage = 1f * Random.Range ((int)(this.damageMin * 100), (int)(this.damageMax * 100) + 1) / 100f;
			if (!this.ignoreDefence) {
				float defence = (this.isMagical) ? owner.magicalDefence : owner.physicalDefence;
				damage = damage * ((Character.MAX_DEFENCE - defence) / Character.MAX_DEFENCE);
			}
			damage = owner.Damage (damage);

			EffectReport report = new EffectReport() {
				type = EffectReportType.DAMAGE,
				owner = owner,
				value = damage
			};
			owner.reports.Add (report);

			return report;
		}

		public void AddModifiers ()
		{
			for (int i = 0; i < this.modifierValue.Count; i++) {
				float value = this.modifierValue [i];
				Modifier modifier = new Modifier () {
					asEnemy = false,
					value = value,
					tag = "",
					modifierId = this.owner.maxModifierId,
					isMultiplier = this.isMultiplier [i],
					type = this.modifierType [i]
				};
				this.modifierId.Add (this.owner.maxModifierId);
				this.owner.maxModifierId++;

				this.owner.modifiers.Add (modifier);
			}
		}

		public void RemoveModifiers () {
			List <Modifier> modifiersToRemove = new List<Modifier> ();
			foreach (Modifier modifier in this.owner.modifiers) {
				if (this.modifierId.Contains (modifier.modifierId)) {
					modifiersToRemove.Add (modifier);
				}
			}
			foreach (Modifier modifier in modifiersToRemove) {
				this.owner.modifiers.Remove (modifier);
			}
			modifiersToRemove.Clear ();
		}

		public bool IsEnded() 
		{
			if (this.permanent)
				return false;
			return this.turnsLeft <= 0;
		}

		public void End() 
		{
		}
	}
}

