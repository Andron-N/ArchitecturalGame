using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
	public interface IGameFactory : IService
	{
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }

		GameObject CreateHero(GameObject at);

		GameObject CreateHud();

		void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);

		GameObject CreateMonster(MonsterTypeId typeId, Transform parent);

		LootPiece CreateLoot();

		void Cleanup();
	}
}
