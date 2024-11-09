using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
	public class EnemyDeath : MonoBehaviour
	{
		[SerializeField] private EnemyAnimator _animator;
		[SerializeField] private EnemyHealth _health;
		[SerializeField] private Follow _follow;
		[SerializeField] private EnemyAttack _attack;
		[SerializeField] private GameObject _deathFx;

		public event Action Happened;

		private void Start() =>
			_health.HealthChanged += HealthChanged;

		private void OnDestroy() =>
			_health.HealthChanged -= HealthChanged;

		private void HealthChanged()
		{
			if(_health.Current <= 0)
				Die();
		}

		private void Die()
		{
			_health.HealthChanged -= HealthChanged;

			DisableComponents();
			_animator.PlayDeath();

			SpawnDeathFx();
			StartCoroutine(DestroyTimer());

			Happened?.Invoke();
		}

		private void DisableComponents()
		{
			_follow.enabled = false;
			_attack.enabled = false;
		}

		private void SpawnDeathFx() =>
			Instantiate(_deathFx, transform.position, Quaternion.identity);

		private IEnumerator DestroyTimer()
		{
			yield return new WaitForSeconds(3);

			Destroy(gameObject);
		}
	}
}
