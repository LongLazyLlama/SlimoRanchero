using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	public Item[] Items = new Item[4];
	[SerializeField]
	public int SlotCapacity = 20;
	[SerializeField]
	public int SelectedSlot = 0;
	[SerializeField]
	public int Money = 0;

	public bool AddItem(Item addedItem)
	{
		if (addedItem == null) return false;
		foreach(Item item in Items)
		{
			//Debug.Log("Added Item");
			if(item != null && addedItem.Name == item.Name && item.Amount < SlotCapacity)
			{
				item.Amount += addedItem.Amount;
				return true;
			}
		}

		int i = 0;
		while(i < Items.Length){
			if(Items[i]==null || Items[i].Amount == 0)
			{
				//Debug.Log("Added New Item");
				Items[i] = addedItem;
				//Debug.Log(addedItem.ToString());
				return true;
			}
			i++;
		}
		return false;
	}

	public Item RemoveItem()
	{
		if(Items[SelectedSlot] != null && Items[SelectedSlot].Amount > 0)
		{
			Item toRemove = new Item(1, null);
			toRemove.Properties = Items[SelectedSlot].Properties;
			Items[SelectedSlot].Amount--;
			if (Items[SelectedSlot].Amount == 0) Items[SelectedSlot].Properties.name = "";
			return toRemove;
		}
		return null;
	}

	public void ChangeSlot(int i)
	{
		SelectedSlot += i;
		if (SelectedSlot == Items.Length) SelectedSlot = 0;
		else if (SelectedSlot < 0) SelectedSlot = Items.Length - 1;
		//Debug.Log("Selected itemSlot :" + SelectedSlot +" " + Items[SelectedSlot].Name + " " + Items[SelectedSlot].Amount);
	}
}
