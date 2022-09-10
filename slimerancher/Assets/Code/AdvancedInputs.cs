using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AdvancedInputs : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField] private LayerMask _raycastMask = new LayerMask();
	[SerializeField] private Transform _raycastTransform = null;

	[SerializeField] private UnityEvent _onVacuumStart = new UnityEvent();
	[SerializeField] private UnityEvent _onVacuumEnd = new UnityEvent();
	[SerializeField] private UnityEvent _onShoot = new UnityEvent();
	#endregion

	#region Properties
	#endregion

	#region Fields
	private Inventory _inventory = null;
	private bool _raycastHit = false;
	private RaycastHit _raycastHitInfo = new RaycastHit();
	#endregion

	#region Life Cycle
	private void Start()
	{
		_inventory = GetComponent<Inventory>();
	}

	private void Update()
	{
		_raycastHit = Physics.Raycast(_raycastTransform.position, _raycastTransform.forward, out _raycastHitInfo, 2f, _raycastMask);
	}
	#endregion

	#region Interactions
	public void OnVacuum(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			_onVacuumStart.Invoke();
		}

		if(context.canceled)
		{
			_onVacuumEnd.Invoke();
		}
		
	}

	public void OnScroll(InputAction.CallbackContext context)
	{
        if (context.started)
        {
			_inventory.ChangeSlot(Mathf.RoundToInt(context.ReadValue<float>()));
		}
	}

	public void OnGameQuit(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}

	public void OnShoot(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			_onShoot.Invoke();
		}
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if(context.performed && _raycastHit)
		{
			Interactable interactable = _raycastHitInfo.transform.GetComponentInChildren<Interactable>();

			if(interactable != null)
            {
				interactable.InteractibleObject = _raycastHitInfo.collider.gameObject;
				interactable.Interact(this);
			}
		}
	}
	#endregion

	#region Methods

	#endregion
}
