using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class AbstractState<T> : MonoBehaviour, IState<T> where T : Enum {


	#region IState

	[field: SerializeField]
	public T CurrentState {
		get;
		private set;
	}

	public T LastState {
		get;
		private set;
	}

	#endregion


	#region Actions

	public Action StateChanged;

	#endregion


	#region MonoBehaviour

	public virtual void Awake() {
		EnterState(this.CurrentState);
	}

	public virtual void Update() {
		UpdateState(this.CurrentState);
	}

	#endregion


	#region Abstract Methods

	protected virtual void EnterState(T state) {

	}

	protected virtual void ExitState(T state) {

	}

	protected virtual void UpdateState(T state){

	}

	#endregion


	#region Virtual Methods

	public virtual void SetState(T state) {
		if (!EqualityComparer<T>.Default.Equals(this.CurrentState, state)) {
			this.LastState = state;
			ExitState(this.CurrentState);
			this.CurrentState = state;
			EnterState(this.CurrentState);
			StateChanged?.Invoke();
		}
	}

	#endregion

}
