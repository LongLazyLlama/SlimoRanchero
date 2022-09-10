using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IVacuumable
{
	#region Inspector Fields
	public ItemProperties Item = null;
	#endregion
	private Rigidbody _rigidBody = null;

	private void Start()
    {
		_rigidBody = GetComponent<Rigidbody>();
	}

	public Item ConvertToItem()
	{
		Destroy(gameObject);
		return new Item(1,Item); // Replace this with the item that the plort should become!
	}

	public Rigidbody GetRigidbody()
	{
		return _rigidBody;
	}
}
