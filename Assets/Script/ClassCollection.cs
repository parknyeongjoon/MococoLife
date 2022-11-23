#region enum
//player
public enum State { Idle, Dash, Attack, Stun, Dead };

public enum Dmg_Type { None, Hammer, Axe, Pickaxe, UnBreakable, Damage, Stagger, Hit, Destruction }
public enum Item_Type { Tool, BattleItem, Ingredient }

public enum Boss { Baltan }

#endregion

#region interface

public interface IDamagable
{
    void Damage(Dmg_Type dmg_Type, float dmg);
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
