using System;

namespace FullmetalKobzar.MadGod.Battle
{
	public enum EffectReportType {
		DAMAGE,
		HEAL,
		OTHER
	}

	public class EffectReport
	{
		public EffectReportType type;

		public Character owner;

		public float value;

		public string text;

		public bool isStopChild = false;

		public bool isCrit = false;

		public bool isMiss = false;

		public bool isDodge = false;

		public bool isEffectAdd = false;

		public bool isEffectRemove = false;

		public string effectName = "";
	}
}

