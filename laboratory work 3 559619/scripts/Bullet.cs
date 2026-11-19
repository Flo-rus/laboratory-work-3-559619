using Godot;
using System;

public partial class Bullet : Area3D
{
	[Export] public float BulletSpeed = 200f; 
	public string targetGroup1 = ""; 
	[Export] public DamageComponent damageComponent {get; set;}

	public void _Ready() // задание группы врага пуле
	{
		foreach(Node component in GetChildren())
		{
			if (component is DamageComponent dmgComponent)
			{
				targetGroup1 = dmgComponent.targetGroup;
			}
		}
	}

	public override void _Process(double delta) // движение пули
	{
		Position += -Transform.Basis.Z * BulletSpeed * (float)delta;
	}

	public void _on_body_entered(Node3D body) // проверка что пуля зашла в коллизию врага с последующий нанесением урона
	{
		if (body.IsInGroup(targetGroup1))
		{
			foreach(var child in body.GetChildren())
			{
				if (child is health_component healthComponent)
				{
					healthComponent.takeDamage(30f);
					break;
				}
			}
			QueueFree();
		}
	}
}
