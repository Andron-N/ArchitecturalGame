using CodeBase.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private const string MonsterPathFromResources = "StaticData/Monsters";
		private const string LevelsPathFromResources = "StaticData/Levels";

		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
		private Dictionary<string, LevelStaticData> _levels;

		public void LoadMonsters()
		{
			_monsters = Resources.LoadAll<MonsterStaticData>(MonsterPathFromResources).ToDictionary(x => x.MonsterTypeId, x => x);

			_levels = Resources.LoadAll<LevelStaticData>(LevelsPathFromResources).ToDictionary(x => x.LevelKey, x => x);
		}

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;

		public LevelStaticData ForLevel(string sceneKey) =>
			_levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;
	}
}
