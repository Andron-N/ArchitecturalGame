using CodeBase.CameraLogic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private IPayloadedState<string> _payloadedStateImplementation;

		public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
		}

		public void Enter(string sceneName) =>
			_sceneLoader.Load(sceneName, OnLoaded);

		public void Exit()
		{
		}

		private void OnLoaded()
		{
			GameObject hero = Instantiate("Hero/hero");
			Instantiate("Hud/Hud");

			CameraFollow(hero);
		}

		private void CameraFollow(GameObject hero) =>
			Camera.main.GetComponent<CameraFollow>().Follow(hero);

		private static GameObject Instantiate(string path)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}
	}
}
