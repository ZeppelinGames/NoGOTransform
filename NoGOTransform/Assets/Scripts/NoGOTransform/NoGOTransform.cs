using System.Collections;
using UnityEngine;

[System.Serializable]
public class NoGOTransform
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = new Quaternion(1, 1, 1, 1);
    public Vector3 scale = Vector3.one;

    public Vector3 eulerAngles
    {
        get
        {
            return this.rotation.eulerAngles;
        }
        set
        {
            this.rotation.eulerAngles = value;
        }
    }
    public Vector3 forward
    {
        get
        {
            return this.rotation * Vector3.forward;
        }
    }
    public Vector3 right
    {
        get
        {
            return this.rotation * Vector3.right;
        }
    }
    public Vector3 up
    {
        get
        {
            return this.rotation * Vector3.up;
        }
    }
}