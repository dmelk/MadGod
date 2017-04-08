using UnityEngine;
using Zenject;
using FullmetalKobzar.Core.Dialog;
using FullmetalKobzar.Core.Translation;
using FullmetalKobzar.MadGod.Battle;

public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    public override void InstallBindings()
    {
		GameObject staticGameObject = GameObject.FindWithTag ("StaticObject");
		if (!staticGameObject) {
			staticGameObject = new GameObject ("StaticObject");
			staticGameObject.AddComponent<StaticObject>();
			staticGameObject.tag = "StaticObject";
		}
		StaticObject staticObject = (staticGameObject) ? staticGameObject.GetComponent <StaticObject> () : null;

		IDialogFactory dialogFactory = new DummyDialogFactory ();
		Container.Bind<IDialogFactory> ().FromInstance (dialogFactory);

		Container.Bind<ITranslatorFactory> ().FromInstance (staticObject.translatorFactory);
		Container.Bind<ITranslator> ().FromInstance (staticObject.translator);

		ICharacterFactory characterFactory = new DummyCharacterFactory ();
		Container.Bind<ICharacterFactory> ().FromInstance (characterFactory);
	}
}