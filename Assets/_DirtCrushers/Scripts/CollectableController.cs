using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField]private int collectablesStored;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("I hit something trigger");
        if (other.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("I Collected something");
            Destroy(other.gameObject);
            collectablesStored++;
        }
    }
}
