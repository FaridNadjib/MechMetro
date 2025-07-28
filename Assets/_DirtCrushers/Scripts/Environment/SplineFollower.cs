using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    [SerializeField] SplineContainer path;
    [SerializeField] Transform followObject;
    [SerializeField] float duration = 10f;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration) timer = 0f;
        followObject.position = transform.position + (Vector3)path.Spline.EvaluatePosition(timer/duration);

        Vector3 tangent = path.Spline.EvaluateTangent(timer / duration);

        // Optional: define a consistent up vector
        Vector3 up = Vector3.up;

        // Calculate rotation from tangent
        Quaternion rotation = Quaternion.LookRotation(tangent, followObject.up);
        followObject.rotation = rotation;
    }
}
