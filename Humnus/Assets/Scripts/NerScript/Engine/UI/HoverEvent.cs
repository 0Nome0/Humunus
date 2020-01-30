using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NerScript;
using NerScript.Attribute;


public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    [SerializeField, ReadOnlyOnInspector] private bool isHover = false;

    [SerializeField] private bool useScale = false;
    [SerializeField, ShowIf("useScale")] private Vector3 scale = default;
    [SerializeField, ShowIf("useScale")] private float scalingPower = 0.1f;
    private Vector3 oldScale = default;
    private Vector3 targetScale = default;
    private bool scaling = false;



    private void Start()
    {
        oldScale = transform.localScale;
    }


    private void Update()
    {
        if (useScale && scaling)
        {
            transform.localScale += (targetScale - transform.localScale) * scalingPower;
            if ((targetScale - transform.localScale).magnitude <= 0.01f)
            {
                transform.localScale = targetScale;
                scaling = false;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;

        if (useScale)
        {
            if (!scaling) oldScale = transform.localScale;
            scaling = true;
            targetScale.x = oldScale.x * scale.x;
            targetScale.y = oldScale.y * scale.y;
            targetScale.z = oldScale.z * scale.z;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        if (useScale)
        {
            scaling = true;
            targetScale = oldScale;
        }

    }
}
