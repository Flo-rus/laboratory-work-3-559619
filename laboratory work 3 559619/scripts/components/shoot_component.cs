using Godot;
using System;

[GlobalClass]
public partial class shoot_component : Node
{
	private CharacterBody3D ParentNode;
	[Export] private vision_component vision_component;
	[Export] private PackedScene BulletScene;
	[Export] public float ShootingCooldown = 1f;
	private float CooldownProgress = 0f;
	public Node3D Target;

	public override void _Ready() // регистрация родителя
	{
		ParentNode = GetParent<CharacterBody3D>();
	}

	public override void _Process(double delta)
	{
		if (IsInstanceValid(Target)) // стрельба по цели с учетом перезарядки и наличия самой цели
		{
			if (CooldownProgress <= 0)
			{
				CooldownProgress = ShootingCooldown;
				SpawnBullet();
			}
		}
		else if (vision_component != null)
		{
			Target = vision_component.GetTarget();
		}
		CooldownProgress -= (float)delta;
	}

	private void SpawnBullet() // спаун пули и задание ей группы цели
	{
		Bullet BulletInstance = BulletScene.Instantiate<Bullet>();
		BulletInstance.Position = ParentNode.GlobalPosition;
		bool isAlly = ParentNode.IsInGroup("alias");
		ParentNode.AddSibling(BulletInstance);
		if (isAlly)
		{
			foreach(Node component in BulletInstance.GetChildren())
			{
				if (component is DamageComponent dmgComponent)
				{
					dmgComponent.targetGroup = "enemies";
				}
			}
		}
		else
		{
			foreach(Node component in BulletInstance.GetChildren())
			{
				if (component is DamageComponent dmgComponent)
				{
					dmgComponent.targetGroup = "alias";
				}
			}
		}
		
		if (IsInstanceValid(Target))
		{
			BulletInstance.LookAt(Target.Position);
		}
	}
}
