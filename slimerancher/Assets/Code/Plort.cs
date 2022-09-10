using UnityEngine;

public class Plort : MonoBehaviour, IVacuumable
{
	#region Inspector Fields
	public ItemProperties Item = null;
	#endregion
	public enum PlortTypes
    {
        Pink = 0,
        Blue = 1,
    }

    private Rigidbody _rigidBody = null;

	public PlortTypes PlortType = PlortTypes.Pink;

	private void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
	}

	public Item ConvertToItem()
	{
		Destroy(gameObject);
		return new Item(1,Item); // Replace this with the item that the plort should become!
	}

	public Rigidbody GetRigidbody()
	{
		return _rigidBody;
	}
}
