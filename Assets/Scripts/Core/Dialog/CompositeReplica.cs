using System.Collections.Generic;

namespace FullmetalKobzar.Core.Dialog {

	public class CompositeReplica : Replica {
		public List<string> replicas { get; private set; }

		public CompositeReplica () {
			this.isPlayerReplica = true;
			this.replicas = new List<string> ();
		}

		public CompositeReplica (string[] replicas) {
			this.isPlayerReplica = true;
			this.replicas = new List<string> (replicas);
		}
	}

}