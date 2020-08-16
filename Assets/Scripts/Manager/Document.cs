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

public enum GIFT_TYPE {
  POWER,
  SCORE_FLY,
  BOOM_FLY,
  NONE
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
  public const int HOOK_SPEED_NORMAL      = 3;
  public const int HOOK_SPEED_WITH_POWER  = 5;
  public List<ItemPickupSo> PickupSos;
  public int Level {
    get => operation_data_.Level;
  }
  public int TagetScore { get; private set; }
  public int LevelScore { get; private set; }
  public float HookSpeed {
    get => bought_item_dict_.ContainsKey(ItemPickupType.POWER) ? HOOK_SPEED_WITH_POWER : HOOK_SPEED_NORMAL;
  }
  public float HookHeavySpeed { get => HookSpeed / 2; }
  public int TotalScore { get; private set; }
  public int ScoreAmount { get; private set; }
  public GIFT_TYPE Gift { get; private set; }
  public ScoreAmountType AmountType { get; private set; }
  public int TotalTime { get; private set; }
  public bool IsVictory { get => TotalScore >= TagetScore; }

  public bool IsFinished { get => timer_.IsFire; }
  public bool IsFirstTime { get => operation_data_.IsFirstTime; }

  public int Counter {
    get => (int)timer_.Counter;
  }

  public bool MusicEnable {
    get => user_settings_.MusicEnable;
    set {
      user_settings_.MusicEnable = value;
      SaveUserSettings();
    }
  }

  public bool SoundEnable {
    get => user_settings_.SoundEnable;
    set {
      user_settings_.SoundEnable = value;
      SaveUserSettings();
    }
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
    user_settings_.name = "UserSettings";
    operation_data_.name = "OperationSettings";
    LoadSetting(user_settings_, user_settings_.name);
    LoadSetting(operation_data_, operation_data_.name);
    bought_item_dict_ = new Dictionary<ItemPickupType, BoughtItem>();
    TotalScore = operation_data_.TotalScore;
    LoadBomb();
  }

  public void GoNextLevel() {
    if(bought_item_dict_.ContainsKey(ItemPickupType.CLOCK)) {
      TotalTime = (int)((Random.Range(0, Level) / Level) * 20) + 70;
    } else {
      TotalTime = 60;
    }
    int oldTargetScore = TagetScore;
    TagetScore = ((Level - 1) * 1200) + 800 + Random.Range(0, Level) * 500;
    LevelScore = (TagetScore - oldTargetScore) + Random.Range(200, 800);
    TotalScore = operation_data_.TotalScore;
    timer_.StartTime = TotalTime;
    timer_.Restart();
  }

  public void NewGame() {
    operation_data_.IsFirstTime = false;
    operation_data_.Level = 1;
    TotalTime = 61;
    TagetScore = 800;
    operation_data_.TotalScore = 0;
    TotalScore = 0;
    LevelScore = TagetScore + Random.Range(800, 1500);
    timer_.StartTime = TotalTime;
    SaveData();
  }

  public void StartTimer() {
    timer_.Restart();
  }

  public void Continue() {
    int oldTargetScore = TagetScore;
    TotalTime = 61;
    TagetScore = ((Level - 1) * 1200) + 800 + Random.Range(0, Level) * 500;
    LevelScore = (TagetScore - oldTargetScore) + Random.Range(800, 2000);
    timer_.StartTime = TotalTime;
  }

  public void UpdateScore() {
    TotalScore += ScoreAmount;
  }

  public void FinishLevel() {
    if(IsVictory) {
      operation_data_.Level++;
      operation_data_.TotalScore = TotalScore;
      SaveData();
    }
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
      case "DiamondGold":
      case "DiamondYellow":
      case "DiamondViolet":
        if (bought_item_dict_.ContainsKey(ItemPickupType.DIAMOND)) {
          ScoreAmount = victim.ScoreAmount + 100;
        } else {
          ScoreAmount = victim.ScoreAmount;
        }
        break;
      case "GiftBag": {
          Gift = (GIFT_TYPE)Random.Range((int)GIFT_TYPE.POWER, (int)GIFT_TYPE.NONE + 1);
          if(Gift == GIFT_TYPE.SCORE_FLY) {
            int maxScore = bought_item_dict_.ContainsKey(ItemPickupType.CLOVER) ? 400 : 150;
            ScoreAmount = Random.Range(50, maxScore);
          }
        }
        break;
      default:
        Gift = GIFT_TYPE.NONE;
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
    if(item.Type.Equals(ItemPickupType.BOMB)) {
      operation_data_.BombCount = bought_item_dict_[item.Type].Count;
      SaveOperationData();
    }
  }

  public void BuyPickupItem(ItemPickupSo item, int amount, int count = 1) {
    if(bought_item_dict_.ContainsKey(item.Type)) {
      bought_item_dict_[item.Type].Count += count;
    } else {
      bought_item_dict_.Add(item.Type, new BoughtItem(item, count));
    }
    TotalScore -= amount;
    if (item.Type.Equals(ItemPickupType.BOMB)) {
      operation_data_.BombCount = bought_item_dict_[item.Type].Count;
      SaveOperationData();
    }
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
    SaveUserSettings();
    SaveOperationData();
  }

  private void SaveUserSettings() {
    SaveSetting(user_settings_, user_settings_.name);
  }

  private void SaveOperationData() {
    SaveSetting(operation_data_, operation_data_.name);
  }

  private void LoadSetting<T>(T instance, string key) {
    string settingData = PlayerPrefs.GetString(key, string.Empty);
    if (settingData != string.Empty) {
      JsonUtility.FromJsonOverwrite(settingData, instance);
    }
  }

  private void SaveSetting<T>(T instance, string key) {
    string settingData = JsonUtility.ToJson(instance);
    PlayerPrefs.SetString(key, settingData);
    PlayerPrefs.Save();
  }

  private void LoadBomb() {
    int bombCount = operation_data_.BombCount;
    if (bombCount > 0) {
      BuyPickupItem(PickupSos.Find(x => x.Type == ItemPickupType.BOMB), 0, bombCount);
    }
  }
}
