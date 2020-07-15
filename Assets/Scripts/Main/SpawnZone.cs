using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {
  private RectTransform rect_transform_;
  private int layer_mask_;
  public float XMax { get; private set; }
  public float XMin { get; private set; }
  public float YMax { get; private set; }
  public float YMin { get; private set; }


  void Awake() {
    rect_transform_ = GetComponent<RectTransform>();
    layer_mask_ = LayerMask.NameToLayer("Goods") << 1;
    XMax = rect_transform_.rect.xMax;
    XMin = rect_transform_.rect.xMin;
    YMax = rect_transform_.rect.yMax;
    YMin = rect_transform_.rect.yMin;
  }

  public Vector3 SpawnPoint(float radius) {
    Vector3 point = Vector3.zero;
    float startTime = Time.realtimeSinceStartup;
    bool isOk = false;
    while (!isOk) {
      point = new Vector3(Random.Range(XMin + radius, XMax - radius),
          Random.Range(YMin + radius, YMax - radius), 0) 
          + rect_transform_.transform.position;
      isOk = Physics2D.OverlapCircleAll(point, radius, layer_mask_).Length == 0;
      if (Time.realtimeSinceStartup - startTime > 0.5f) {
        point = Vector3.zero;
        break;
      }
    }
    return point;
  }
}
