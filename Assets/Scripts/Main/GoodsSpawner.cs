using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsSpawner : MonoBehaviour, ISpawner {
  [SerializeField]
  private GoodsSo[] list_goods_;
  [SerializeField]
  private GameObject goods_prefab_;
  public List<GameObject> list_goods_spawned_;

  public void Spawn(int level) {
    GameObject child = Instantiate(goods_prefab_, new Vector3(2.0F, 0, 0), Quaternion.identity);
    child.GetComponent<SpriteRenderer>().sprite = list_goods_[0].Icon;
    child.transform.localScale = new Vector3(list_goods_[0].Scale, list_goods_[0].Scale, 0);
    list_goods_spawned_.Add(child);
  }

  public void Clear() {

  }
}
