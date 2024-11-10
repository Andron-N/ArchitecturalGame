using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		private readonly IAssets _assets;
		private readonly IStaticDataService _staticData;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

		private GameObject _heroGameObject;

		public GameFactory(IAssets assets, IStaticDataService staticData)
		{
			_assets = assets;
			_staticData = staticData;
		}

		public GameObject CreateHero(GameObject at)
		{
			_heroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
			return _heroGameObject;
		}

		public GameObject CreateHud() =>
			InstantiateRegistered(AssetPath.HudPath);

		public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
		{
			MonsterStaticData monsterData = _staticData.ForMonster(typeId);
			GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

			IHealth health = monster.GetComponent<IHealth>();
			health.Current = monsterData.Hp;
			health.Max = monsterData.Hp;

			monster.GetComponent<ActorUI>().Construct(health);
			monster.GetComponent<AgentMoveToHero>().Construct(_heroGameObject.transform);
			monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

			EnemyAttack attack = monster.GetComponent<EnemyAttack>();
			attack.Construct(_heroGameObject.transform);
			attack.Damage = monsterData.Damage;
			attack.AttackCooldown = monsterData.AttackCooldown;
			attack.EffectiveDistance = monsterData.EffectiveDistance;
			attack.Cleavage = monsterData.Cleavage;

			monster.GetComponent<AgentRotateToHero>()?.Construct(_heroGameObject.transform);

			return monster;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		public void Register(ISavedProgressReader progressReader)
		{
			if(progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);

			ProgressReaders.Add(progressReader);
		}

		private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath, at);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegistered(string prefabPath)
		{
			GameObject gameObject = _assets.Instantiate(prefabPath);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private void RegisterProgressWatchers(GameObject gameObject)
		{
			foreach(ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
		}
	}
}
