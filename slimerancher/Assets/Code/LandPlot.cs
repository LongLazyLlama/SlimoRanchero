using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPlot : MonoBehaviour
{
    [HideInInspector]
    public bool HasBuilding;

    public GameObject Plot;
    public GameObject PlotLand;

    private void Awake()
    {
        Plot = this.gameObject;
    }
}
