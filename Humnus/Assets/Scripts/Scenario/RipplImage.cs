using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RipplImage : MonoBehaviour
{
    [SerializeField]
    private Image commentImage = null;
    [SerializeField]
    private float rippling = 0.3f;

    private float currentrippling;
    private Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (commentImage == null)
            commentImage = GetComponent<Image>();
        currentrippling = rippling;
        initPosition = transform.localPosition;
    }

    public void Initialize()
    {
        commentImage.enabled = false;
        currentrippling = rippling;
        transform.localPosition = initPosition;
    }    
    
    public void EndMeasures()
    {
        commentImage.enabled = true;
        currentrippling -= Time.deltaTime;
        if (Vector3.Distance(transform.localPosition, initPosition) <= 0.25f)
        {
            transform.localPosition = initPosition;
            currentrippling = rippling;
        }
        transform.localPosition += new Vector3(0, currentrippling, 0);
    }
}
