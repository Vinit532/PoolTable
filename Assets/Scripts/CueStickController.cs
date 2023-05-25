using UnityEngine;

public class CueStickController : MonoBehaviour
{
    public LineRenderer guidingLineRenderer;
    public Transform cueTip;
    public float guidingLineLength = 10f;

    void Update()
    {
        Vector3 guidingLineStart = cueTip.position;
        Vector3 guidingLineEnd = guidingLineStart + cueTip.forward * guidingLineLength;

        guidingLineRenderer.SetPosition(0, guidingLineStart);
        guidingLineRenderer.SetPosition(1, guidingLineEnd);
    } 
}
