namespace FullmetalKobzar.Core.Dialog {

	public interface ITransition {
		string GetFromReplicaKey (); 

		string GetToReplicaKey (); 

		bool IsApplicable ();
	}

}
