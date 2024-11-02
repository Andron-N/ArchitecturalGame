using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
	public interface IGameFactory : IService
	{
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }

		GameObject CreateHero(GameObject at);

		void CreateHud();

		void Cleanup();
	}
}
