using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero.Move
{
	public class HeroMove : MonoBehaviour
	{
		public CharacterController CharacterController;

		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;

		private void Awake()
		{
			_inputService = Game.InputService;
		}

		private void Update()
		{
			Vector3 movementVector = Vector3.zero;

			if(_inputService.Axis.sqrMagnitude > Constants.Epsilon)
			{
				movementVector = Camera.main.transform.TransformDirection(_inputService.Axis);
				movementVector.y = 0;
				movementVector.Normalize();

				transform.forward = movementVector;
			}

			movementVector += Physics.gravity;

			CharacterController.Move(movementVector * (_movementSpeed * Time.deltaTime));
		}



	}
}
