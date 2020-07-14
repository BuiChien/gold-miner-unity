using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {
  public float XMax { get; private set; }
  public float XMin { get; private set; }
  public float YMax { get; private set; }
  public float YMin { get; private set; }

  void Awake() {
    XMin = transform.localPosition.x;
    YMin = transform.localPosition.y;
    XMax = XMin + ((RectTransform)transform).rect.width;
    YMax = YMin + ((RectTransform)transform).rect.height;
  }

  public Vector3 SpawnPoint() {
    Vector3 point = new Vector3(Random.Range(XMin, XMax), Random.Range(YMin, YMax), 0);
    //TODO: Some checks for repeating place
    return point;
  }
}
