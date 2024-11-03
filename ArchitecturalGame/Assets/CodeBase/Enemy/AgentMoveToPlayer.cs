﻿using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
	public class AgentMoveToPlayer : MonoBehaviour
	{
		private const float MinimalDistance = 1;

		[SerializeField] private NavMeshAgent _agent;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;

		private void Start()
		{
			_gameFactory = AllServices.Container.Single<IGameFactory>();

			if(_gameFactory.HeroGameObject != null)
				InstantiateHeroTransform();
			else
				_gameFactory.HeroCreated += HeroCreated;
		}

		private void Update()
		{
			if(Initialized() && HeroNotReached())
				_agent.destination = _heroTransform.position;

		}

		private void InstantiateHeroTransform() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;

		private void HeroCreated() =>
			InstantiateHeroTransform();

		private bool Initialized() =>
			_heroTransform != null;

		private bool HeroNotReached() =>
			Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;
	}
}
