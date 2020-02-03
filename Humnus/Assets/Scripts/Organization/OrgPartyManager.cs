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
            if (choise != null && !isChoiseParty)
            {
                if (clickedGameObject.GetComponent<OrgChara>().id <= orgCharas.Count)
                    orgCharas[clickedGameObject.GetComponent<OrgChara>().id].CanSelect();

                clickedGameObject.GetComponent<OrgChara>().id = choise.GetComponent<OrgChara>().id;
                clickedGameObject.GetComponent<Image>().sprite = choise.GetComponent<OrgChara>().sprite;
                choise.GetComponent<OrgChara>().UnChoseColer();
                orgCharas[clickedGameObject.GetComponent<OrgChara>().id].CantSelect();
                choise = null;
            }
            else
            {
                choise = clickedGameObject;
                isChoiseParty = true;
            }
        }
        else
        {
            if (!clickedGameObject.GetComponent<OrgChara>().canChoise)
                return;

            if (isChoiseParty)
            {
                if (choise.GetComponent<OrgChara>().id <= orgCharas.Count)
                    orgCharas[choise.GetComponent<OrgChara>().id].CanSelect();

                choise.GetComponent<OrgChara>().id = clickedGameObject.GetComponent<OrgChara>().id;
                choise.GetComponent<Image>().sprite = clickedGameObject.GetComponent<OrgChara>().sprite;
                choise.GetComponent<OrgChara>().UnChoseColer();
                orgCharas[choise.GetComponent<OrgChara>().id].CantSelect();
                choise = null;
                isChoiseParty = false;
            }
            else
            {
                if (choise != null)
                    choise.GetComponent<OrgChara>().UnChoseColer();
                choise = clickedGameObject;
            }
        }
        if (choise != null)
            choise.GetComponent<OrgChara>().ChoseColor();

    }

    void CreateCharaButton()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            GameObject obj = Instantiate(charaButton, new Vector3(-1.5f + ((i % 3) * 1.5f), 0 - 1.5f * (int)(i / 3)), Quaternion.identity);
            obj.GetComponent<OrgChara>().id = i;
            obj.GetComponent<Image>().sprite = characters[i];
            obj.GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
            obj.transform.SetParent(GameObject.Find("Content").transform);
            orgCharas.Add(obj.GetComponent<OrgChara>());
        }
    }
}
