using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private Vector3 _resetPosition = new Vector3(0, 1, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            characterController.enabled = false;
            other.transform.position = _resetPosition;
            characterController.enabled = true;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
