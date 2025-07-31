using UnityEngine;
using UnityEngine.Events;

public class ActivationTrigger : MonoBehaviour
{
    [SerializeField] string[] tagsToCompare = { "Player" };
    [SerializeField] UnityEvent activationMethods;
    [SerializeField] bool destroyAfterTrigger = false;
    [SerializeField] UnityEvent deactivationMethods;


    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in tagsToCompare) { if (other.CompareTag(tag)) { activationMethods?.Invoke(); if (destroyAfterTrigger) { Destroy(this); } break; } };
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (string tag in tagsToCompare) { if (other.CompareTag(tag)) { deactivationMethods?.Invoke(); break; } };
    }
}
