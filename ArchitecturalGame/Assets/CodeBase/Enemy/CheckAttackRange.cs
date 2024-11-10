using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAttack))]
	public class CheckAttackRange : MonoBehaviour
	{
		[SerializeField] private EnemyAttack _enemyAttack;
		[SerializeField] private TriggerObserver _triggerObserver;

		private void Start()
		{
			_triggerObserver.TriggerEnter += TriggerEnter;
			_triggerObserver.TriggerExit += TriggerExit;

			_enemyAttack.DisableAttack();
		}

		private void TriggerEnter(Collider obj) =>
			_enemyAttack.EnableAttack();

		private void TriggerExit(Collider obj) =>
			_enemyAttack.DisableAttack();
	}
}
