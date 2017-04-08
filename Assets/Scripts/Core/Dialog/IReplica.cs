namespace FullmetalKobzar.Core.Dialog {

	public interface IReplica {
		bool IsPlayerReplica ();

		bool IsFinal ();

		bool IsBattle ();

		bool IsApplicable ();
	}

}
