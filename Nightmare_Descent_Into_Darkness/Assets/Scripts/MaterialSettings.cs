using UnityEngine;

public class MaterialSettings : MonoBehaviour
{
    public Color startColor = Color.red;
    public Color endColor = Color.blue;
    public ColorInterpolationType interpolationType = ColorInterpolationType.Linear;

    public Vector3 startPos;
    public Vector3 endPos;

}
