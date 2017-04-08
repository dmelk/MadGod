namespace FullmetalKobzar.Core.Dialog {

	public interface IDialog {
		void Start ();

		void SetFirstReplica (string key);

		void AddReplica (string key, IReplica replica);

		void AddTransition (string key, ITransition transition);

		IReplica GetReplica (string key);

		IReplica GetCurrentReplica ();

		void SetCurrentReplica (string key);
	}

}
