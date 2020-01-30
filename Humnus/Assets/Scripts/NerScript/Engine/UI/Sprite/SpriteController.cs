using System;
using UnityEngine;
using NerScript;
using NerScript.RiValuer;
using UnityEditor.UIElements;

public class SpriteController : MonoBehaviour, IRiValuerDemander
{
    public enum SpriteControlContent
    {
        Color_R,
        Color_G,
        Color_B,
        Color_A,
        FillAmount,
    }

    [SerializeField] private SpriteRenderer sprite = null;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private SpriteControlContent contant = SpriteControlContent.Color_R;
    private Material material = null;
    private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");



    private void SetColor()
    {
        if(sprite != null)
        {
            sprite.material.color = color;
        }
    }

    private void Start()
    {
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();

        if(sprite != null)
        {
            material = sprite.material;
        }
    }

    ValueDataType IRiValuerDemander.ValueType => ValueDataType.Float;

    void IRiValuerDemander.Draw(RiValuerValue value)
    {
        if(material == null) return;
        float f = value.Float;
        switch(contant)
        {

            case SpriteControlContent.Color_R:
                color.SetRed(f);
                SetColor();
                break;
            case SpriteControlContent.Color_G:
                color.SetGreen(f);
                SetColor();
                break;
            case SpriteControlContent.Color_B:
                color.SetBlue(f);
                SetColor();
                break;
            case SpriteControlContent.Color_A:
                color.SetAlpha(f);
                SetColor();
                break;
            case SpriteControlContent.FillAmount:
                material.SetFloat(FillAmount, f);
                break;
        }
    }
}
