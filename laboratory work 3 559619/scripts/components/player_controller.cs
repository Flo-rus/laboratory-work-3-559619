using Godot;
using System;
[GlobalClass]
public partial class player_controller : Node
{
	[Export] public float Speed = 20.0f;
	public bool IsBlueprint = false;
	private CharacterBody3D ParentNode;
	private Vector3 MoveDirection;
	private Plane GamePlane = new Plane(new Vector3(0, -1, 0), 0);

	[Export] PackedScene BulletScene;
	[Export] PackedScene TowerBlueprintScene;

	[Export] PackedScene TowerScene1;
	private int _towerNumber1 = 1;
	[Export] PackedScene TowerScene2;
	private int _towerNumber2 = 2;
	[Export] PackedScene TowerScene3;
	private int _towerNumber3 = 3;

	private int number;

	public override void _Ready()
	{	
		ParentNode = GetParent<CharacterBody3D>();
	}

	public override void _Process(double delta)
	{
		ParentNode.Velocity = new Vector3(Input.GetVector("a", "d", "w", "s").X, 0, Input.GetVector("a", "d", "w", "s").Y)*Speed;
		ParentNode.MoveAndSlide(); // движение игрока

		if (Input.IsActionJustPressed("click")) // стрельба игрока
		{
			SpawnBullet();
		}
		ParentNode.GetNode<Node3D>("MeshInstance3D").LookAt(PlanePosition(GetViewport().GetMousePosition())); // поворот игрока

		if (IsBlueprint == false && Input.IsActionJustPressed("build")) // постановка чертежа башни перед подтверждением постройки
		{
			_buildTowerBlueprint();
			IsBlueprint = true;
		}
		else if (IsBlueprint == true)
		{
			if (Input.IsActionJustPressed("1"))
			{
				TowerBlueprint.QueueFree();
				IsBlueprint = false;
				_buildTower(_towerNumber1);
			}
			else if (Input.IsActionJustPressed("2"))
			{
				TowerBlueprint.QueueFree();
				IsBlueprint = false;
				_buildTower(_towerNumber2);
			}
			else if (Input.IsActionJustPressed("3"))
			{
				TowerBlueprint.QueueFree();
				IsBlueprint = false;
				_buildTower(_towerNumber3);
			}
		}
	}
	public Vector3 PlanePosition (Vector2 Coord) {
		return (Vector3)GamePlane.IntersectsRay(GetViewport().GetCamera3D().ProjectRayOrigin(Coord), GetViewport().GetCamera3D().ProjectRayNormal(Coord));
	} 

	private void SpawnBullet() // функция спауна пули и задания группы её целей
	{
		Bullet BulletInstance = BulletScene.Instantiate<Bullet>();
		BulletInstance.Position = ParentNode.GlobalPosition;
		ParentNode.AddSibling(BulletInstance);
		bool isAlly = ParentNode.IsInGroup("allies");
		if (isAlly)
		{
			foreach(Node component in BulletInstance.GetChildren())
			{
				if (component is DamageComponent dmgComponent)
				{
					dmgComponent.targetGroup = "alias";
				}
			}
		}
		else
		{
			foreach(Node component in BulletInstance.GetChildren())
			{
				if (component is DamageComponent dmgComponent)
				{
					dmgComponent.targetGroup = "enemies";
				}
			}
		}
		
		BulletInstance.LookAt(PlanePosition(GetViewport().GetMousePosition()));
	}

	private CharacterBody3D TowerBlueprint;

	private void _buildTowerBlueprint() // метод постановки чертежа башни перед самой постройкой
	{
		TowerBlueprint = TowerBlueprintScene.Instantiate<CharacterBody3D>();
		ParentNode.AddChild(TowerBlueprint);
		TowerBlueprint.Position = new Vector3 (1, 0, 1);
	}

	private void _buildTower(int number) // функция стройки башни в зависимости от того, какой тип башни выбрал игрок
	{
		CharacterBody3D Tower = TowerScene1.Instantiate<CharacterBody3D>();
		switch(number)
		{
			case 1: 
				Tower = TowerScene1.Instantiate<CharacterBody3D>();
				break;
			case 2:
				Tower = TowerScene2.Instantiate<CharacterBody3D>();
				break;
			case 3:
				Tower = TowerScene3.Instantiate<CharacterBody3D>();
				break;
		}
		
		ParentNode.AddSibling(Tower);
		Tower.Position = TowerBlueprint.GlobalPosition;
	}

}