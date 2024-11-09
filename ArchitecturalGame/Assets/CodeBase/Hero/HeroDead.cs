using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(HeroHealth))]
	public class HeroDead : MonoBehaviour
	{
		[SerializeField] private HeroHealth _health;
		[SerializeField] private HeroMove _move;
		[SerializeField] private HeroAttack _attack;
		[SerializeField] private HeroAnimator _animator;
		[SerializeField] private GameObject _deathFx;

		private bool _isDead;

		private void Start() =>
			_health.HealthChanged += HealthChanged;

		private void OnDestroy() =>
			_health.HealthChanged -= HealthChanged;

		private void HealthChanged()
		{
			if(!_isDead && _health.Current <= 0)
				Die();
		}

		private void Die()
		{
			_isDead = true;
			DisableComponents();
			_animator.PlayDeath();

			SpawnDeathFx();
		}

		private void SpawnDeathFx() =>
			Instantiate(_deathFx, transform.position, Quaternion.identity);

		private void DisableComponents()
		{
			_move.enabled = false;
			_attack.enabled = false;
		}
	}
}
