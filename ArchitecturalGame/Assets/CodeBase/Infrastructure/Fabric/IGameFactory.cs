using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
	public interface IGameFactory : IService
	{
		GameObject CreateHero(GameObject at);

		void CreateHud();
	}
}
