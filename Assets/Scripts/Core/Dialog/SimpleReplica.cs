using System.Collections.Generic;

namespace FullmetalKobzar.Core.Dialog {

	public class SimpleReplica : Replica {
		public string text { get; private set; }
		public List<ITransition> transitions { get; private set; }

		public SimpleReplica (string text)  
		{
			this.text = text;
			this.transitions = new List<ITransition> ();
		}

		public SimpleReplica (string text, bool isPlayerReplica)  
		{
			this.text = text;
			this.isPlayerReplica = isPlayerReplica;
			this.transitions = new List<ITransition> ();
		}

		public SimpleReplica (string text, bool isPlayerReplica, int replicaState)  
		{
			this.text = text;
			this.isPlayerReplica = isPlayerReplica;
			this.replicaState = replicaState;
			this.transitions = new List<ITransition> ();
		}

		public void AddTransition (ITransition transition) 
		{
			this.transitions.Add (transition);
		}

		public ITransition GetTransition() 
		{
			foreach (ITransition transition in this.transitions) {
				if (transition.IsApplicable ())
					return transition;
			}
			return null;
		}
	}

}