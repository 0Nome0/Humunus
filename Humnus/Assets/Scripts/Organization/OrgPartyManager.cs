using System.Collections;
using System.Collections.Generic;
using NerScript;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrgPartyManager : MonoBehaviour
{
    GameObject clickedGameObject;
    OrgChara clickOrg;

    [SerializeField]
    GameObject charaButton;
    [SerializeField]
    List<Sprite> characters;
    [SerializeField]
    OrgButton button;
    [SerializeField]
    List<int> dontUseId;


    //キャラデータベースを参照
    public ScriptableObjectDatabase database = null;

    OrgChara[] deck = new OrgChara[2];
    List<OrgChara> orgCharas = new List<OrgChara>();

    PointerEventData pointer;

    // Start is called before the first frame update
    void Start()
    {
        CreateCharaButton();
        pointer = new PointerEventData(EventSystem.current);
        deck = GameObject.Find("Party").GetComponentsInChildren<OrgChara>();
        foreach (var x in deck)
            x.id = 100;

        //deck[0].id = (int)PlayCharacter.Left;
        //deck[1].id = (int)PlayCharacter.Right;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Click();
            }
        }
        else
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Click();
            }
        }
    }

    void Click()
    {
        clickedGameObject = null;

        List<RaycastResult> results = new List<RaycastResult>();
        if (Application.isEditor)
            pointer.position = Input.mousePosition;
        else
            pointer.position = Input.GetTouch(0).position;
        EventSystem.current.RaycastAll(pointer, results);

        if (results.Count == 0)
            return;

        GameObject hit2d = results[0].gameObject;
        foreach (var x in results)
        {
            if (x.gameObject.name.Contains("Button"))
            {
                hit2d = x.gameObject;
                break;
            }
        }

        if (hit2d)
        {
            clickedGameObject = hit2d.transform.gameObject;
        }

        if (!(clickedGameObject.name == "Button"))
            return;

        clickOrg = clickedGameObject.GetComponent<OrgChara>();

        if (clickOrg.cantUse)
            return;

        if (!clickOrg.canChoise)
        {
            foreach (var x in deck)
            {
                if (x.id == clickOrg.id)
                {
                    x.transform.parent.GetComponent<Image>().sprite = x.sprite;
                    clickOrg.CanSelect();
                    foreach (var y in orgCharas)
                    {
                        if (y.id != deck[0].id && y.id != deck[1].id)
                        {
                            if (y.cantUse)
                                continue;
                            y.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                            button.CantStart();
                        }
                    }
                    x.id = 100;
                    return;
                }
            }
        }

        if (deck[0].id != 100 && deck[1].id != 100)
        {
            return;
        }

        foreach (var x in deck)
        {
            if (x.id <= orgCharas.Count)
                continue;

            x.id = clickOrg.id;
            x.transform.parent.GetComponent<Image>().sprite = clickOrg.sprite;
            clickOrg.CantSelect();
            break;
        }
        PlayCharacter.Left = (PlayerID)deck[0].id;
        PlayCharacter.Right = (PlayerID)deck[1].id;
        //選択されたキャラをセット

        if (deck[0].id != 100 && deck[1].id != 100)
        {
            foreach (var x in orgCharas)
            {
                if (x.id != deck[0].id && x.id != deck[1].id)
                {
                    x.gameObject.GetComponent<Image>().color -= new Color(0.5f, 0.5f, 0.5f, 0);
                    button.CanStart();
                }
            }
        }
    }

    void CreateCharaButton()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            GameObject obj = Instantiate(charaButton,GameObject.Find("Content").transform);

            obj = obj.transform.GetChild(0).gameObject;
            obj.GetComponent<OrgChara>().id = i;
            //ローカル座標をGridLayoutComponentに委託
            obj.GetComponent<Image>().sprite = characters[i];

            if (!database.GetObjectByID<CharacterData>(i).isOpened)
                obj.GetComponent<OrgChara>().CantUse();//使用不可をデータベース引用に変更

            orgCharas.Add(obj.GetComponent<OrgChara>());
        }
    }
}
