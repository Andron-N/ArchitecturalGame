using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
	[RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
	public class HeroAttack : MonoBehaviour, ISavedProgressReader
	{
		private static int _layerMask;

		[SerializeField] private HeroAnimator _heroAnimator;
		[SerializeField] private CharacterController _characterController;

		private IInputService _input;
		private Collider[] _hits = new Collider[3];
		private Stats _stats;

		private void Awake()
		{
			_input = AllServices.Container.Single<IInputService>();

			_layerMask = 1 << LayerMask.NameToLayer("Hittable");
		}

		private void Update()
		{
			if(CanAttack())
				_heroAnimator.PlayAttack();
		}

		private void OnAttack()
		{
			PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1.0f);
			for(int i = 0; i < Hit(); i++)
				_hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
		}

		public void LoadProgress(PlayerProgress progress) =>
			_stats = progress.HeroStats;

		private bool CanAttack() =>
			_input.IsAttackButtonUp() && !_heroAnimator.IsAttacking;

		private int Hit() =>
			Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

		private Vector3 StartPoint() =>
			new Vector3(transform.position.x, _characterController.center.y, transform.position.z);
	}
}
