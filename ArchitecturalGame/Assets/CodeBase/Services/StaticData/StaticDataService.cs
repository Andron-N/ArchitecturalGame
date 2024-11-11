using CodeBase.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private const string MonsterPathFromResources = "StaticData/Monsters";

		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

		public void LoadMonsters() =>
			_monsters = Resources.LoadAll<MonsterStaticData>(MonsterPathFromResources).ToDictionary(x => x.MonsterTypeId, x => x);

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;
	}
}
