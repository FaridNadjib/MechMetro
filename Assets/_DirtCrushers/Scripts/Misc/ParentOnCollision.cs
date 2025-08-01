using UnityEngine;

public class ParentOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")) { if (GameManager.Instance.Player.parent == null) { GameManager.Instance.Player.parent = transform; } }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) { if (GameManager.Instance.Player.parent != null) { GameManager.Instance.Player.parent = null; } }
    }
}
