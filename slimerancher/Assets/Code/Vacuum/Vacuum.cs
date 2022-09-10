using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Vacuum : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField] private float _vacuumStrength = 10f;

	[FormerlySerializedAs("_suckCollector")][SerializeField] private VacuumableCollector _vacuumableCollector = null;
	[SerializeField] private VacuumableCollector _collector = null;
	[SerializeField] private Inventory _inventory = null;

	[SerializeField] private Transform _ejectPoint = null;

	#endregion

	#region Properties

	#endregion

	#region Fields
	private bool _vacuumOn = false;
	#endregion

	#region Life Cycle
	private void Start()
	{
	}

	private void Update()
	{

	}

	private void FixedUpdate()
	{
		if(_vacuumOn)
		{
			while (_collector.Vacuumables.Count > 0)
			{
				CaptureObject(_collector.Vacuumables[0]);
				_collector.RemoveVacuumable(_collector.Vacuumables[0]);
			}

			foreach (IVacuumable vacuumable in _vacuumableCollector.Vacuumables)
			{
				if(vacuumable != null)
					PullRigidbodyWithoutRotation(vacuumable.GetRigidbody(), 0, _vacuumStrength);
			}
		}
	}
	#endregion

	#region Vacuuming Methods
	public void StartVacuum()
	{
		_vacuumOn = true;
	}

	public void EndVacuum()
	{
		_vacuumOn = false;
	}

	private void PullRigidbodyWithoutRotation(Rigidbody rigidbody, float distanceToStop, float speed)
	{
		if (rigidbody == null)
			return;

		Vector3 direction = Vector3.zero;

		if(Vector3.Distance(transform.position, rigidbody.position) > distanceToStop)
		{
			direction = transform.position - rigidbody.position;
			rigidbody.AddForce(direction.normalized * speed, ForceMode.Force);
		}
	}

	private void CaptureObject(IVacuumable vacuumable)
	{
		if(_vacuumOn)
		{
			_inventory.AddItem(vacuumable.ConvertToItem());
		}
	}
	#endregion

	#region Shooting Methods
	public void EjectItem()
	{
		Item item = _inventory.RemoveItem();

		if (item == null) return;

		GameObject gameObject = Instantiate(item.ConvertToGameObject(), _ejectPoint.position, _ejectPoint.rotation);
		Rigidbody rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
		rigidbody.AddForce(rigidbody.transform.forward * 10, ForceMode.Impulse);
		
	}
	#endregion
}
