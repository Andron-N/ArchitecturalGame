using UnityEngine;

namespace CodeBase.StaticData
{
	[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
	public class MonsterStaticData : ScriptableObject
	{
		public MonsterTypeId MonsterTypeId;

		[Range(1, 100)]
		public int Hp;

		[Range(1f, 30f)]
		public float Damage;

		[Range(0.1f, 30f)]
		public float AttackCooldown;

		[Range(0.5f, 1f)]
		public float EffectiveDistance;

		[Range(0.5f, 1f)]
		public float Cleavage;

		[Range(0.5f, 30f)]
		public float MoveSpeed;

		public GameObject Prefab;
	}
}
