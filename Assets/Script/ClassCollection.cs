#region enum

public enum Dmg_Type { None, Hammer, Axe, Pickaxe, UnBreakable, Damage, Stagger, Hit, Destruction }

public enum Boss { Baltan }

#endregion

#region interface

public interface IDamagable
{
    void Damage(Dmg_Type dmg_Type, float dmg);
}

#endregion
