using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "SO/Item/Food")]
public class Food : BattleItemData
{
    [SerializeField] float foodTime;

    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        if (info.canAtkTime > 0)//음식을 먹은 상태라면 시간 추가
        {
            info.canAtkTime += foodTime;
        }
        else//음식을 안 먹은 상태라면 타이머 실행
        {
            info.canAtkTime = foodTime;
            info.StartCoroutine(AtkTimer(info));
        }
    }

    public IEnumerator AtkTimer(PlayerInfo info)
    {
        while (info.canAtkTime >= 0)
        {
            info.canAtkTime -= Time.deltaTime;
            yield return null;
        }
    }
}
