using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVacuumable
{
	#region Methods
	public Item ConvertToItem();
	public Rigidbody GetRigidbody();
	#endregion
}
