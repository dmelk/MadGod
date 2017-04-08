using System;
using System.Collections.Generic;

namespace FullmetalKobzar.MadGod.Battle
{
	public class BattleField
	{
		public Character[] leftSide;

		public Character[] rightSide;

		public int turn;

		public Character[] GetPossibleTargets(Character owner, bool left, TargetType type)
		{
			List<Character> targets = new List<Character> ();
			if (type == TargetType.SELF) {
				targets.Add (owner);
				return targets.ToArray ();
			}
			Character[] allFriends = (left) ? this.leftSide : this.rightSide;
			if (type == TargetType.FRIEND) {
				for (var i = 0; i < allFriends.Length; i++) {
					if (!allFriends [i].IsDead ())
						targets.Add (allFriends [i]);
				}
				return targets.ToArray ();
			}
			Character[] allEnemies = (left) ? this.rightSide : this.leftSide;
			if (type == TargetType.ENEMY_ALL) {
				for (var i = 0; i < allEnemies.Length; i++) {
					if (!allEnemies [i].IsDead ())
						targets.Add (allEnemies [i]);
				}
				return targets.ToArray ();
			}
			bool[] firstLine = new bool[2] { false, false };
			// get enemies from first line
			for (var i = 0; i < allEnemies.Length; i++) {
				if (!allEnemies[i].IsDead () && allEnemies [i].position.x == 0) {
					firstLine [(int)allEnemies [i].position.y] = true;
					targets.Add (allEnemies [i]);
				}
			}
			// get enemies from second line, if first line is empty
			for (var i = 0; i < allEnemies.Length; i++) {
				if (!allEnemies[i].IsDead () && allEnemies [i].position.x == 1 && !firstLine[(int)allEnemies [i].position.y]) {
					targets.Add (allEnemies [i]);
				}
			}
			return targets.ToArray ();
		}
	}
}

