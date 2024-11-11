using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
	public class LootPiece : MonoBehaviour, ISavedProgress
	{
		private const float DelayBeforeDestroying = 1.5f;

		[SerializeField] private GameObject _skull;
		[SerializeField] private GameObject _pickupFxPrefab;
		[SerializeField] private TextMeshPro _lootText;
		[SerializeField] private GameObject _pickupPopup;

		private Loot _loot;
		private WorldData _worldData;

		private string _id;
		private bool _pickedUp;
		private bool _loadedFromProgress;

		public void Construct(WorldData worldData) =>
			_worldData = worldData;

		public void Initialize(Loot loot) =>
			_loot = loot;

		private void Start()
		{
			if(!_loadedFromProgress)
				_id = GetComponent<UniqueId>().Id;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_pickedUp)
			{
				_pickedUp = true;
				PickUp();
			}
		}

		public void LoadProgress(PlayerProgress progress)
		{
			_id = GetComponent<UniqueId>().Id;

			LootPieceData data = progress.WorldData.LootData.LootPiecesOnScene.Dictionary[_id];
			Initialize(data.Loot);
			transform.position = data.Position.AsUnityVector();

			_loadedFromProgress = true;
		}

		public void UpdateProgress(PlayerProgress progress)
		{
			if(_pickedUp)
				return;

			LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

			if(!lootPiecesOnScene.Dictionary.ContainsKey(_id))
				lootPiecesOnScene.Dictionary.Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
		}

		private void PickUp()
		{
			UpdateWorldData();
			HideSkull();
			PlayPickupFx();
			ShowText();

			Destroy(gameObject, DelayBeforeDestroying);
		}

		private void UpdateWorldData()
		{
			UpdateCollectedLootAmount();
			RemoveLootPieceFromSavedPieces();
		}

		private void UpdateCollectedLootAmount() =>
			_worldData.LootData.Collect(_loot);

		private void RemoveLootPieceFromSavedPieces()
		{
			LootPieceDataDictionary savedLootPieces = _worldData.LootData.LootPiecesOnScene;

			if(savedLootPieces.Dictionary.ContainsKey(_id))
				savedLootPieces.Dictionary.Remove(_id);
		}

		private void HideSkull() =>
			_skull.SetActive(false);

		private void PlayPickupFx() =>
			Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);

		private void ShowText()
		{
			_lootText.text = $"{_loot.Value}";
			_pickupPopup.SetActive(true);
		}
	}
}
