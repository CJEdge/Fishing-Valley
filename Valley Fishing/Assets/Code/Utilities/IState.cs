using System;

public interface IState<T> where T : Enum {
	T CurrentState {
		get;
	}

	public void SetState(T state);
}
