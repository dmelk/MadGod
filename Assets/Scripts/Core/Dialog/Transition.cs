using FullmetalKobzar.Core.Condition;

namespace FullmetalKobzar.Core.Dialog {

	public class Transition : ITransition
	{
		private string fromReplica;

		private string toReplica;

		private ICondition condition = null;

		public Transition (string fromReplica, string toReplica) {
			this.fromReplica = fromReplica;
			this.toReplica = toReplica;
		}

		public Transition (string fromReplica, string toReplica, ICondition condition) {
			this.fromReplica = fromReplica;
			this.toReplica = toReplica;
			this.condition = condition;
		}

		public string GetFromReplicaKey()
		{
			return this.fromReplica;
		}

		public string GetToReplicaKey ()
		{
			return this.toReplica;
		}

		public bool IsApplicable () 
		{
			if (this.condition == null)
				return true;
			return this.condition.GetResult ();
		}

	}
}

