//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class Character {
//
//	public float toHit;
//	public float damageMin;
//	public float damageMax;
//	public float maxHealth;
//	public float currHealth;
//	public float crit;
//	public float dodge;
//	public int armour;
//	public int maxActionPoints;
//	public int currActionPoints;
//	public int initiative;
//	public bool turnEnded;
//
//	private List<AbstractAbility> abilities;
//	private List<Debuff> debuffs;
//
//	public Character() {
//		this.abilities = new List<AbstractAbility> ();
//		this.debuffs = new List<Debuff> ();
//	}
//
//	public void Awake() {
//		this.currActionPoints = this.maxActionPoints;
//		this.currHealth = this.maxHealth;
//	}
//
//	public void Update() {
//		this.turnEnded = false;
//		this.currActionPoints = this.maxActionPoints;
//
//		for (int i = 0; i < this.abilities.Count; i++) {
//			this.abilities [i].Update ();
//		}
//
//		List<Debuff> debuffsToRemove = new List<Debuff> ();
//		for (int i = 0; i < this.debuffs.Count; i++) {
//			this.debuffs [i].Update ();
//			if (this.debuffs [i].lifeTime <= 0) {
//				debuffsToRemove.Add (this.debuffs [i]);
//			}
//		}
//		for (int i = 0; i < debuffsToRemove.Count; i++) {
//			this.debuffs.Remove (debuffsToRemove [i]);
//		}
//		debuffsToRemove = null;
//	}
//
//	public void AddDebuff(Debuff debuff) {
//		this.debuffs.Add (debuff);
//	}
//
//	public void AddAbility(AbstractAbility ability) {
//		this.abilities.Add (ability);
//	}
//
//	public List<AbstractAbility> GetPossibleAbilities() {
//		List<AbstractAbility> result =  new List<AbstractAbility> ();
//
//		for (var i = 0; i < this.abilities.Count; i++) {
//			if (this.abilities [i].actionPoints > this.currActionPoints)
//				continue;
//			if (!this.abilities [i].CanBeUsed ())
//				continue;
//			result.Add (this.abilities [i]);
//		}
//
//		return result;
//	}
//
//	public bool CanAct() {
//		if (this.turnEnded || (this.currHealth == 0))
//			return false;
//		// check for debuffs
//		for (int i = 0; i < this.debuffs.Count; i++) {
//			if (!this.debuffs [i].allowAct)
//				return false;
//		}
//		if (this.currActionPoints > 0)
//			return true;
//		return false;
//	}
//
//	public void AddDamage(float damage)
//	{
//		this.currHealth -= damage;
//		if (this.currHealth <= 0)
//			this.currHealth = 0;
//	}
//
//	public bool IsDead() {
//		return this.currHealth == 0;
//	}
//}
//
//public abstract class AbstractAbility 
//{
//	public const int TO_HIT_FROM_OWNER = 0;
//	public const int TO_HIT_VALUE = 1;
//
//	public const int DAMAGE_FROM_OWNER = 0;
//	public const int DAMGE_VALUE = 1;
//
//	public const int EFFECT_DAMAGE = 0;
//	public const int EFFECT_SPECIAL = 1;
//
//	public string title;
//	public string name;
//
//	public Character owner;
//
//	public int effectType;
//
//	public int actionPoints;
//
//	public int coolDown;
//	public int beforUseTime;
//
//	public int toHitType;
//	public bool autoHit;
//	public float toHit;
//
//	public bool allowCrit = true;
//	public bool allowDodge = true;
//
//	public bool ignoreArmour = false;
//
//	public int damageType;
//	public float damageMin;
//	public float damageMax;
//
//	public bool CanBeUsed() {
//		return this.beforUseTime == 0;
//	}
//
//	public void SetCoolDown() {
//		this.beforUseTime = this.coolDown;
//	}
//
//	public void Update() {
//		if (this.beforUseTime == 0)
//			return;
//		this.beforUseTime--;
//	}
//
//	public float GetToHit() {
//		if (this.autoHit)
//			return 100f;
//
//		if (this.toHitType == AbstractAbility.TO_HIT_FROM_OWNER)
//			return this.owner.toHit;
//
//		return this.toHit;
//	}
//
//	public float GetDamageMin() {
//		if (this.damageType == AbstractAbility.DAMAGE_FROM_OWNER)
//			return this.owner.damageMin;
//
//		return this.damageMin;
//	}
//
//	public float GetDamageMax() {
//		if (this.damageType == AbstractAbility.DAMAGE_FROM_OWNER)
//			return this.owner.damageMax;
//
//		return this.damageMax;
//	}
//
//	public bool IsHit(float hitValue) {
//		if (this.autoHit)
//			return true;
//		return hitValue <= this.GetToHit ();
//	}
//
//	public bool IsCrit(float hitValue) {
//		if (!this.allowCrit)
//			return false;
//		return hitValue < this.GetToHit () * owner.crit / 100;
//	}
//
//	public bool IsDodge(Character target, float dodgeValue) {
//		if (!this.allowDodge)
//			return false;
//		return dodgeValue < target.dodge;
//	}
//
//	public float GetDamage (Character target, bool isCrit) {
//		if (isCrit)
//			return this.GetDamageMax ();
//		float damage = Random.Range (this.GetDamageMin (), this.GetDamageMax ());
//		if (this.ignoreArmour)
//			return damage;
//		return damage * ((10f - target.armour) / 10f);
//	}
//
//	public virtual void Effect (Character target) {
//		// template method for special effect
//	}
//}
//
//public class AttackAbility : AbstractAbility {
//	public AttackAbility (Character owner) {
//		this.title = "Attack";
//		this.name = "attack";
//		this.owner = owner;
//		this.toHitType = AbstractAbility.TO_HIT_FROM_OWNER;
//		this.damageType = AbstractAbility.DAMAGE_FROM_OWNER;
//		this.effectType = AbstractAbility.EFFECT_DAMAGE;
//		this.actionPoints = 1;
//		this.coolDown = 0;
//		this.beforUseTime = 0;
//	}
//}
//
//public class LightningAbility : AbstractAbility {
//	public LightningAbility (Character owner) {
//		this.title = "Lightning spell";
//		this.name = "spell";
//		this.owner = owner;
//		this.autoHit = true;
//		this.allowCrit = false;
//		this.damageType = AbstractAbility.DAMGE_VALUE;
//		this.damageMin = 8f;
//		this.damageMax = 10f;
//		this.effectType = AbstractAbility.EFFECT_DAMAGE;
//		this.actionPoints = 1;
//		this.coolDown = 2;
//		this.ignoreArmour = true;
//		this.beforUseTime = 0;
//	}
//}
//
//public class StunAbility : AbstractAbility {
//	public StunAbility (Character owner) {
//		this.title = "Stun";
//		this.name = "stun";
//		this.owner = owner;
//		this.autoHit = true;
//		this.allowCrit = false;
//		this.allowDodge = false;
//		this.effectType = AbstractAbility.EFFECT_SPECIAL;
//		this.actionPoints = 1;
//		this.coolDown = 4;
//		this.beforUseTime = 0;
//	}
//
//	public override void Effect (Character enemy) {
//		enemy.AddDebuff (new StunDebuff ());
//	}
//}
//
//public abstract class Debuff
//{
//	public bool permanent;
//	public int lifeTime;
//
//	public bool allowAct;
//
//	public void Update() {
//		if (!this.permanent)
//			this.lifeTime--;
//	}
//}
//
//public class StunDebuff : Debuff {
//	public StunDebuff () {
//		this.permanent = false;
//		this.lifeTime = 2;
//		this.allowAct = false;
//	}
//}