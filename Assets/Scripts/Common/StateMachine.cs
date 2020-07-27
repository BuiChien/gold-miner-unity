using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEntity {
  public int Id { get; private set; }
  public Action Enter { get; private set; }
  public Action<GameEventArgs> Update { get; private set; }
  public StateEntity(int id, Action enter, Action<GameEventArgs> update) {
    Id = id;
    Enter = enter;
    Update = update;
  }
}

public class StateMachine {
  private Dictionary<int, StateEntity> state_dict_ = new Dictionary<int, StateEntity>();
  private StateEntity current_state_;

  public int StateId {
    get { 
      return current_state_ != null ? current_state_.Id : -1; 
    } 
  }
  public void AddState(int id, Action enter, Action<GameEventArgs> update) {
    if(state_dict_.ContainsKey(id)) {
      throw new ArgumentException();
    }
    StateEntity state = new StateEntity(id, enter, update);
    state_dict_.Add(id, state);
  }

  public void ProcessEvent(GameEventArgs gameEvent) {
    if(current_state_ != null) {
      current_state_.Update(gameEvent);
    }
  }

  public void ChangeState(int Id) {
    if(state_dict_.ContainsKey(Id)) {
      current_state_ = state_dict_[Id];
      current_state_.Enter();
    } else {
      throw new ArgumentException();
    }
  }
}
