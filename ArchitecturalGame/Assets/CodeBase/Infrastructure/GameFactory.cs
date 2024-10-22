using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class GameFactory : IGameFactory
	{
		private readonly IAssetProvider _asset;

		public GameFactory(IAssetProvider asset)
		{
			_asset = asset;
		}

		public GameObject CreateHero(GameObject at) =>
			_asset.Instantiate(AssetPath.HeroPath, at: at.transform.position);

		public void CreateHud() =>
			_asset.Instantiate(AssetPath.HudPath);
	}
}
