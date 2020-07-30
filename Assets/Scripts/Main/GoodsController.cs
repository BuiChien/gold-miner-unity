﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsController : GameScript, IVictim {
  internal class GoodState {
    public const int IDLE = 1;
    public const int EXPLODE = 5;
  }
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
  private Animator animator_;
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
    tag = character_.Tag;
    collider_ = GetComponent<PolygonCollider2D>();
    animator_ = GetComponent<Animator>();
  }

  void Start() {
    RegisterGameEventController();
    SetAnimation(GoodState.IDLE);
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
      SetAnimation(GoodState.EXPLODE);
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

  private void SetAnimation(int state) {
    if(animator_.runtimeAnimatorController != null) {
      animator_.SetInteger("State", state);
    }
  }
}
