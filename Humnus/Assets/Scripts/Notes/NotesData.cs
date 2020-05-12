using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniJSON;
using NerScript.Attribute;
using UnityEngine;

public class NotesData : ScriptableObject
{
    public List<Notes> notes = null;
    public string notesData = "";


    public bool isR = false;
    [InspectorButton("ToNotes", "ToNotes")] public bool jsonToList = false;


    public void ToNotes()
    {
        NotesType ToType(int i)
        {
            if(i == 0) return NotesType.Tap;
            if(i == 1) return NotesType.Slide;
            if(i == 2) return NotesType.Slide;
            if(i == 3) return NotesType.Start;
            if(i == 4) return NotesType.End;
            return NotesType.Tap;
        }

        dynamic list = Json.Deserialize(notesData);

        dynamic list2 = isR ? list["Rlane"] : list["Llane"];

        notes.Clear();
        for(int i = 0; i < list2.Count; i++)
        {
            float time = (float)list2[i]["time"];
            NotesType type = ToType((int)list2[i]["pattern"]);

            notes.Add(new Notes(time, type));
        }
    }

}