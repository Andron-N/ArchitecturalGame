using UnityEngine;

namespace CodeBase.Infrastructure
{
	public class GameFactory : IGameFactory
	{
		private const string HeroPath = "Hero/hero";
		private const string HudPath = "Hud/Hud";

		public GameObject CreateHero(GameObject at) =>
			Instantiate(HeroPath, at: at.transform.position);

		public void CreateHud() =>
			Instantiate(HudPath);

		private static GameObject Instantiate(string path, Vector3 at)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}

		private static GameObject Instantiate(string path)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab);
		}
	}
}
