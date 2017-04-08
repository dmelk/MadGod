using System.Collections.Generic;

namespace FullmetalKobzar.Core.Dialog {

	public class Dialog : IDialog {
		private Dictionary<string, IReplica> replicas;

		private Dictionary<string, ITransition> transitions;

		private string currentReplica;
		private string firstReplica;

		public Dialog () {
			this.replicas = new Dictionary<string, IReplica> ();
			this.transitions = new Dictionary<string, ITransition> ();
		}

		public void Start ()
		{
			this.currentReplica = this.firstReplica;
		}

		public void SetFirstReplica (string key)
		{
			this.firstReplica = key;
		}

		public void AddReplica (string key, IReplica replica)
		{
			this.replicas.Add (key, replica);
		}

		public void AddTransition (string key, ITransition transition)
		{
			this.transitions.Add (key, transition);
			SimpleReplica replica = this.replicas [transition.GetFromReplicaKey ()] as SimpleReplica;
			replica.AddTransition (transition);
		}

		public IReplica GetReplica (string key) 
		{
			return this.replicas [key];
		}

		public IReplica GetCurrentReplica () 
		{
			return this.GetReplica (this.currentReplica);
		}

		public void SetCurrentReplica (string key) {
			this.currentReplica = key;
		}
	}

}