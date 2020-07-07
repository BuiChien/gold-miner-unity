using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HookArea : GameScript, IPointerUpHandler, IPointerDownHandler {
  public void OnPointerDown(PointerEventData eventData) {
    NotifyEvent(new GameEventArgs(EventNames.HOOK_AREA_TOUCH));
  }

  public void OnPointerUp(PointerEventData eventData) {

  }
}
