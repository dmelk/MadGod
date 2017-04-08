using System;

namespace FullmetalKobzar.MadGod.Battle
{
	public enum ModifierType {
		TO_HIT,
		CRIT,
		DODGE,
		MAGICAL_DODGE,
		DAMAGE,
		MAGICAL_DAMAGE
	}

	public struct Modifier
	{
		public float value;
		public bool asEnemy;
		public string tag;
		public int modifierId;
		public bool isMultiplier;
		public ModifierType type;
	}
}

