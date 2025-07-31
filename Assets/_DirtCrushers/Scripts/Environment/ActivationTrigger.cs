using UnityEngine;
using UnityEngine.Events;

public class ActivationTrigger : MonoBehaviour
{
    [SerializeField] private string[] tagsToCompare = { "Player" };
    [SerializeField] private UnityEvent activationMethods;
    [SerializeField] private bool destroyAfterTrigger = false;
    [SerializeField] private UnityEvent deactivationMethods;
    private bool gotUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!gotUsed)
            foreach (string tag in tagsToCompare) { if (other.CompareTag(tag)) { gotUsed = true; activationMethods?.Invoke(); if (destroyAfterTrigger) { Destroy(this); } break; } };
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (string tag in tagsToCompare) { if (other.CompareTag(tag)) { deactivationMethods?.Invoke(); break; } };
    }
}