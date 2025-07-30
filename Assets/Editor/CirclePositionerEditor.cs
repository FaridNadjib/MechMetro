using UnityEditor;
using UnityEngine;

public class CirclePositionerEditor : EditorWindow
{
    private float radius = 1f;
    private Vector3 axis = Vector3.up;
    private GameObject parent;

    [MenuItem("Tools/Circle Positioner")]
    public static void ShowWindow()
    {
        GetWindow<CirclePositionerEditor>("Circle Positioner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Arrange Children in Circle", EditorStyles.boldLabel);

        parent = (GameObject)EditorGUILayout.ObjectField("Parent Object", parent, typeof(GameObject), true);
        radius = EditorGUILayout.FloatField("Radius", radius);
        axis = EditorGUILayout.Vector3Field("Rotation Axis", axis);

        if (GUILayout.Button("Arrange"))
        {
            if (parent == null)
            {
                Debug.LogWarning("Please assign a parent GameObject.");
                return;
            }

            ArrangeChildrenInCircle();
        }
    }

    private void ArrangeChildrenInCircle()
    {
        int count = parent.transform.childCount;
        if (count == 0)
        {
            Debug.LogWarning("No children to arrange.");
            return;
        }

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            Transform child = parent.transform.GetChild(i);
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 localPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius; // get a point on the circle in local space
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, axis.normalized);
            Vector3 rotatedPos = rotation * localPos;
            child.localPosition = rotatedPos;
        }
    }
}
