using Godot;
using System;

[GlobalClass]
public partial class explosive_component : DamageComponent
{
	public void _Ready() // увеличивает CollisionShape3D DamageComponent'а 
	{
		Scale = new Vector3(10f, 10f, 40f);
	}
}
