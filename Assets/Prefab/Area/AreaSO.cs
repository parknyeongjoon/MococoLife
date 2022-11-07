using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaSO", menuName = "SO/AreaSO")]
public class AreaSO : ScriptableObject
{
    public Boss boss;
    public List<GameObject> bossAreas;
}
