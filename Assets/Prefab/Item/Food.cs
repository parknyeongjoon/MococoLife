using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "SO/Item/Food")]
public class Food : BattleItemData
{
    [SerializeField] float foodTime;

    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        if (info.canAtkTime > 0)//������ ���� ���¶�� �ð� �߰�
        {
            info.canAtkTime += foodTime;
        }
        else//������ �� ���� ���¶�� Ÿ�̸� ����
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
