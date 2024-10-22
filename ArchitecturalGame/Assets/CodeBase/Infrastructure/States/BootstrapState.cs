using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";

		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
		{
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
		}

		public void Enter()
		{
			RegisterServices();
			_sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
		}

		private void EnterLoadLevel()
		{
			_stateMachine.Enter<LoadLevelState, string>("Main");
		}

		public void Exit()
		{
		}

		private void RegisterServices()
		{
			AllServices.Container.RegisterSingle<IInputService>(InputService());
			AllServices.Container.RegisterSingle<IGameFactory>(new GameFactory(AllServices.Container.Single<IAssets>()));
		}

		private static IInputService InputService()
		{
			if(Application.isEditor)
				return new StandaloneInputService();
			else
				return new MobileInputService();
		}
	}
}
