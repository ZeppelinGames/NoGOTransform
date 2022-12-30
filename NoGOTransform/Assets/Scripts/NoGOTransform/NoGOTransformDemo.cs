using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGOTransformDemo : MonoBehaviour
{
    [SerializeField] private NoGOTransform t;
    [SerializeField] private List<NoGOTransform> transforms = new List<NoGOTransform>();

    private void OnDrawGizmos()
    {
        if (transforms.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < transforms.Count - 1; i++)
            {
                Gizmos.DrawLine(transforms[i].position, transforms[i + 1].position);
            }
        }
    }
}
