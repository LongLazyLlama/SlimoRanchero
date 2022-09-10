using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
	#region Inspector Fields
	//[SerializeField] private UnityEvent _interacted = new UnityEvent();
	#endregion

	#region Properties
	public GameObject InteractibleObject;

	public bool IsInteracting;
	#endregion

	#region Fields

	#endregion

	#region Life Cycle
	#endregion

	#region Methods

	public void Interact(AdvancedInputs advancedInputs)
	{
        if (IsInteracting)
        {
			LandPlotMenu.PlotMenu.ClosePlotMenu();

			StarterAssetsInputs.StarterAssetsInput.SetCursorState(true);
			FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("Player");

			IsInteracting = false;
		}
		else if (!IsInteracting)
        {
			var plotObject = InteractibleObject.GetComponentInParent<LandPlot>().Plot;

			LandPlotMenu.PlotMenu.ActivateMenu(plotObject);

			StarterAssetsInputs.StarterAssetsInput.SetCursorState(false);
			FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("UI");

			//Debug.Log("Interacting with " + plotObject);

			IsInteracting = true;

			//_interacted?.Invoke();
		}
	}
	#endregion
}
