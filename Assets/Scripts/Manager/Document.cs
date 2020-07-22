using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreAmountType {
  LOW,
  NORMAL,
  HIGH
}

public class BoughtItem {
  public ItemPickupSo Character { get; private set; }
  public int Count { get; set; }

  public BoughtItem(ItemPickupSo item, int count = 1) {
    Character = item;
    Count = count;
  }

  public bool IsUsedUp() {
    return Count == 0;
  }
}

public class Document : Singleton<Document> {
  #region PrivateField
  [SerializeField]
  private UserSettings user_settings_;
  [SerializeField]
  private OperationData operation_data_;
  private const int LOW_SCORE_MARGIN        = 150;
  private const int NORMAL_SCORE_MARGIN     = 400;
  private CountdownTimer timer_;
  #endregion

  #region PublicFields
  public List<ItemPickupSo> PickupSos;
  public UserSettings UserSettingsInfo { get => user_settings_; }
  public int Level {
    get => operation_data_.Level;
  }

  public int TagetScore { get; private set; }

  public int LevelScore { get; private set; }

  public int HookSpeed { get; private set; }
  public int HookHeavySpeed { get => HookSpeed / 2; }
  public int TotalScore { get; private set; }
  public int ScoreAmount { get; private set; }
  public ScoreAmountType AmountType { get; private set; }
  public int TotalTime { get; private set; }
  public bool IsVictory { get => TotalScore >= TagetScore; }

  public bool IsFinished { get => timer_.IsFire; }
  public bool IsFirstTime { get => operation_data_.IsFirstTime; }

  public int Counter {
    get => (int)timer_.Counter;
  }

  public List<BoughtItem> BuyItems {
    get {
      List<BoughtItem> buyItems = new List<BoughtItem>();
      foreach(KeyValuePair<ItemPickupType, BoughtItem> item in bought_item_dict_) {
        buyItems.Add(item.Value);
      }
      return buyItems;
    }
  }
  private Dictionary<ItemPickupType, BoughtItem> bought_item_dict_;
  #endregion

  void Start() {
    timer_ = GetComponent<CountdownTimer>();
    timer_.Stop();
  }

  public void Init() {
    LoadSetting(user_settings_);
    LoadSetting(operation_data_);
    bought_item_dict_ = new Dictionary<ItemPickupType, BoughtItem>();
    TotalScore = operation_data_.TotalScore;
    LoadBomb();
  }

  public void GoNextLevel() {
    operation_data_.Level++;
    if(bought_item_dict_.ContainsKey(ItemPickupType.CLOCK)) {
      TotalTime = (int)((Random.Range(0, Level) / Level) * 20) + 70;
    } else {
      TotalTime = 60;
    }
    if (bought_item_dict_.ContainsKey(ItemPickupType.POWER)) {
      HookSpeed = 5;
    } else {
      HookSpeed = 3;
    }
    int oldTargetScore = TagetScore;
    TagetScore = ((Level - 1) * 1200) + 800 + Random.Range(0, Level) * 500;
    LevelScore = (TagetScore - oldTargetScore) + Random.Range(800, 2000);
    TotalScore = operation_data_.TotalScore;
    timer_.StartTime = TotalTime;
    timer_.Restart();
  }

  public void NewGame() {
    operation_data_.IsFirstTime = false;
    operation_data_.Level = 1;
    TotalTime = 61;
    HookSpeed = 3;
    TagetScore = 800;
    operation_data_.TotalScore = 0;
    LevelScore = TagetScore + Random.Range(800, 2000);
    timer_.StartTime = TotalTime;
  }

  public void StartTimer() {
    timer_.Restart();
  }

  public void Continue() {
    int oldTargetScore = TagetScore;
    TotalTime = 61;
    TagetScore = ((Level - 1) * 1200) + 800 + Random.Range(0, Level) * 500;
    LevelScore = (TagetScore - oldTargetScore) + Random.Range(800, 2000);
    HookSpeed = 3;
    timer_.StartTime = TotalTime;
  }

  public void UpdateScore() {
    TotalScore += ScoreAmount;
  }

  public void FinishLevel() {
    operation_data_.TotalScore = TotalScore;
    SaveData();
  }

  public void SetScoreAmount(IVictim victim) {
    switch (victim.Tag) {
      case "Rock":
        if (bought_item_dict_.ContainsKey(ItemPickupType.ROCK_BOOK)) {
          ScoreAmount = victim.ScoreAmount * 3;
        } else {
          ScoreAmount = victim.ScoreAmount;
        }
        break;
      case "Diamond":
        if (bought_item_dict_.ContainsKey(ItemPickupType.DIAMOND)) {
          ScoreAmount = victim.ScoreAmount + 100;
        } else {
          ScoreAmount = victim.ScoreAmount;
        }
        break;
      default:
        ScoreAmount = victim.ScoreAmount;
        break;
    }
    if(ScoreAmount <= LOW_SCORE_MARGIN) {
      AmountType = ScoreAmountType.LOW;
    } else if(ScoreAmount <= NORMAL_SCORE_MARGIN) {
      AmountType = ScoreAmountType.NORMAL;
    } else {
      AmountType = ScoreAmountType.HIGH;
    }
  }

  public void ResumeLevel() {
    timer_.Resume();
  }

  public void PauseLevel() {
    timer_.Pause();
    SaveData();
  }

  public void UseItemPickup(ItemPickupSo item) {
    if(bought_item_dict_.ContainsKey(item.Type) 
      && bought_item_dict_[item.Type].Count > 0) {
      bought_item_dict_[item.Type].Count--;
    }
  }

  public void BuyPickupItem(ItemPickupSo item, int amount, int count = 1) {
    if(bought_item_dict_.ContainsKey(item.Type)) {
      bought_item_dict_[item.Type].Count += count;
    } else {
      bought_item_dict_.Add(item.Type, new BoughtItem(item, count));
    }
    TotalScore -= amount;
    SaveData();
  }

  public void Clear() {
    for(int i = 0; i < PickupSos.Count; i++) {
      if(bought_item_dict_.ContainsKey(PickupSos[i].Type) && PickupSos[i].IsOnlyOneLevel) {
        bought_item_dict_.Remove(PickupSos[i].Type);
      }
    }
    bought_item_dict_.Clear();
  }

  public void SaveData() {
    SaveSetting(user_settings_);
    SaveSetting(operation_data_);
  }

  private void LoadSetting<T>(T instance) {
    string settingData = PlayerPrefs.GetString(typeof(T).Name, string.Empty);
    if (settingData != string.Empty) {
      JsonUtility.FromJsonOverwrite(settingData, instance);
    }
  }

  private void SaveSetting<T>(T instance) {
    string settingData = JsonUtility.ToJson(instance);
    PlayerPrefs.SetString(instance.ToString(), settingData);
    PlayerPrefs.Save();
  }

  private void LoadBomb() {
    int bombCount = operation_data_.BombCount;
    if (bombCount > 0) {
      BuyPickupItem(PickupSos.Find(x => x.Type == ItemPickupType.BOMB), 0, bombCount);
    }
  }
}
