using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using System;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(HeroAnimator))]
	public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
	{
		public float Current
		{
			get => _state.CurrentHp;
			set
			{
				if(_state.CurrentHp != value)
				{
					_state.CurrentHp = value;
					HealthChanged?.Invoke();
				}
			}
		}

		public float Max
		{
			get => _state.MaxHp;
			set => _state.MaxHp = value;
		}

		[SerializeField] private HeroAnimator _animator;

		private State _state;

		public event Action HealthChanged;

		public void LoadProgress(PlayerProgress progress)
		{
			_state = progress.HeroState;
			HealthChanged?.Invoke();
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			progress.HeroState.CurrentHp = Current;
			progress.HeroState.MaxHp = Max;
		}

		public void TakeDamage(float damage)
		{
			if(Current <= 0)
				return;

			Current -= damage;
			_animator.PlayHit();
		}
	}
}
