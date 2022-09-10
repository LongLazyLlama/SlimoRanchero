using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemProperties : ScriptableObject
{
	#region Inspector Fields
	[SerializeField]
	public string Name;
	[SerializeField]
	public Sprite Picture;
	[SerializeField]
	public GameObject ItemObject;
	#endregion

	#region Properties

	#endregion

	#region Fields

	#endregion

	#region Life Cycle
	private void Start()
	{
	}

	private void Update()
	{
	}
	#endregion

	#region Methods
	public virtual GameObject ConvertToGameObject()
	{
		return ItemObject;
	}
	#endregion
}
