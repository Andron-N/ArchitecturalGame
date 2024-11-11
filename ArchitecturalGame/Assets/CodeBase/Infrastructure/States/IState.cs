namespace CodeBase.Infrastructure.States
{
	public interface IState : IExcitableState
	{
		void Enter();
	}

	public interface IPayloadedState<TPayload> : IExcitableState
	{
		void Enter(TPayload payload);
	}

	public interface IExcitableState
	{
		void Exit();
	}
}
