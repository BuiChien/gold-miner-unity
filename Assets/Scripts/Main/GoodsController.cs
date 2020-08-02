using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsController : GameScript, IVictim {
  protected class GoodState {
    public const int IDLE = 1;
    public const int RUN_OR_BLINK = 2;
    public const int EXPLODE = 5;
  }
  public GoodsSo character_;
  private int score_amount_;
  public int ScoreAmount {
    get => score_amount_; 
  }

  public int Id => (int)character_.Type;
  public string Tag => character_.Tag;

  public bool IsHeavy {
    get => character_.IsHeavy;
  }
  protected PolygonCollider2D collider_;
  protected Animator animator_;
  public GoodsSo Character {
    set {
      character_ = value;
      GetComponent<SpriteRenderer>().sprite = value.Icon;
      transform.localScale = new Vector3(value.Scale, value.Scale, 0);
      collider_.SetPath(0, value.Icon.vertices);
      if(!string.IsNullOrEmpty(character_.Tag)) {
        gameObject.tag = character_.Tag;
      }
      if(character_.MaxAmount > character_.ScoreAmount) {
        score_amount_ = Random.Range(character_.ScoreAmount, character_.MaxAmount);
      } else {
        score_amount_ = character_.ScoreAmount;
      }
    }
  }

  public Vector3 Position => transform.position;
  public ISpawner Spawner;
  protected int state_;
  void Awake() {
    collider_ = GetComponent<PolygonCollider2D>();
    animator_ = GetComponent<Animator>();
  }

  protected virtual void Start() {
    RegisterGameEventController();
    SetAnimation(GoodState.IDLE);
    StartCoroutine(DoAnimation());
  }

  protected virtual IEnumerator DoAnimation() {
    while(true) {
      if(state_ == GoodState.EXPLODE) {
        yield return new WaitForSeconds(0.15f);
        break;
      } else if(state_ == GoodState.IDLE) {
        SetAnimation(GoodState.RUN_OR_BLINK);
        yield return new WaitForSeconds(Random.Range(1, 3));
      } else if(state_ == GoodState.RUN_OR_BLINK) {
        SetAnimation(GoodState.IDLE);
        yield return new WaitForSeconds(Random.Range(2, 5));
      }
    }
    Destroy(gameObject);
  }

  protected virtual void OnTriggerEnter2D(Collider2D collision) {
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
    } else {
      Destroy(gameObject);
    }
    if(Spawner != null) {
      Spawner.OnSpawnableDestroy(gameObject);
    }
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

  protected void SetAnimation(int state) {
    state_ = state;
    if (animator_.runtimeAnimatorController != null) {
      animator_.SetInteger("State", state);
    }
  }
}
