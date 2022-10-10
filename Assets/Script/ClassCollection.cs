#region enum

public enum Dmg_Type { None, UnBreakable, Damage, Stagger, Hit, Destruction }

#endregion

#region interface

public interface IDamagable
{
    void Damage(Dmg_Type dmg_Type, float dmg);
}

#endregion
