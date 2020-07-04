using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : GameScript {

  [SerializeField]
  private float BootingTimeInSecond = 1;
  Document document_;
  void Start() {
    document_ = Document.Instance;
    StartCoroutine(LoadData());
  }

  private IEnumerator LoadData() {
    document_.Init();
    yield return new WaitForSeconds(BootingTimeInSecond);
    NotifyEvent(new GameEventArgs(EventNames.STARTUP_SUCCESS));
    yield return null;
  }
}
