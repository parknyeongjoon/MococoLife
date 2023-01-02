using Photon.Pun;
using System.Collections;
using UnityEngine;

#region enum
//player
public enum P_State { Idle, Action, Stun, TimePause, Dead };
//BlackSmith
public enum Create_State { Idle, Create, Finish };
public enum Item_Type { Tool, BattleItem, Ingredient, Food, Elixir }

public enum Dmg_Type { None, UnBreakable, Damage, Stagger, Hit, Destruction }
public enum CC_Type { KnockBack, Stun }

public enum Boss_Type { Crab }
public enum Boss_State { Idle, Action, Pattern, Immune, Stun, Dead };

#endregion

#region interface

public interface IDamagable
{
    void Damage(float dmg);
}

public interface ICC
{
    void KnockBack(Vector3 knockDir, float dis);

    void Stun(float stunTime);
}

#endregion

#region class

[System.Serializable]
public class Slot
{
    public ItemData itemData = null;
    public int itemCount = 0;
}

#endregion
