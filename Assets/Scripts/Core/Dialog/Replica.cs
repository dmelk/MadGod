using FullmetalKobzar.Core.Condition;

namespace FullmetalKobzar.Core.Dialog {

	public abstract class Replica : IReplica {
		public const int NORMAL_STATE = 0;
		public const int FINAL_STATE = 1;
		public const int BATTLE_STATE = 2;

		protected bool isPlayerReplica = false;
		protected int replicaState = Replica.NORMAL_STATE;

		protected ICondition condition;

		public bool IsPlayerReplica ()
		{
			return this.isPlayerReplica;
		}

		public bool IsFinal ()
		{
			return this.replicaState == Replica.FINAL_STATE || this.replicaState == Replica.BATTLE_STATE;
		}

		public bool IsBattle ()
		{
			return this.replicaState == Replica.BATTLE_STATE;
		}

		public bool IsApplicable ()
		{
			if (this.condition == null)
				return true;
			return this.condition.GetResult ();
		}
	}

}
