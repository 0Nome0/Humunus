using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrgChara : MonoBehaviour
{
    public int id { get; set; }
    public Sprite sprite { get; set; }
    public bool canChoise { get; private set; }
    public bool cantUse { get; private set; }

    [SerializeField]
    Sprite back;
    [SerializeField]
    Sprite selectedBack;
    
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        sprite = image.sprite;
        canChoise = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void CantSelect()
    {
        transform.parent.GetComponent<Image>().sprite = selectedBack;
        canChoise = false;
    }
    public void CanSelect()
    {
        transform.parent.GetComponent<Image>().sprite = back;
        canChoise = true;
    }
    public void CantUse()
    {
        GetComponent<Image>().color = Color.black;
        canChoise = false;
        cantUse = true;
    }
}
