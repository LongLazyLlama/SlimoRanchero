using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    [SerializeField]
    public int Amount = 1;
    public ItemProperties Properties = null;
    public string Name { get { return Properties.Name;  } }


    public Item(int amount, ItemProperties itemProperties)
    {
        Amount = amount;
        Properties = itemProperties;
    }

    public virtual GameObject ConvertToGameObject()
	{
        return Properties.ItemObject;
	}
}
