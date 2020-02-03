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

    bool isChoiseParty;

    PointerEventData pointer;

    // Start is called before the first frame update
    void Start()
    {
        CreateCharaButton();
        pointer = new PointerEventData(EventSystem.current);
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
                clickedGameObject.GetComponent<OrgChara>().id = choise.GetComponent<OrgChara>().id;
                clickedGameObject.GetComponent<Image>().sprite = choise.GetComponent<OrgChara>().sprite;
                choise.GetComponent<OrgChara>().UnChoseColer();
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
            if (isChoiseParty)
            {
                choise.GetComponent<OrgChara>().id = clickedGameObject.GetComponent<OrgChara>().id;
                choise.GetComponent<Image>().sprite = clickedGameObject.GetComponent<OrgChara>().sprite;
                choise.GetComponent<OrgChara>().UnChoseColer();
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
        for (int i = 0; i < 12; i++)
        {
            GameObject obj = Instantiate(charaButton, new Vector3(-1.5f + ((i % 3) * 1.5f), 0 - 1.5f * (int)(i / 3)), Quaternion.identity);
            obj.GetComponent<OrgChara>().id = i + 1;
            if (characters.Count >= i)
                obj.GetComponent<Image>().sprite = characters[i];
            obj.GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
            obj.transform.SetParent(GameObject.Find("Content").transform);
        }
    }
}
