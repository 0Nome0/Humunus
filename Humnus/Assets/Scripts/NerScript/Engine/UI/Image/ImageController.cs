using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NerScript;
using NerScript.RiValuer;

public class ImageController : MonoBehaviour, IRiValuerDemander
{
    public enum ImageControlContent
    {
        Color_R,
        Color_G,
        Color_B,
        Color_A,
        FillAmount,
    }

    [SerializeField] private Image image = null;
    [SerializeField] private ImageControlContent content = ImageControlContent.Color_R;

    private void Start()
    {
        if (image == null) image = GetComponent<Image>();
    }


    ValueDataType IRiValuerDemander.ValueType => ValueDataType.Float;

    void IRiValuerDemander.Draw(RiValuerValue value)
    {
        float f = value.Float;
        switch (content)
        {
            case ImageControlContent.Color_R: image.color = image.color.SetedRed(f); break;
            case ImageControlContent.Color_G: image.color = image.color.SetedGreen(f); break;
            case ImageControlContent.Color_B: image.color = image.color.SetedBlue(f); break;
            case ImageControlContent.Color_A: image.color = image.color.SetedAlpha(f); break;
            case ImageControlContent.FillAmount: image.fillAmount = f; break;
        }
    }
}
