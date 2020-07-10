using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsSpawner : MonoBehaviour, ISpawner {
  [SerializeField]
  private GoodsSo[] list_goods_;
  [SerializeField]
  private GameObject goods_prefab_;

  public void Spawn(int level) {
    GameObject child = Instantiate(goods_prefab_, new Vector3(2.0F, 0, 0), Quaternion.identity);
    child.GetComponent<GoodsController>().Character = list_goods_[0];
  }

  public void Clear() {

  }
}
