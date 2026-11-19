using Godot;
using System;

[GlobalClass]
public partial class frozen_component : DamageComponent
{
	public override void ApplyDamage (Node damagedBody) // переопределяет функцию регистрации урона с замедлением скорости объекта, которому наносится урон
	{
		foreach (Node component in damagedBody.GetChildren())
		{
			if (component is health_component healthComponent ) 
			{
				healthComponent.takeDamage(DamageAmount);
			}
			if (component is move_component moveComponent ) 
			{
				moveComponent.MoveSpeed /= 2f;
			}
		}
	}
}
