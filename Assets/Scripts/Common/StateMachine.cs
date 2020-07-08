using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEntity {
  public string Name { get; private set; }
  public Action Enter { get; private set; }
  public Action<GameEventArgs> Update { get; private set; }
  public StateEntity(string name, Action enter, Action<GameEventArgs> update) {
    Name = name;
    Enter = enter;
    Update = update;
  }
}

public class StateMachine {
  private Dictionary<string, StateEntity> state_dict_ = new Dictionary<string, StateEntity>();
  private StateEntity current_state_;

  public string StateName {
    get { 
      return current_state_ != null ? current_state_.Name : string.Empty; 
    } 
  }
  public void AddState(string name, Action enter, Action<GameEventArgs> update) {
    if(state_dict_.ContainsKey(name)) {
      throw new ArgumentException();
    }
    StateEntity state = new StateEntity(name, enter, update);
    state_dict_.Add(name, state);
  }

  public void ProcessEvent(GameEventArgs gameEvent) {
    if(current_state_ != null) {
      current_state_.Update(gameEvent);
    }
  }

  public void ChangeState(string name) {
    if(state_dict_.ContainsKey(name)) {
      current_state_ = state_dict_[name];
      current_state_.Enter();
    } else {
      throw new ArgumentException();
    }
  }
}
