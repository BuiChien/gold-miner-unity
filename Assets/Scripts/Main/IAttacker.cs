using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker {
  void OnAttach(IVictim victim);
}
