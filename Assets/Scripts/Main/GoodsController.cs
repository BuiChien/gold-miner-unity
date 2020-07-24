using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsController : GameScript, IVictim {
  public GoodsSo character_;
  public int ScoreAmount {
    get => character_.ScoreAmount; 
  }

  public int Id => (int)character_.Type;
  public string Tag => character_.Tag;

  public bool IsHeavy {
    get => character_.IsHeavy;
  }
  private PolygonCollider2D collider_;
  public GoodsSo Character {
    set {
      character_ = value;
      GetComponent<SpriteRenderer>().sprite = value.Icon;
      transform.localScale = new Vector3(value.Scale, value.Scale, 0);
      collider_.SetPath(0, value.Icon.vertices);
    } 
  }

  public Vector3 Position => transform.position;
  public ISpawner Spawner;
  void Awake() {
    collider_ = GetComponent<PolygonCollider2D>();
  }

  void Start() {
    RegisterGameEventController();
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    GameObject gameObject = collision.gameObject;
    IAttacker attacker = gameObject.GetComponent<IAttacker>();
    if (attacker != null) {
      attacker.OnAttach(this);
      BroadcastEvent(new AttachEventArgs(attacker, this));
    }
  }

  public virtual void Death(IAttacker attacker) {
    if(attacker.Name == Bomb.MyName) {
      //TODO:
    }
    if(Spawner != null) {
      Spawner.OnSpawnableDestroy(gameObject);
    }
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
}
