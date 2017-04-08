using UnityEngine;
using System.Collections;

public class DragonMonster : MonoBehaviour {

	private Animator mAnimator;
	private bool attack = false;

//	public Character character;

	private void Awake()
	{
		this.mAnimator = GetComponent<Animator>();

		// init player character
//		this.character = new Character ();
//		this.character.armour = 5;
//		this.character.toHit = 80f;
//		this.character.damageMin = 2f;
//		this.character.damageMax = 4f;
//		this.character.crit = 10f;
//		this.character.dodge = 10f;
//		this.character.maxActionPoints = 1;
//		this.character.maxHealth = 20f;
//		this.character.initiative = 2;
//		this.character.AddAbility (new AttackAbility (this.character));
//
//		this.character.Awake ();
	}

	void FixedUpdate () {
//		this.mAnimator.SetBool("Dead", this.character.IsDead());
		this.mAnimator.SetBool("Attack", this.attack);

		this.attack = false;
	}

	public void ActionAnimation(string actionName) {
		this.attack = true;
	}

}
