using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class LandPlotMenu : MonoBehaviour
{
    public static LandPlotMenu PlotMenu;

    public GameObject ActiveLandPlot;

    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject[] BuildingPrefabs;

    private LandPlot _plot;

    private void Awake()
    {
        if (PlotMenu == null)
        {
            PlotMenu = this;
        }
        _menu.SetActive(false);
    }

    public void PlaceBuilding(int buildingNumber)
    {
        if (!_plot.HasBuilding)
        {
            var building = BuildingPrefabs[buildingNumber];

            Instantiate(building, ActiveLandPlot.transform.position, ActiveLandPlot.transform.rotation, _plot.PlotLand.transform);
            _plot.HasBuilding = true;
        }
    }

    public void RemoveBuilding()
    {
        if (_plot.HasBuilding)
        {
            foreach (Transform child in _plot.PlotLand.transform)
            {
                Destroy(child.gameObject);
            }
            _plot.HasBuilding = false;
        }
    }

    public void ActivateMenu(GameObject landPlot)
	{
        Cursor.visible = true;
        _menu.SetActive(true);
        ActiveLandPlot = landPlot;
        _plot = landPlot.GetComponent<LandPlot>();
    }

    //On escape pressed.
    public void ClosePlotMenu()
    {
        Cursor.visible = false;
        _menu.SetActive(false);
        ActiveLandPlot = null;
        _plot = null;
    }
}
