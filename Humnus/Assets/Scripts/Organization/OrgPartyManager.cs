using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrgPartyManager : MonoBehaviour
{
    GameObject choise;
    GameObject clickedGameObject;
    [SerializeField]
    GameObject charaButton;
    [SerializeField]
    List<Sprite> characters;

    OrgChara[] deck = new OrgChara[2];
    List<OrgChara> orgCharas = new List<OrgChara>();

    bool isChoiseParty;

    PointerEventData pointer;

    // Start is called before the first frame update
    void Start()
    {
        CreateCharaButton();
        pointer = new PointerEventData(EventSystem.current);
        deck = GameObject.Find("Party").GetComponentsInChildren<OrgChara>();
        foreach (var x in deck)
            x.id = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        clickedGameObject = null;

        List<RaycastResult> results = new List<RaycastResult>();
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, results);

        if (results.Count == 0)
            return;

        var hit2d = results[0].gameObject;

        if (hit2d)
        {
            clickedGameObject = hit2d.transform.gameObject;
        }

        if (!clickedGameObject.name.Contains("Button"))
            return;
        

        if (clickedGameObject.transform.parent.name == "Party")
        {
        }
        else
        {
            if (!clickedGameObject.GetComponent<OrgChara>().canChoise)
            {
                foreach (var x in deck)
                {
                    if (x.id == clickedGameObject.GetComponent<OrgChara>().id)
                    {
                        x.GetComponent<Image>().sprite = x.sprite;
                        clickedGameObject.GetComponent<OrgChara>().CanSelect();
                        foreach (var y in orgCharas)
                        {
                            if (y.id != deck[0].id && y.id != deck[1].id)
                                y.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        }
                        x.id = 100;
                        return;
                    }
                }
            }
            
            choise = clickedGameObject;

            foreach (var x in deck)
            {
                if (x.id <= orgCharas.Count)
                    continue;

                x.id = choise.GetComponent<OrgChara>().id;
                x.gameObject.GetComponent<Image>().sprite = choise.GetComponent<OrgChara>().sprite;
                choise.GetComponent<OrgChara>().CantSelect();
                choise = null;
                break;
            }
        }

        if (deck[0].id != 100 && deck[1].id != 100)
        {
            foreach (var x in orgCharas)
            {
                if (x.id != deck[0].id && x.id != deck[1].id)
                    x.gameObject.GetComponent<Image>().color -= new Color(1, 1, 1, 0);
            }
        }
    }

    void CreateCharaButton()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            GameObject obj = Instantiate(charaButton, new Vector3(-1.5f + ((i % 3) * 1.5f), 0 - 1.5f * (int)(i / 3)), Quaternion.identity);
            obj.GetComponent<OrgChara>().id = i;
            obj.GetComponent<Image>().sprite = characters[i];
            obj.transform.SetParent(GameObject.Find("Content").transform);
            orgCharas.Add(obj.GetComponent<OrgChara>());
        }
    }
}
