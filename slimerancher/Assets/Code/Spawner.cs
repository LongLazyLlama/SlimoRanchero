using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int _amountOfObjects = 5; //Amount of objects that will be spawned.
    [SerializeField]
    private float _spawnRange = 3.0f;
    [SerializeField]
    private Vector2 _randomSpawnHeight = new Vector2(2.0f, 4.0f);

    [Space]
    [SerializeField]
    private float _timeBetweenSpawns = 0.2f;
    [SerializeField]
    private float _spawnerDelay = 2.0f;
    [SerializeField]
    private float _respawnDelay = 20.0f;

    [Space]
    [SerializeField]
    private GameObject[] _ObjectTypes; //All different possible objects that can be spawned.

    private int spawnedObjects;
    private float _timer;
    private bool _spawnCycleActive;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _spawnerDelay || _spawnCycleActive)
        {
            if (_timer > _timeBetweenSpawns && spawnedObjects < _amountOfObjects)
            {
                SpawnObject();

                _spawnCycleActive = true;

                spawnedObjects++;
                _timer = 0;
            }
            else if (spawnedObjects == _amountOfObjects)
            {
                _spawnCycleActive = false;
                _timer = 0;
            }
        }
        if (spawnedObjects == _amountOfObjects && this.transform.childCount == 0)
        {
            _spawnCycleActive = true;
            spawnedObjects = 0;

            //Sets the timer to respawn time to further delay a new spawncycle if all objects are gone.
            _timer = -_respawnDelay + _spawnerDelay;
        }
    }

    private void SpawnObject()
    {
        var randomPosition = Random.insideUnitCircle * _spawnRange;
        var randomHeight = Random.Range(_randomSpawnHeight.x, _randomSpawnHeight.y);
        var objectSpawnPosition = this.transform.position + new Vector3(randomPosition.x, randomHeight, randomPosition.y);

        var randomObjectType = Random.Range(0, _ObjectTypes.Length);

        var spawnedObject = Instantiate(_ObjectTypes[randomObjectType], objectSpawnPosition, Quaternion.identity, this.transform);

        if (spawnedObject.CompareTag("Slime"))
        {
            spawnedObject.GetComponent<Slime>().Spawner = this.gameObject;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _spawnRange);
    }
}
