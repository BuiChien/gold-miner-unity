using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker {
  string Name { get; set; }
  IGoodsBehavior GoodAttacked { get; set; }
  void DestroyGoodAttached();
}
