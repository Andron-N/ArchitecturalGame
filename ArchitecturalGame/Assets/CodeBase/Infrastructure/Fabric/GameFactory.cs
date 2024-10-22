using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
	public class GameFactory : IGameFactory
	{
		private readonly IAssets _asset;

		public GameFactory(IAssets asset)
		{
			_asset = asset;
		}

		public GameObject CreateHero(GameObject at) =>
			_asset.Instantiate(AssetPath.HeroPath, at: at.transform.position);

		public void CreateHud() =>
			_asset.Instantiate(AssetPath.HudPath);
	}
}
