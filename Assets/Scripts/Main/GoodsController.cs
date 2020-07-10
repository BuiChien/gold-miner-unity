using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsController : GameScript, IVictim {
  public GoodsSo character_;
  public GoodsSo Character {
    set {
      character_ = value;
      GetComponent<SpriteRenderer>().sprite = value.Icon;
      transform.localScale = new Vector3(value.Scale, value.Scale, 0);
    } 
  }

  void Start() {
    RegisterGameEventController();
  }

  void Update() {

  }

  private void OnTriggerEnter2D(Collider2D collision) {
    GameObject gameObject = collision.gameObject;
    IAttacker attacker = gameObject.GetComponent<IAttacker>();
    if (attacker != null) {
      attacker.OnAttach(this);
      BroadcastEvent(new AttachEventArgs(attacker, this));
    }
  }

  public virtual void Death() {
    Destroy(gameObject);
  }

  public virtual void DragAway(Transform target) {
    Quaternion tg = Quaternion.Euler(target.parent.transform.rotation.x,
                                 target.parent.transform.rotation.y,
                                  target.parent.transform.rotation.z * 40);
    transform.rotation = Quaternion.Slerp(transform.rotation, tg, 0.5f);
    transform.position = new Vector3(target.position.x,
                                        target.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 3,
                                        transform.position.z);
  }

  public bool IsHeavy() {
    return character_.IsHeavy;
  }
}
