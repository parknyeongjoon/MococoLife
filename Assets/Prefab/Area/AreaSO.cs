using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaSO", menuName = "SO/AreaSO")]
public class AreaSO : ScriptableObject
{
    public BossInfo boss;
    public List<GameObject> bossAreas;
}
