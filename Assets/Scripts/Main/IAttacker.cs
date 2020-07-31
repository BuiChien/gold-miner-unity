using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker {
  string Name { get; }
  IVictim Target { get; set; }
  void CancelAttach();
  bool OnAttach(IVictim victim);
}
