using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : GoodsController {
  private float run_speed_ = 2f;
  private int run_direction_ = 1;
  private bool IsVisible {
    get => gameObject.GetComponent<Renderer>().isVisible;
  }

  protected override void Start() {
    RegisterGameEventController();
    SetAnimation(GoodState.RUN_OR_BLINK);
    animator_.speed = run_speed_;
  }

  private void Update() {
    MonsterRunning();
  }

  private void MonsterRunning() {
    if(state_ == GoodState.RUN_OR_BLINK) {
      if (!IsVisible) {
        run_direction_ = -run_direction_;
      }
      Vector3 temp = transform.position;
      temp.x += run_direction_ * run_speed_ * UnityEngine.Time.deltaTime;
      transform.position = temp;

      Vector3 scale = transform.localScale;
      scale.x = -run_direction_;
      transform.localScale = scale;
    }
  }

  protected override IEnumerator DoAnimation() {
    yield return new WaitForSeconds(0.5f);
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collision) {
    GameObject gameObject = collision.gameObject;
    IAttacker attacker = gameObject.GetComponent<IAttacker>();
    if (attacker != null) {
      if(attacker.OnAttach(this)) {
        BroadcastEvent(new AttachEventArgs(attacker, this));
        if (attacker.Name.Equals(Hook.MyName)) {
          SetAnimation(GoodState.IDLE);
        }
      }
    } else {
      run_direction_ = -run_direction_;
    }
  }

  public override void Death(IAttacker attacker) {
    base.Death(attacker);
    if(state_.Equals(GoodState.EXPLODE)) {
      StartCoroutine(DoAnimation());
    }
  }
}
