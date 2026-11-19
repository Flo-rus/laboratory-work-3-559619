using Godot;
using System;

[GlobalClass]
public partial class vision_component : Area3D
{
	private Node3D ParentNode;
	private Node3D currentTarget;

	public override void _Ready()
	{
		ParentNode = GetParent<CharacterBody3D>();
	}

	public Node3D GetTarget() // функция захвата цели и задания ей цели
	{
		foreach(var item in GetOverlappingBodies())
		{
			bool isAlly = ParentNode.IsInGroup("alias");
			bool isEnemy = ParentNode.IsInGroup("enemies");
			bool differenteTeams = (isAlly && item.IsInGroup("enemies") || isEnemy && item.IsInGroup("alias"));
			if (item != ParentNode && differenteTeams && (!IsInstanceValid(currentTarget) || item.Position.DistanceTo(ParentNode.Position) < currentTarget.Position.DistanceTo(ParentNode.Position)))
			{
				currentTarget = item;
			}
		}
		return currentTarget;
	}
}
