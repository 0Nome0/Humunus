using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrgChara : MonoBehaviour
{
    public int id { get; set; }
    public Sprite sprite { get; set; }

    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        sprite = image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChoseColor()
    {
        image.color += new Color(0, 0, 0, 0.5f);
    }
    public void UnChoseColer()
    {
        image.color -= new Color(0, 0, 0, 0.5f);
    }
}
