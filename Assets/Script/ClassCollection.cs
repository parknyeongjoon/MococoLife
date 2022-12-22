using Photon.Pun;

#region enum
//player
public enum P_State { Idle, Action, Stun, TimePause, Dead };
//BlackSmith
public enum Create_State { Idle, Create, Finish };

public enum CC_Type { KnockBack, Stun }
public enum Item_Type { Tool, BattleItem, Ingredient, Food, Elixir }

public enum Boss_Type { Crab }
public enum Boss_State { Idle, Action, Pattern, Stun };

#endregion

#region interface

public interface IDamagable
{
    void Damage(float dmg);
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
