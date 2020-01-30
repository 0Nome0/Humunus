using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ScenarioCharacter
{
    [SerializeField, Header("FacePosition")]
    private Vector3 facePosition = Vector3.zero;
    [SerializeField, Header("FaceScale")]
    private Vector2 faceScale = Vector2.zero;
    [SerializeField]
    private CharacterType type = CharacterType.None;
    [SerializeField, Header("立ち絵")]
    private Sprite standImage = null;
    [SerializeField, Header("表情差分")]
    private List<CharacterEmotion> emotionList;

    public Vector3 FacePosition => facePosition;
    public Vector2 FaceScale => faceScale;
    public CharacterType Type => type;
    public Sprite StandImage => standImage;
    public List<CharacterEmotion> EmotionList => emotionList;
}

[Serializable]
public class CharacterEmotion
{
    public EmotionType type = EmotionType.Normal;
    public Sprite faceImage = null;
}
