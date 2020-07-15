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
  public float Radius { get; private set; }
  
  void Awake() {
    rect_transform_ = GetComponent<RectTransform>();
    layer_mask_ = LayerMask.NameToLayer("Goods") << 1;
    XMax = rect_transform_.rect.xMax;
    XMin = rect_transform_.rect.xMin;
    YMax = rect_transform_.rect.yMax;
    YMin = rect_transform_.rect.yMin;
    Radius = Mathf.Sqrt(Mathf.Pow(rect_transform_.rect.height, 2) + Mathf.Pow(rect_transform_.rect.width, 2)) / 2f;
  }

  public Vector3 SpawnPoint(float radius) {
    Vector3 point = Vector3.zero;
    int safetyNet = 0;
    while (safetyNet < 50) {
      point = new Vector3(Random.Range(XMin + radius, XMax - radius),
          Random.Range(YMin + radius, YMax - radius), 0) 
          + transform.position;
      if(IsSpawnOverlap(point, radius)) {
        break;
      }
      safetyNet++;
    }
    if (safetyNet >= 50) {
      point = Vector3.zero;
    }
    return point;
  }

  private bool IsSpawnOverlap(Vector3 spawnPos, float radius) {
    bool isOk = true;
    float width, hight, leftExtent, rightExtent, upperExtent, lowerExtent;
    Vector3 centerPoint;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, Radius, ~layer_mask_);
    for(int i=0; i < colliders.Length; i++) {
      centerPoint = colliders[i].bounds.center;
      width = colliders[i].bounds.extents.x;
      hight = colliders[i].bounds.extents.y;
      leftExtent = centerPoint.x - width;
      rightExtent = centerPoint.x + width;
      upperExtent = centerPoint.y + hight;
      lowerExtent = centerPoint.y - hight;
      if(spawnPos.x + radius >= leftExtent && 
        spawnPos.x - radius <= rightExtent &&
        spawnPos.y + radius >= lowerExtent &&
        spawnPos.y - radius <= upperExtent) {
        isOk = false;
        break;
      }
    }
    return isOk;
  }
}
