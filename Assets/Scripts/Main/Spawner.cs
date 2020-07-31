using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AnimatorFactory))]
public class Spawner : MonoBehaviour, ISpawner {
  public int MinLevelSpawnMonster = 10;
  [SerializeField]
  private GoodsSo[] common_goods_list_;
  [SerializeField]
  private GoodsSo[] precious_goods_list_;
  [SerializeField]
  private GoodsSo[] monster_list_;
  [SerializeField]
  private GameObject goods_prefab_;
  [SerializeField]
  private SpawnZone precious_material_zone_;
  [SerializeField]
  private SpawnZone whole_level_zone_;
  private int total_score_;
  private int level_;
  private List<GameObject> list_spawnable_;
  private int precious_min_score_;
  private int precious_max_score_;
  private int common_min_score_;
  private int common_max_score_;
  private int monster_min_score_;
  private int monster_max_score_;
  private AnimatorFactory animator_factory_;

  public int Count => list_spawnable_.Count;

  void Awake() {
    UpdateMaxMin(common_goods_list_, ref common_max_score_, ref common_min_score_);
    UpdateMaxMin(precious_goods_list_, ref precious_max_score_, ref precious_min_score_);
    UpdateMaxMin(monster_list_, ref monster_max_score_, ref monster_min_score_);
    animator_factory_ = GetComponent<AnimatorFactory>();
    list_spawnable_ = new List<GameObject>();
  }

  public void Spawn(int level, int totalScore) {
    total_score_ = totalScore;
    level_ = level;
    SpawnPreciousZone();
    SpawnMonster();
    SpawnCommonZone();
  }

  private void UpdateMaxMin(GoodsSo[] listGoods, ref int max, ref int min) {
    bool isFirst = true;
    int compareValue;
    foreach (GoodsSo goods in listGoods) {
      if (isFirst) {
        max = goods.MaxAmount > goods.ScoreAmount ? goods.MaxAmount : goods.ScoreAmount;
        min = goods.MaxAmount > goods.ScoreAmount ? goods.MaxAmount : goods.ScoreAmount;
      } else {
        compareValue = goods.MaxAmount > goods.ScoreAmount ? goods.MaxAmount : goods.ScoreAmount;
        if (max < compareValue) {
          max = compareValue;
        }
        if (min > compareValue) {
          min = compareValue;
        }
      }
    }
  }

  private void SpawnPreciousZone() {
    int scoreAmount = Random.Range(precious_min_score_, precious_max_score_);
    int spawnNumber;
    if (level_ <= MinLevelSpawnMonster) {
      spawnNumber = total_score_ / scoreAmount;
    } else {
      spawnNumber = Random.Range(1, total_score_ / scoreAmount);
    }
    int indexSpawn;
    GameObject spawnObj;
    Vector3 spawnPoint;
    GoodsSo spawnCharacter;
    GoodsController controller;
    float radius;
    for (int i = 0; i < spawnNumber; i++) {
      indexSpawn = Random.Range(0, precious_goods_list_.Length);
      spawnCharacter = precious_goods_list_[indexSpawn];
      radius = spawnCharacter.Icon.bounds.extents.magnitude * spawnCharacter.Scale;
      spawnPoint = precious_material_zone_.SpawnPoint(radius);
      if(spawnPoint.Equals(Vector3.zero)) {
        continue;
      }
      spawnObj = Instantiate(goods_prefab_, spawnPoint, Quaternion.identity, precious_material_zone_.transform);
      controller = spawnObj.AddComponent<GoodsController>();
      controller.Spawner = this;
      controller.Character = spawnCharacter;
      spawnObj.GetComponent<Animator>().runtimeAnimatorController = animator_factory_.Create(spawnObj.tag);
      list_spawnable_.Add(spawnObj);
      total_score_ -= spawnCharacter.ScoreAmount;
    }
  }

  private void SpawnCommonZone() {
    int indexSpawn;
    GoodsSo spawnCharacter;
    float radius;
    Vector3 spawnPoint;
    GameObject spawnObj;
    GoodsController controller;
    while (total_score_ > 0) {
      indexSpawn = Random.Range(0, common_goods_list_.Length);
      spawnCharacter = common_goods_list_[indexSpawn];
      radius = spawnCharacter.Icon.bounds.extents.magnitude * spawnCharacter.Scale;
      spawnPoint = whole_level_zone_.SpawnPoint(radius);
      if(spawnPoint.Equals(Vector3.zero)) {
        continue;
      }
      spawnObj = Instantiate(goods_prefab_, spawnPoint, Quaternion.identity, whole_level_zone_.transform);
      controller = spawnObj.AddComponent<GoodsController>();
      controller.Spawner = this;
      controller.Character = spawnCharacter;
      spawnObj.GetComponent<Animator>().runtimeAnimatorController = animator_factory_.Create(spawnObj.tag);
      list_spawnable_.Add(spawnObj);
      total_score_ -= spawnCharacter.ScoreAmount;
    }
  }

  private void SpawnMonster() {
    if (level_ <= MinLevelSpawnMonster) {
      return;
    }
    int scoreAmount = Random.Range(monster_min_score_, monster_max_score_);
    int spawnNumber = Random.Range(1, total_score_ / scoreAmount);
    int indexSpawn;
    GameObject spawnObj;
    Vector3 spawnPoint;
    GoodsSo spawnCharacter;
    GoodsController controller;
    float radius;
    for (int i = 0; i < spawnNumber; i++) {
      indexSpawn = Random.Range(0, monster_list_.Length);
      spawnCharacter = monster_list_[indexSpawn];
      radius = spawnCharacter.Icon.bounds.extents.magnitude * spawnCharacter.Scale;
      spawnPoint = whole_level_zone_.SpawnPoint(radius);
      if (spawnPoint.Equals(Vector3.zero)) {
        continue;
      }
      spawnObj = Instantiate(goods_prefab_, spawnPoint, Quaternion.identity, whole_level_zone_.transform);
      controller = spawnObj.AddComponent<MonsterController>();
      controller.Spawner = this;
      controller.Character = spawnCharacter;
      spawnObj.GetComponent<Animator>().runtimeAnimatorController = animator_factory_.Create(spawnObj.tag);
      list_spawnable_.Add(spawnObj);
      total_score_ -= spawnCharacter.ScoreAmount;
    }
  }

  public void OnSpawnableDestroy(GameObject obj) {
    if(list_spawnable_.Contains(obj)) {
      list_spawnable_.Remove(obj);
    }
  }
}
