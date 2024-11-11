using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
	public class AgentMoveToHero : Follow
	{
		private const float MinimalDistance = 1;

		[SerializeField] private NavMeshAgent _agent;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;

		public void Construct(Transform heroTransform) =>
			_heroTransform = heroTransform;

		private void Update() =>
			SetDestinationForAgent();

		private void SetDestinationForAgent()
		{
			if(_heroTransform && IsHeroNotReached())
				_agent.destination = _heroTransform.position;
		}

		private bool IsHeroNotReached() =>
			_agent.transform.position.SqrMagnitudeTo(_heroTransform.position) >= MinimalDistance;
	}
}
