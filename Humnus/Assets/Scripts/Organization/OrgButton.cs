using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrgButton : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;

    Image thisImage;
    Image startImage;

    public void CanStart()
    {
        thisImage.sprite = sprites[0];
        startImage.sprite = sprites[1];
        GetComponent<Button>().enabled = true;
    }

    public void CantStart()
    {
        thisImage.sprite = sprites[2];
        startImage.sprite = sprites[3];
        GetComponent<Button>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisImage = GetComponent<Image>();
        startImage = transform.GetChild(0).GetComponent<Image>();
        CantStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
