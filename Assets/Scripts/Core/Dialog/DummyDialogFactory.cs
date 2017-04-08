namespace FullmetalKobzar.Core.Dialog {

	public class DummyDialogFactory : IDialogFactory 
	{

		public IDialog Create (string key) {
			Dialog dialog = new Dialog ();
			dialog.AddReplica ("dragon_hello", new SimpleReplica ("Quo Vadis, странник?"));
			dialog.AddReplica ("dragon_stars", new SimpleReplica ("Твой путь к звездам лежит в другом месте. Здесь ты не пройдешь"));
			dialog.AddReplica ("dragon_not_pass", new SimpleReplica ("Я не дам тебе пройти"));
			dialog.AddReplica ("dragon_think", new SimpleReplica ("Даю тебе шанс одуматься. Развернись и уходи"));
			dialog.AddReplica ("dragon_stop", new SimpleReplica ("Ты неучтив. Я не пропущу тебя"));
			dialog.AddReplica ("dragon_fight", new SimpleReplica ("Посмотрим, так ли остер твой меч, как твой язык", false, Replica.BATTLE_STATE));
			dialog.AddReplica ("dragon_knew", new SimpleReplica ("Я знаю, кто ты, странник. Но я не пропущу тебя."));
			dialog.AddReplica ("dragon_id", new SimpleReplica ("Я - дракон. Я от рождения не боюсь никого и ничего. Твоя маска не имеет власти надо мной."));

			dialog.AddReplica ("player_hello_1", new SimpleReplica ("Ad astra per aspera, дракон", true));
			dialog.AddReplica ("player_hello_2", new SimpleReplica ("С дороги", true));
			dialog.AddReplica ("player_hello_3", new SimpleReplica ("Разве говорящие ящерицы не вымерли? Я сейчас исправлю это недоразумение", true));
			dialog.AddReplica ("player_hello_4", new SimpleReplica ("Я - посланник Безумного Бога", true));
			string[] playerHello = { "player_hello_1", "player_hello_2", "player_hello_3", "player_hello_4" };
			dialog.AddReplica ("player_hello", new CompositeReplica (playerHello));

			dialog.AddReplica ("player_1_1", new SimpleReplica ("И почему же?", true));
			dialog.AddReplica ("player_1_2", new SimpleReplica ("Уйди с моего пути, дракон, иначе мне придется убить тебя.", true));
			dialog.AddReplica ("player_1_3", new SimpleReplica ("Что же, я поищу другую дорогу. Прощай", true, Replica.FINAL_STATE));
			dialog.AddReplica ("player_1_4", new SimpleReplica ("Я принимаю твой вызов", true, Replica.BATTLE_STATE));

			string[] player1 = { "player_1_1", "player_1_2", "player_1_3" };
			dialog.AddReplica ("player_1", new CompositeReplica (player1));
			string[] player11 = { "player_1_2", "player_1_3" };
			dialog.AddReplica ("player_1_d_1", new CompositeReplica (player11));
			string[] player12 = { "player_1_3", "player_1_4" };
			dialog.AddReplica ("player_1_d_2", new CompositeReplica (player12));

			dialog.AddReplica ("player_2_1", new SimpleReplica ("Плевать, я убью тебя", true, Replica.BATTLE_STATE));
			dialog.AddReplica ("player_2_2", new SimpleReplica ("Плевать, я найду другую дорогу.", true, Replica.FINAL_STATE));

			string[] player2 = { "player_2_1", "player_2_2" };
			dialog.AddReplica ("player_2", new CompositeReplica (player2));

			dialog.AddReplica ("player_4_1", new SimpleReplica ("Ты не боишься меня? Не склонишься перед знаком Безумного Бога?", true));
			dialog.AddReplica ("player_4_2", new SimpleReplica ("Тогда прими смерть во славу Безумного Бога.", true, Replica.BATTLE_STATE));
			dialog.AddReplica ("player_4_3", new SimpleReplica ("Я еще вернусь.", true, Replica.FINAL_STATE));

			string[] player4 = { "player_4_1", "player_4_2", "player_4_3" };
			dialog.AddReplica ("player_4", new CompositeReplica (player4));
			string[] player41 = { "player_4_2", "player_4_3" };
			dialog.AddReplica ("player_4_d_1", new CompositeReplica (player41));

			dialog.AddTransition ("01", new Transition ("dragon_hello", "player_hello"));

			dialog.AddTransition ("02", new Transition ("player_hello_1", "dragon_stars"));
			dialog.AddTransition ("03", new Transition ("dragon_stars", "player_1"));
			dialog.AddTransition ("04", new Transition ("player_1_1", "dragon_not_pass"));
			dialog.AddTransition ("05", new Transition ("dragon_not_pass", "player_1_d_1"));
			dialog.AddTransition ("06", new Transition ("player_1_2", "dragon_think"));
			dialog.AddTransition ("07", new Transition ("dragon_think", "player_1_d_2"));

			dialog.AddTransition ("08", new Transition ("player_hello_2", "dragon_stop"));
			dialog.AddTransition ("09", new Transition ("dragon_stop", "player_2"));

			dialog.AddTransition ("10", new Transition ("player_hello_3", "dragon_fight"));

			dialog.AddTransition ("11", new Transition ("player_hello_4", "dragon_knew"));
			dialog.AddTransition ("12", new Transition ("dragon_knew", "player_4"));
			dialog.AddTransition ("13", new Transition ("player_4_1", "dragon_id"));
			dialog.AddTransition ("14", new Transition ("dragon_id", "player_4_d_1"));

			dialog.SetFirstReplica ("dragon_hello");	

			return dialog;
		}

	}
}

