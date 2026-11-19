using Godot;
using System;

[GlobalClass]
public partial class health_component : Node
{

	private Node3D ParentNode;
	[Export] public float max_health = 100f;
	public float current_health;
	
	public override void _Ready() // регистрирует родителя и задает ему здоровье
	{
		ParentNode = GetParent<CharacterBody3D>();
		current_health = max_health;
	}

	public void takeDamage (float damageAmount) // принимает урон и уменьшает здоровье. Если здоровье <= 0, то вызывает функцию смерти
	{
		current_health -= damageAmount;
		if (current_health <= 0)
		{
			Die();
		}
	}

	public void Die() // функция смерти объекта. Если объект - игрок или ядро, то заканчивает игру
	{
		ParentNode.QueueFree();
		if (ParentNode.IsInGroup("game_cores") && ParentNode.IsInGroup("alias"))
		{
			GetTree().Quit();
		}
	}

	
}
