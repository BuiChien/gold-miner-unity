using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsController : MonoBehaviour, IGoodsBehavior {
  public GoodsSo GoodDefinition;
  [SerializeField]
  private Animator animator_;
  private GameObject follow_subject_ = null;
  void Start() {
    follow_subject_ = null;
  }

  // Update is called once per frame
  void Update() {

  }

  void FixedUpdate() {
    if(follow_subject_ != null) {
      OnFollowTarget();
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    GameObject gameObject = collision.gameObject;
    IAttacker attacker = gameObject.GetComponent<IAttacker>();
    if (attacker != null) {
      attacker.GoodAttacked = this;
    }
  }

  public void SetFollowTarget(GameObject target) {
    follow_subject_ = target;
  }

  protected virtual void OnFollowTarget() {
    
  }

  public virtual void Destroy() {

  }
}
