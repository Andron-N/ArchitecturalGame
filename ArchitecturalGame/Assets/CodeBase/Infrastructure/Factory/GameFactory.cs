using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
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
		private readonly IRandomService _randomService;
		private readonly IPersistentProgressService _progressService;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

		private GameObject _heroGameObject;

		public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService)
		{
			_assets = assets;
			_staticData = staticData;
			_randomService = randomService;
			_progressService = progressService;
		}

		public void Cleanup()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		public GameObject CreateHero(GameObject at)
		{
			_heroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
			return _heroGameObject;
		}

		public GameObject CreateHud()
		{
			GameObject hud = InstantiateRegistered(AssetPath.HudPath);
			hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
			return hud;
		}

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

			LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
			lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
			lootSpawner.Construct(this, _randomService);

			EnemyAttack attack = monster.GetComponent<EnemyAttack>();
			attack.Construct(_heroGameObject.transform);
			attack.Damage = monsterData.Damage;
			attack.AttackCooldown = monsterData.AttackCooldown;
			attack.EffectiveDistance = monsterData.EffectiveDistance;

			monster.GetComponent<AgentRotateToHero>()?.Construct(_heroGameObject.transform);

			return monster;
		}

		public LootPiece CreateLoot()
		{
			LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();

			lootPiece.Construct(_progressService.Progress.WorldData);

			return lootPiece;
		}

		public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
		{
			SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at).GetComponent<SpawnPoint>();

			spawner.Construct(this);
			spawner.Id = spawnerId;
			spawner.MonsterTypeId = monsterTypeId;
		}

		private void Register(ISavedProgressReader progressReader)
		{
			if(progressReader is ISavedProgress progressPiece)
				if(!ProgressWriters.Contains(progressPiece))
					ProgressWriters.Add(progressPiece);

			if(!ProgressReaders.Contains(progressReader))
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
