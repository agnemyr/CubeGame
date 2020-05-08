using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private const float XToBarOffset = 0.73f; // 1.07
    private readonly Dictionary<float, float> _zPosToProgressBarOffs = new Dictionary<float, float>{{9, 0}, {6, -0.01f}, {3, -0.02f } };
    private const float ZToOutlinedBarOffset = 0.01f;

    public void UpdateBar(float completed)
    {
        var index = (int) Math.Floor(completed / (1.0 / (transform.childCount + 1)) - 1);
        // Progress in opposite dir on upper map
        if (transform.parent.transform.position.x > 0) index = transform.childCount - index - 1;
        if (index >= 0 && index < transform.childCount) transform.GetChild(index).gameObject.SetActive(true);
    }

    public void StopDisplaying()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void AlignProgressBar(Vector3 position)
    {
        var xPos = position.x > 0 ? XToBarOffset : -XToBarOffset;
        var zProgBarPos = position.z > 0 ? _zPosToProgressBarOffs[position.z] : _zPosToProgressBarOffs[-position.z];
        transform.localPosition = new Vector3(xPos, 0.6f, zProgBarPos);
        var outlineZPos = position.z > 0 ? ZToOutlinedBarOffset : -ZToOutlinedBarOffset; 
        transform.parent.GetChild(2).localPosition = new Vector3(xPos, 0.6f, outlineZPos);
    }
    
}
