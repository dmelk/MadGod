using System;
using FullmetalKobzar.MadGod.Battle.SkillEffect;
using FullmetalKobzar.MadGod.Battle.AI;

namespace FullmetalKobzar.MadGod.Battle
{
	public class DummyCharacterFactory: ICharacterFactory
	{

		private int createdPotions = 0;

		public Character create (string name, string template) 
		{
			if (template == "player")
				return this.createPlayer (name);
			if (template == "level1meele")
				return this.createLevel1MeeleNPC (name);
			if (template == "level1ranged")
				return this.createLevel1RangedNPC (name);
			if (template == "level1boss")
				return this.createLevel1BossNPC (name);
			return this.createLevel0NPC (name);
		}


		private Character createPlayer (string name)
		{
			Character character = new Character ();
			character.isPlayer = true;
			character.name = name;
			character.toHit = 85f;
			character.crit = 30f;
			character.dodge = 30f;
			character.magicalDodge = 10f;
			character.maxHealth = 15f;
			character.physicalDefence = 0;
			character.magicalDefence = 0;
			character.maxActionPoints = 1;
			character.initiative = 5;
			character.damageModifier = 1f;

			character.meleeDamageMin = 1.75f;
			character.meleeDamageMax = 2.75f;

			character.rangeDamageMin = 1.25f;
			character.rangeDamageMax = 2.25f;

			character.bloodMagicDamageMin = 3f;
			character.bloodMagicDamageMax = 4f;

			character.madMagicDamageMin = 1.5f;
			character.madMagicDamageMax = 2f;

			Skill madSkill1 = new Skill () {
				owner = character,
				title = "Ярость бога",
				targetType = TargetType.ENEMY_FIRST_LINE,
				autoHit = false,
				crit = character.crit,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 1,
				actionPoints = 0,
				isMadMarksRequired = true,
				madMarksRequired = 3
			};
			madSkill1.effectStrategy = new DamageEffectStrategy () {
				parent = madSkill1,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			Skill meleeSkill1 = new Skill () {
				owner = character,
				title = "Удар",
				targetType = TargetType.ENEMY_FIRST_LINE,
				crit = character.crit,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			meleeSkill1.effectStrategy = new DamageEffectStrategy () {
				parent = meleeSkill1,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			Skill shootingSkill1 = new Skill () {
				owner = character,
				title = "Выстрел",
				targetType = TargetType.ENEMY_ALL,
				crit = 50f,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			shootingSkill1.effectStrategy = new DamageEffectStrategy () {
				parent = meleeSkill1,
				damageMin = character.rangeDamageMin,
				damageMax = character.rangeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			Skill madnessSkill1 = new Skill () {
				owner = character,
				title = "Расколотый разум",
				targetType = TargetType.ENEMY_ALL,
				crit = character.crit,
				autoHit = false,
				toHit = 95f,
				allowCrit = false,
				allowDodge = true,
				cooldown = 2,
				actionPoints = 1,
				isMagical = true
			};
			Effect effect = new Effect () {
				stackable = false,
				name = "Расколотый разум",
				damageMin = character.madMagicDamageMin * 0.75f,
				damageMax = character.madMagicDamageMax * 0.75f,
				maxTurns = 4,
				permanent = false,
				isMagical = true,
				isDamage = true,
				allowAct = true,
				ignoreDefence = false,
			};
			madnessSkill1.effectStrategy = new DamageWithDebuffEffectStrategy () {
				parent = madnessSkill1,
				damageMin = character.madMagicDamageMin,
				damageMax = character.madMagicDamageMax,
				ignoreDefence = false,
				isMagical = true,
				debuffTemplate = effect
			};

			Skill bloodSkill1 = new Skill (){
				owner = character,
				title = "Похищение крови",
				targetType = TargetType.ENEMY_ALL,
				crit = character.crit,
				autoHit = false,
				toHit = 95f,
				allowCrit = false,
				allowDodge = true,
				cooldown = 2,
				actionPoints = 1,
				isMagical = true
			};
			bloodSkill1.effectStrategy = new DamageWithHealEffectStrategy () {
				parent = bloodSkill1,
				damageMin = character.bloodMagicDamageMin,
				damageMax = character.bloodMagicDamageMax,
				ignoreDefence = false,
				isMagical = true,
				requireDeathToHeal = false,
				heal = 50f
			};

			Skill passTurn = new Skill () {
				owner = character,
				title = "Пропустить ход",
				targetType = TargetType.SELF,
				crit = character.crit,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 0,
				actionPoints = 0
			};
			passTurn.effectStrategy = new PassTurnEffectStrategy ();

			Skill healSkill = new Skill () {
				owner = character,
				title = "Зелье лечения",
				targetType = TargetType.SELF,
				crit = character.crit,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 1,
				actionPoints = 1,
				isUnlimited = false,
				maxUsages = 4
			};
			healSkill.effectStrategy = new HealPotionEffectStrategy () {
				parent = healSkill,
				healPercent = 25f
			};

			Skill drug1Skill = new Skill () {
				owner = character,
				title = "Наркотик прозрения",
				targetType = TargetType.SELF,
				crit = character.crit,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 1,
				actionPoints = 1,
				isUnlimited = false,
				maxUsages = 2
			};
			drug1Skill.effectStrategy = new DrugEffectStrategy () {
				drugName = "Прозрение",
				parent = drug1Skill,
				maxTurns = 5
			};
			(drug1Skill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
			(drug1Skill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.TO_HIT);
			(drug1Skill.effectStrategy as DrugEffectStrategy).modifierValue.Add (10f);
			(drug1Skill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
			(drug1Skill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.CRIT);
			(drug1Skill.effectStrategy as DrugEffectStrategy).modifierValue.Add (-5f);

			Skill drug2Skill = new Skill () {
				owner = character,
				title = "Наркотик силы",
				targetType = TargetType.SELF,
				crit = character.crit,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 1,
				actionPoints = 1,
				isUnlimited = false,
				maxUsages = 2
			};
			drug2Skill.effectStrategy = new DrugEffectStrategy () {
				drugName = "Сила",
				parent = drug2Skill,
				maxTurns = 5
			};
			(drug2Skill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (true);
			(drug2Skill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.DAMAGE);
			(drug2Skill.effectStrategy as DrugEffectStrategy).modifierValue.Add (1.2f);
			(drug2Skill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (true);
			(drug2Skill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.MAGICAL_DAMAGE);
			(drug2Skill.effectStrategy as DrugEffectStrategy).modifierValue.Add (0.8f);

			character.skills.Add (madSkill1);
			character.skills.Add (meleeSkill1);
			character.skills.Add (shootingSkill1);
			character.skills.Add (madnessSkill1);
			character.skills.Add (bloodSkill1);
			character.skills.Add (healSkill);
			character.skills.Add (drug1Skill);
			character.skills.Add (drug2Skill);
			character.skills.Add (passTurn);

			return character;
		}

		private Character createLevel0NPC (string name)
		{
			Character character = new Character ();
			character.name = name;
			character.toHit = 60f;
			character.crit = 10f;
			character.dodge = 7.5f;
			character.magicalDodge = 5f;
			character.maxHealth = 6f;
			character.physicalDefence = 0;
			character.magicalDefence = 0;
			character.maxActionPoints = 1;
			character.initiative = 4;
			character.damageModifier = 1f;

			character.meleeDamageMin = 1f;
			character.meleeDamageMax = 1.5f;

			Skill battleSkill = new Skill () {
				owner = character,
				title = "Удар",
				targetType = TargetType.ENEMY_FIRST_LINE,
				crit = character.crit,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			battleSkill.effectStrategy = new DamageEffectStrategy () {
				parent = battleSkill,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			character.skills.Add (battleSkill);

			Skill passTurn = new Skill () {
				owner = character,
				title = "Пропустить ход",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 0,
				actionPoints = 0
			};
			passTurn.effectStrategy = new PassTurnEffectStrategy ();

			character.skills.Add (passTurn);

			character.actingStrategy = new SimpleDamageStrategy () {
				battleSkill = battleSkill,
				passSkill = passTurn,
				owner = character
			};

			return character;
		}

		private Character createLevel1MeeleNPC (string name)
		{
			Character character = new Character ();
			character.name = name;
			character.toHit = 60f;
			character.crit = 10f;
			character.dodge = 12.5f;
			character.magicalDodge = 7.5f;
			character.maxHealth = 6f;
			character.physicalDefence = 0;
			character.magicalDefence = 0;
			character.maxActionPoints = 1;
			character.initiative = 3;
			character.damageModifier = 1f;

			character.meleeDamageMin = 0.75f;
			character.meleeDamageMax = 1.25f;

			Skill battleSkill = new Skill () {
				owner = character,
				title = "Удар",
				targetType = TargetType.ENEMY_FIRST_LINE,
				crit = character.crit,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			battleSkill.effectStrategy = new DamageEffectStrategy () {
				parent = battleSkill,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			character.skills.Add (battleSkill);

			Skill passTurn = new Skill () {
				owner = character,
				title = "Пропустить ход",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 0,
				actionPoints = 0
			};
			passTurn.effectStrategy = new PassTurnEffectStrategy ();

			character.skills.Add (passTurn);

			Skill healSkill = null;
			Skill drugSkill = null;
			if (this.createdPotions < 2) {
				healSkill = new Skill () {
					owner = character,
					title = "Зелье лечения",
					targetType = TargetType.SELF,
					autoHit = true,
					allowCrit = false,
					allowDodge = false,
					cooldown = 1,
					actionPoints = 1,
					isUnlimited = false,
					maxUsages = 1
				};
				healSkill.effectStrategy = new HealPotionEffectStrategy () {
					parent = healSkill,
					healPercent = 25f
				};

				character.skills.Add (healSkill);

				drugSkill = new Skill () {
					owner = character,
					title = "Наркотик прозрения",
					targetType = TargetType.SELF,
					autoHit = true,
					allowCrit = false,
					allowDodge = false,
					cooldown = 1,
					actionPoints = 1,
					isUnlimited = false,
					maxUsages = 1
				};
				drugSkill.effectStrategy = new DrugEffectStrategy () {
					drugName = "Прозрение",
					parent = drugSkill,
					maxTurns = 5
				};
				(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.TO_HIT);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (10f);
				(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.CRIT);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (-5f);

				character.skills.Add (drugSkill);

				this.createdPotions ++;
			}
			character.actingStrategy = new SimpleDamageStrategy () {
				battleSkill = battleSkill,
				healSkill = healSkill,
				drugSkill = drugSkill,
				passSkill = passTurn,
				owner = character
			};

			return character;
		}

		private Character createLevel1RangedNPC (string name)
		{
			Character character = new Character ();
			character.name = name;
			character.toHit = 70f;
			character.crit = 35f;
			character.dodge = 5f;
			character.magicalDodge = 7.5f;
			character.maxHealth = 6f;
			character.physicalDefence = 0;
			character.magicalDefence = 0;
			character.maxActionPoints = 1;
			character.initiative = 4;
			character.damageModifier = 1f;

			character.meleeDamageMin = 0.5f;
			character.meleeDamageMax = 1f;

			Skill battleSkill = new Skill () {
				owner = character,
				title = "Выстрел",
				targetType = TargetType.ENEMY_ALL,
				crit = character.crit,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			battleSkill.effectStrategy = new DamageEffectStrategy () {
				parent = battleSkill,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			character.skills.Add (battleSkill);

			Skill passTurn = new Skill () {
				owner = character,
				title = "Пропустить ход",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 0,
				actionPoints = 0
			};
			passTurn.effectStrategy = new PassTurnEffectStrategy ();

			character.skills.Add (passTurn);

			Skill healSkill = null;
			Skill drugSkill = null;
			if (this.createdPotions < 2) {
				healSkill = new Skill () {
					owner = character,
					title = "Зелье лечения",
					targetType = TargetType.SELF,
					autoHit = true,
					allowCrit = false,
					allowDodge = false,
					cooldown = 1,
					actionPoints = 1,
					isUnlimited = false,
					maxUsages = 1
				};
				healSkill.effectStrategy = new HealPotionEffectStrategy () {
					parent = healSkill,
					healPercent = 25f
				};

				character.skills.Add (healSkill);

				drugSkill = new Skill () {
					owner = character,
					title = "Наркотик силы",
					targetType = TargetType.SELF,
					autoHit = true,
					allowCrit = false,
					allowDodge = false,
					cooldown = 1,
					actionPoints = 1,
					isUnlimited = false,
					maxUsages = 1
				};
				drugSkill.effectStrategy = new DrugEffectStrategy () {
					drugName = "Сила",
					parent = drugSkill,
					maxTurns = 5
				};
				(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (true);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.DAMAGE);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (1.2f);
				(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (true);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.MAGICAL_DAMAGE);
				(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (0.8f);

				this.createdPotions ++;
			}

			character.actingStrategy = new SimpleDamageStrategy () {
				battleSkill = battleSkill,
				healSkill = healSkill,
				drugSkill = drugSkill,
				passSkill = passTurn,
				owner = character
			};

			return character;
		}

		private Character createLevel1BossNPC (string name)
		{
			this.createdPotions = 0;

			Character character = new Character ();
			character.name = name;
			character.toHit = 65f;
			character.crit = 10f;
			character.dodge = 12.5f;
			character.magicalDodge = 7.5f;
			character.maxHealth = 7.5f;
			character.physicalDefence = 0;
			character.magicalDefence = 0;
			character.maxActionPoints = 1;
			character.initiative = 4;
			character.damageModifier = 1f;

			character.meleeDamageMin = 1.5f;
			character.meleeDamageMax = 2.5f;

			Skill battleSkill = new Skill () {
				owner = character,
				title = "Удар",
				targetType = TargetType.ENEMY_FIRST_LINE,
				crit = character.crit,
				autoHit = false,
				toHit = character.toHit,
				allowCrit = true,
				allowDodge = true,
				cooldown = 0,
				actionPoints = 1
			};
			battleSkill.effectStrategy = new DamageEffectStrategy () {
				parent = battleSkill,
				damageMin = character.meleeDamageMin,
				damageMax = character.meleeDamageMax,
				ignoreDefence = false,
				isMagical = false
			};

			character.skills.Add (battleSkill);

			Skill passTurn = new Skill () {
				owner = character,
				title = "Пропустить ход",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 0,
				actionPoints = 0
			};
			passTurn.effectStrategy = new PassTurnEffectStrategy ();

			character.skills.Add (passTurn);

			Skill healSkill = new Skill () {
				owner = character,
				title = "Зелье лечения",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 1,
				actionPoints = 1,
				isUnlimited = false,
				maxUsages = 2
			};
			healSkill.effectStrategy = new HealPotionEffectStrategy () {
				parent = healSkill,
				healPercent = 25f
			};

			character.skills.Add (healSkill);

			Skill drugSkill = new Skill () {
				owner = character,
				title = "Наркотик прозрения",
				targetType = TargetType.SELF,
				autoHit = true,
				allowCrit = false,
				allowDodge = false,
				cooldown = 1,
				actionPoints = 1,
				isUnlimited = false,
				maxUsages = 2
			};
			drugSkill.effectStrategy = new DrugEffectStrategy () {
				drugName = "Прозрение",
				parent = drugSkill,
				maxTurns = 5
			};
			(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
			(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.TO_HIT);
			(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (10f);
			(drugSkill.effectStrategy as DrugEffectStrategy).isMultiplier.Add (false);
			(drugSkill.effectStrategy as DrugEffectStrategy).modifierType.Add (ModifierType.CRIT);
			(drugSkill.effectStrategy as DrugEffectStrategy).modifierValue.Add (-5f);

			character.skills.Add (drugSkill);

			character.actingStrategy = new SimpleDamageStrategy () {
				battleSkill = battleSkill,
				healSkill = healSkill,
				drugSkill = drugSkill,
				passSkill = passTurn,
				owner = character
			};

			return character;
		}

	}
}

