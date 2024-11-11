using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using System.Linq;
using UnityEngine;

namespace CodeBase.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class EnemyAttack : MonoBehaviour
	{
		public float AttackCooldown = 3f;
		public float Cleavage = 0.5f;
		public float EffectiveDistance = 0.5f;
		public float Damage = 10;

		[SerializeField] private EnemyAnimator _animator;

		private IGameFactory _factory;
		private Transform _heroTransform;
		private float _currentAttackCooldown;
		private bool _isAttacking;
		private int _layerMask;
		private Collider[] _hits = new Collider[1];
		private bool _isActiveAttack;

		public void Construct(Transform heroTransform) =>
			_heroTransform = heroTransform;

		private void Awake() =>
			_layerMask = 1 << LayerMask.NameToLayer("Player");

		private void Update()
		{
			UpdateCooldown();

			if(CanAttack())
				StartAttack();
		}

		private void OnAttack()
		{
			if(Hit(out Collider hit))
			{
				//PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
				hit.transform.GetComponent<HeroHealth>().TakeDamage(Damage);
			}
		}

		private void OnAttackEnded()
		{
			_currentAttackCooldown = AttackCooldown;
			_isAttacking = false;
		}

		public void EnableAttack() =>
			_isActiveAttack = true;

		public void DisableAttack() =>
			_isActiveAttack = false;

		private bool Hit(out Collider hit)
		{
			int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

			hit = _hits.FirstOrDefault();

			return hitCount > 0;
		}

		private Vector3 StartPoint() =>
			new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

		private void UpdateCooldown()
		{
			if(!CooldownIsUp())
				_currentAttackCooldown -= Time.deltaTime;
		}

		private bool CooldownIsUp() =>
			_currentAttackCooldown <= 0;

		private bool CanAttack() =>
			_isActiveAttack && !_isAttacking && CooldownIsUp();

		private void StartAttack()
		{
			transform.LookAt(_heroTransform);
			_animator.PlayAttack();
			_isAttacking = true;
		}
	}
}
