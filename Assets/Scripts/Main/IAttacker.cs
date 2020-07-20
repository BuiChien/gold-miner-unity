﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker {
  string Name { get; }
  bool CanAttach { get; set; }
  void OnAttach(IVictim victim);
}
