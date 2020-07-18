using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : GameScript {
  void Start() {
    StartCoroutine(LoadData());
  }

  private IEnumerator LoadData() {
    Document.Instance.Init();
    yield return new WaitForSeconds(1f);
    NotifyEvent(new GameEventArgs(EventNames.STARTUP_SUCCESS));
    yield return null;
  }
}
