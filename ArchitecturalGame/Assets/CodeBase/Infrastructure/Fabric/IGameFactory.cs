using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
	public interface IGameFactory
	{
		GameObject CreateHero(GameObject at);

		void CreateHud();
	}
}
