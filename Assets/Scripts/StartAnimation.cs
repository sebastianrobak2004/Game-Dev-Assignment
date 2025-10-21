using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] private Transform slime;
    [SerializeField] private Transform ghost;

    [SerializeField] private Transform c1;
    [SerializeField] private Transform c2;
    [SerializeField] private Transform c3;
    [SerializeField] private Transform c4;

    [SerializeField] private float speed = 2f;

    private List<Transform> corners;
    private int slimeCornerIndex = 0;
    private int ghostCornerIndex = 0;

    void Start()
    {
        corners = new List<Transform> { c1, c2, c3, c4 };
    }

    void Update()
    {
        MoveBetweenCorners(slime, ref slimeCornerIndex);
        MoveBetweenCorners(ghost, ref ghostCornerIndex);
    }

    private void MoveBetweenCorners(Transform obj, ref int cornerIndex)
    {
        Transform target = corners[cornerIndex];
        obj.position = Vector3.MoveTowards(obj.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(obj.position, target.position) < 0.01f)
        {
            cornerIndex = (cornerIndex + 1) % corners.Count;
            obj.Rotate(0f, 0f, 90f);

        }
    }
}
