using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour, IVacuumable
{
	#region Inspector Fields
	public ItemProperties Item = null;
	public ItemProperties TarrItem = null;
	#endregion

	#region Properties
	private enum SlimeTypes
    {
		Pink = 0,
		Blue = 1,
		Tarr = 2
    }

	private enum ItemTypes
    {
		Food = 0,
		Plort = 1,
    }
	private List<GameObject> _itemList = new List<GameObject>();

	public GameObject Spawner;

	[SerializeField]
	private Material _tarrMaterial;
	[SerializeField]
	private GameObject _plortPrefab;
	[SerializeField]
	private SlimeTypes _slimeType = SlimeTypes.Pink;

	[Space]
	[SerializeField]
	private float _moveSpeed = 5.0f;
	[SerializeField]
	private float _movementRange = 5.0f; //Range from spawnposition.
	[SerializeField]
	private float _jumpHeight = 1.0f;
	[SerializeField]
	private Vector2 _moveDelay = new Vector2(2.0f, 5.0f); // (MIN random value, MAX random value)

	[Space]
	[SerializeField]
	private float _plortSpawnRange = 1.0f;
	[SerializeField]
	private float _tarrEvolveTime = 60.0f; //How long does it take to become a tarr when seperated from spawn and not being fed.

	[Space]
	[SerializeField]
	private bool _infiniteGrowth; //Slime can keep evolving when this is enabled.
	[SerializeField]
	private bool _tarrEvolution; //Becomes a tarr when seperated from its spawn for more than 120sec and eats its own plorts.
	[SerializeField]
	private bool _seperatedFromSpawn; //When a slime left its spawner or does not have one this becomes true.
	[SerializeField]
	private bool _evolved;
	[SerializeField]
	private bool _isTarr;

	private ItemTypes _itemType;

	private GameObject _item;
	private GameObject _slime;

	private Vector3 _spawnPosition;
	private Vector3 _destination;

	private float _randomDelay;
	private float _tarrTimer;
	private float _timer;

	private string _slimeTypeName;

	private bool _ateFruit;
	private bool _slimeInRange;
	private bool _itemInRange;
	#endregion

	#region Fields
	private Rigidbody _rigidBody = null;
	#endregion

	#region Life Cycle
	private void Start()
    {
        InitializeSlime();
    }

    private void Update()
	{
		SlimeMovement();
	}
	#endregion

	#region Methods
	private void InitializeSlime()
	{
		_slimeTypeName = _slimeType.ToString();
		_rigidBody = GetComponent<Rigidbody>();

        if (this.transform.parent == null || !this.transform.parent.CompareTag("Spawner"))
        {
			_spawnPosition = this.transform.position;
			_seperatedFromSpawn = true;
		}
        else
        {
			_spawnPosition = Spawner.transform.position;
		}

		FindRandomDestination();
	}

	private void SlimeMovement()
	{
		_timer += Time.deltaTime;

		if (_seperatedFromSpawn)
        {
            if (_tarrEvolution)
            {
				_tarrTimer += Time.deltaTime;
			}
			_spawnPosition = this.transform.position;
		}

		if (_timer >= _randomDelay)
        {
			this.transform.position = Vector3.MoveTowards(this.transform.position, _destination, Time.deltaTime * _moveSpeed);

            if (_itemInRange && _item != null && !_isTarr)
            {
				//If an item is in range, set the items position as the new destination.
				_destination = _item.transform.position;
			}
			else if (_slimeInRange && _slime != null && _isTarr)
            {
				_destination = _slime.transform.position;
			}
			else if (SlimeNearDestination() && !_itemInRange)
            {
				//Move to a random destination when idle.
                FindRandomDestination();
            }

			if (_timer > 10)
            {
				//If the slime is stuck or the destination is unreachable, reposition to random destination after 10 seconds.
				FindRandomDestination();
			}
		}
	}

	private void FindRandomDestination()
	{
		//Set destination to random location around its spawnposition.
		var randomDestination = Random.insideUnitCircle * (_movementRange + this.transform.localScale.x / 2);
		var rayPosition = _spawnPosition + new Vector3(randomDestination.x, 50.0f, randomDestination.y);

		//Raycast on ground (layermask does not work in raycast itself for some reason).
		RaycastHit hit;
        if (Physics.Raycast(rayPosition, Vector3.down, out hit, Mathf.Infinity))
        {
			Debug.DrawRay(rayPosition, Vector3.down * hit.distance, Color.green);
			//Debug.Log(hit.transform.gameObject + " hit");

			if (hit.transform.gameObject.layer == 8)
            {
				_destination = hit.point + new Vector3(0, _jumpHeight + this.transform.localScale.x / 2, 0);
				_timer = 0;
			}
		}

		//Sets a new random delay before next movement.
		_randomDelay = Random.Range(_moveDelay.x, _moveDelay.y);
	}

	private bool SlimeNearDestination()
    {
		//Is the slime close to its destination.
		var _slimeRadius = this.transform.localScale.x;
		return Vector3.Distance(this.transform.position, _destination) < _slimeRadius / 2;
	}

	private void CreatePlort()
	{
        if (_ateFruit)
        {
			var randomPosition = Random.insideUnitCircle * _plortSpawnRange;
			var plortSpawnPos = this.transform.position + new Vector3(randomPosition.x, this.transform.localScale.x, randomPosition.y);

			Instantiate(_plortPrefab, plortSpawnPos, Quaternion.identity);

            if (_evolved)
            {
				Instantiate(_plortPrefab, plortSpawnPos, Quaternion.identity);
				Instantiate(_plortPrefab, plortSpawnPos, Quaternion.identity);
			}

			_ateFruit = false;
		}
	}

	private void EvolveSlime()
	{
		this.transform.position = this.transform.position + new Vector3(0.0f, 1.5f, 0.0f);
		this.transform.localScale = this.transform.localScale + Vector3.one;

		if (!_infiniteGrowth)
		{
    //        foreach (GameObject item in _itemList)
    //        {
				//if (item.TryGetComponent(out Plort p))
    //            {
				//	_itemList.Remove(item);
    //            }
    //        }
			_evolved = true;
		}
	}

	public void EvolveTarr()
	{
		this.gameObject.GetComponent<MeshRenderer>().material = _tarrMaterial;

		Item = TarrItem;
		_slimeType = SlimeTypes.Tarr;
		_slimeTypeName = _slimeType.ToString();
		_isTarr = true;
	}


	private void OnTriggerEnter(Collider other)
    {
		//Is there an item nearby ?
        if (other.CompareTag("Item"))
        {
			//Debug.Log("Item: " + other.gameObject + " in range.");

			//Is it a plort item ?
			other.gameObject.TryGetComponent<Plort>(out Plort plort);

            if (plort != null && plort.PlortType.ToString() != _slimeTypeName && !_isTarr)
            {
                if (!_evolved || _infiniteGrowth)
                {
					//Adds all items in range to a list to keep track of them.
					//_itemList.Add(other.gameObject);

					_itemType = ItemTypes.Plort;

					_item = other.gameObject;
					_itemInRange = true;
				}
			}
			else if (plort != null && plort.PlortType.ToString() == _slimeTypeName && _tarrTimer > _tarrEvolveTime)
            {
				_itemType = ItemTypes.Plort;

				_item = other.gameObject;
				_itemInRange = true;
			}

            //Is it a food/other item ?
            if (plort == null && !_isTarr)
            {
				//_itemList.Add(other.gameObject);

				_itemType = ItemTypes.Food;

				_item = other.gameObject;
				_itemInRange = true;
			}
		}
		else if (other.CompareTag("Slime") && _isTarr)
        {
			other.gameObject.TryGetComponent<Slime>(out Slime slime);

            if (slime._slimeTypeName != "Tarr")
            {
				_slime = other.gameObject;
				_slimeInRange = true;
            }
		}
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
			//If there is still another item in the list of in-range items target this item, else no items left in range.
			if (_itemList.Count != 0)
			{
				//_itemList.Remove(other.gameObject);
				//_item = _itemList[_itemList.Count - 1];

				//_itemInRange = true;

				//Debug.Log("Item: " + other.gameObject + " is the new target.");
			}
            else
            {
				_itemInRange = false;
			}

			//Debug.Log("Item: " + other.gameObject + " out of range.");
		}
		else if (other.CompareTag("Slime"))
        {
			_slimeInRange = false;
        }
	}

	private void OnCollisionEnter(Collision collision)
    {
		if (_item != null && collision.gameObject == _item.gameObject)
		{
			collision.gameObject.TryGetComponent<Plort>(out Plort plort);
			Debug.Log(collision.gameObject + " eaten.");

			//If its food make a plort, if its a plort (other color then slime) evolve and reposition,
			//else if plort is your type become tarr and reposition.
			if (_itemType == ItemTypes.Food)
			{
				_tarrTimer = 0;
				_ateFruit = true;
				_itemInRange = false;

				CreatePlort();
			}
			else if (_itemType == ItemTypes.Plort && plort.PlortType.ToString() != _slimeTypeName)
			{
				EvolveSlime();
				FindRandomDestination();
			}
			else if (_itemType == ItemTypes.Plort && plort.PlortType.ToString() == _slimeTypeName)
			{
				EvolveTarr();
				FindRandomDestination();
			}

			Destroy(_item.gameObject);
			//_itemList.Remove(collision.gameObject);
		}
		else if (_slime != null && collision.gameObject == _slime.gameObject)
        {
			_slime.GetComponent<Slime>().EvolveTarr();
        }
	}

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, 3);
    }
	#endregion

	#region IVacuumable
	public Item ConvertToItem()
	{
		Destroy(gameObject);
		return new Item(1, Item);
	}

	public Rigidbody GetRigidbody()
	{
		return _rigidBody;
	}
	#endregion
}
