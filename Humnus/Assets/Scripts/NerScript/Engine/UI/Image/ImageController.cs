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
        if(image == null) image = GetComponent<Image>();
    }

    public float Value { get => GetValue(); set => SetValue(value); }


    ValueDataType IRiValuerDemander.ValueType => ValueDataType.Float;

    void IRiValuerDemander.Draw(RiValuerValue value)
    {
        Value = value.Float;
    }

    public float GetValue()
    {
        switch(content)
        {
            case ImageControlContent.Color_R:
                return image.color.r;
            case ImageControlContent.Color_G:
                return image.color.g;
            case ImageControlContent.Color_B:
                return image.color.b;
            case ImageControlContent.Color_A:
                return image.color.a;
            case ImageControlContent.FillAmount:
                return image.fillAmount;
        }
        return 0;
    }

    public void SetValue(float f)
    {
        switch(content)
        {
            case ImageControlContent.Color_R:
                image.color = image.color.SetedRed(f);
                break;
            case ImageControlContent.Color_G:
                image.color = image.color.SetedGreen(f);
                break;
            case ImageControlContent.Color_B:
                image.color = image.color.SetedBlue(f);
                break;
            case ImageControlContent.Color_A:
                image.color = image.color.SetedAlpha(f);
                break;
            case ImageControlContent.FillAmount:
                image.fillAmount = f;
                break;
        }
    }
}