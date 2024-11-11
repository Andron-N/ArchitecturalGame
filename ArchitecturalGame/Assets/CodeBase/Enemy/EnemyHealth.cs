using CodeBase.Logic;
using System;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class EnemyHealth : MonoBehaviour, IHealth
	{
		public float Current
		{
			get => _current;
			set => _current = value;
		}

		public float Max
		{
			get => _max;
			set => _max = value;
		}

		[SerializeField] private EnemyAnimator _animator;
		[SerializeField] private float _current;
		[SerializeField] private float _max;

		public event Action HealthChanged;

		public void TakeDamage(float damage)
		{
			_current -= damage;
			_animator.PlayHit();

			HealthChanged?.Invoke();
		}
	}
}
