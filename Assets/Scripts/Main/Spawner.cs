using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner {
  [SerializeField]
  private GoodsSo[] common_goods_list_;
  [SerializeField]
  private GoodsSo[] precious_goods_list_;
  [SerializeField]
  private GameObject goods_prefab_;
  [SerializeField]
  private SpawnZone precious_material_zone_;
  [SerializeField]
  private SpawnZone whole_level_zone_;
  private int total_score_;
  private int level_;

  private int precious_min_score_;
  private int precious_max_score_;
  private int common_min_score_;
  private int common_max_score_;
  void Awake() {
    UpdateMaxMin(common_goods_list_, ref common_max_score_, ref common_min_score_);
    UpdateMaxMin(precious_goods_list_, ref precious_max_score_, ref precious_min_score_);
  }

  public void Spawn(int level, int totalScore) {
    total_score_ = totalScore;
    level_ = level;
    SpawnPreciousZone();
    SpawnCommonZone();
  }

  private void UpdateMaxMin(GoodsSo[] listGoods, ref int max, ref int min) {
    bool isFirst = true;
    foreach (GoodsSo goods in listGoods) {
      if (isFirst) {
        max = goods.ScoreAmount;
        min = goods.ScoreAmount;
      } else {
        if (max < goods.ScoreAmount) {
          max = goods.ScoreAmount;
        }
        if (min > goods.ScoreAmount) {
          min = goods.ScoreAmount;
        }
      }
    }
  }

  private void SpawnPreciousZone() {
    int scoreAmount = Random.Range(precious_min_score_, precious_max_score_);
    int spawnNumber = Random.Range(1, total_score_ / scoreAmount);
    int indexSpawn;
    GameObject spawnObj;
    Vector3 spawnPoint;
    GoodsSo spawnCharacter;
    float radius;
    for (int i = 0; i < spawnNumber; i++) {
      indexSpawn = Random.Range(0, precious_goods_list_.Length);
      spawnCharacter = precious_goods_list_[indexSpawn];
      radius = spawnCharacter.Icon.bounds.extents.magnitude * 1.2f;
      spawnPoint = precious_material_zone_.SpawnPoint(radius);
      if(spawnPoint.Equals(Vector3.zero)) {
        continue;
      }
      spawnObj = Instantiate(goods_prefab_, spawnPoint, Quaternion.identity, precious_material_zone_.transform);
      spawnObj.GetComponent<GoodsController>().Character = spawnCharacter;
      total_score_ -= spawnCharacter.ScoreAmount;
    }
  }

  private void SpawnCommonZone() {
    int indexSpawn;
    GoodsSo spawnCharacter;
    float radius;
    Vector3 spawnPoint;
    GameObject spawnObj;
    while (total_score_ > 0) {
      indexSpawn = Random.Range(0, common_goods_list_.Length);
      spawnCharacter = common_goods_list_[indexSpawn];
      radius = spawnCharacter.Icon.bounds.extents.magnitude * 1.2f;
      spawnPoint = whole_level_zone_.SpawnPoint(radius);
      if(spawnPoint.Equals(Vector3.zero)) {
        continue;
      }
      spawnObj = Instantiate(goods_prefab_, spawnPoint, Quaternion.identity, whole_level_zone_.transform);
      spawnObj.GetComponent<GoodsController>().Character = spawnCharacter;
      radius = spawnObj.GetComponent<PolygonCollider2D>().bounds.extents.magnitude;
      total_score_ -= spawnCharacter.ScoreAmount;
    }
  }
}
