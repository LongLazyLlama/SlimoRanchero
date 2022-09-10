using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject SlotPicker;
    [SerializeField]
    private Image[] SlotImages = new Image[4];
    [SerializeField]
    private Text[] SlotAmounts = new Text[4];
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private GameObject StaminaBar;
    [SerializeField]
    private Text ItemName;
    [SerializeField]
    private Text Money;
    [SerializeField]
    private Inventory Inv;
    [SerializeField]
    private Sprite EmptySlot;
    void Update()
    {
        UpdateFromInventory();
    }

    private void UpdateFromInventory()
    {
        Money.text = Inv.Money.ToString();
        SlotPicker.transform.localPosition = new Vector3(50+100*(Inv.SelectedSlot-2), SlotPicker.transform.localPosition.y, 0);
        if (Inv.Items[Inv.SelectedSlot] != null && Inv.Items[Inv.SelectedSlot].Amount > 0)
        {
            if (Inv.Items[Inv.SelectedSlot].Name != null)
                ItemName.text = Inv.Items[Inv.SelectedSlot].Name;
            else ItemName.text = "No Name";
        }
        else ItemName.text = "Empty Slot";
        for (int i = 0;i<SlotImages.Length;i++)
        {
            if (Inv.Items[i] != null && Inv.Items[i].Amount > 0)
            {
                if(Inv.Items[i].Properties.Picture != null)
                    SlotImages[i].sprite = Inv.Items[i].Properties.Picture;
                else
                    SlotImages[i].sprite = EmptySlot;
                SlotAmounts[i].text = Inv.Items[i].Amount.ToString();
            }
            else
            {
                SlotImages[i].sprite = EmptySlot;
                SlotAmounts[i].text = "0";
            }
            
        }
    }
}
