using Godot;
using System;

[GlobalClass]
public partial class DamageComponent : Area3D
{

	[Export] public float DamageAmount = 50;
	[Export] public string targetGroup = "enemy";
	public Node ParentNode;

	public override void _Ready()
	{
		ParentNode = GetParent();
		BodyEntered += OnBodyEntered;
	}

	public virtual void OnBodyEntered (Node node) // функция проверки пересечение объектов с коллизией родителя и в случае пересечения подтверждает нанесение урона этим объектам
	{
		if (node.IsInGroup(targetGroup) && node != ParentNode) 
		{
		foreach (Node body in GetOverlappingBodies())
		{
			if (body != ParentNode && body.IsInGroup(targetGroup)) 
			{
				ApplyDamage(body);
			}
		}
		ParentNode.QueueFree();
		}
	}

	public virtual void ApplyDamage (Node damagedBody) // наносит урон и вызывает регистрацуию урона в health_component объекта, которому наносится урон
	{
		foreach (Node component in damagedBody.GetChildren())
		{
			if (component is health_component healthComponent) 
			{
				healthComponent.takeDamage(DamageAmount);
			}
		}
	}
}