﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner {
  void Spawn(int level, int targetScore);
  void OnSpawnableDestroy(GameObject obj);
}
