using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillzExamples
{
  public class BB_AntiCheatVault
  {
    private int xorCode1;
    private int xorCode2;
    private int value;
    private int valueCheck;

    public BB_AntiCheatVault()
    {
      xorCode1 = Random.Range(0, int.MaxValue);
      xorCode2 = Random.Range(0, int.MaxValue);
      value = 0 ^ xorCode1;
      valueCheck = 0 ^ xorCode2;
    }

    public void Set(int newValue)
    {
      value = newValue ^ xorCode1;
      valueCheck = newValue ^ xorCode2;
    }

    public int Get()
    {
      return value ^ xorCode1;
    }

    public bool IsValid()
    {
      return (value ^ xorCode1) == (valueCheck ^ xorCode2);
    }
  }
}
