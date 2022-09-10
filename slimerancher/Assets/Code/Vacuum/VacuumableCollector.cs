using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VacuumableCollector : MonoBehaviour
{
	#region Inspector Fields

	#endregion

	#region Properties
	public List<IVacuumable> Vacuumables { get; private set; } = new List<IVacuumable>();
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

	private void OnTriggerEnter(Collider collider)
	{
		IVacuumable vacuumable = collider.GetComponent<IVacuumable>();

		if (vacuumable == null) return;
		if (Vacuumables.Contains(vacuumable)) return;

		Vacuumables.Add(vacuumable);
	}

	private void OnTriggerExit(Collider collider)
	{
		RemoveVacuumable(collider.GetComponent<IVacuumable>());
	}
	#endregion

	#region Methods
	public void RemoveVacuumable(IVacuumable vacuumable)
	{
		Vacuumables.Remove(vacuumable);
	}
	#endregion
}
