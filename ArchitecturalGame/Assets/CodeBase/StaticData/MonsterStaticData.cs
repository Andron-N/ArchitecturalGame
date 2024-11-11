using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
	public class MonsterStaticData : ScriptableObject
	{
		public MonsterTypeId MonsterTypeId;

		public int Hp;
		public float Damage;

		[Range(0.1f, 100f)]
		public float AttackCooldown;

		[Range(0.5f, 1f)]
		public float EffectiveDistance;

		[Range(0.5f, 30f)]
		public float MoveSpeed;

		public int MinLoot;
		public int MaxLoot;

		public GameObject Prefab;
	}
}
