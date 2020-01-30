using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NerScript;

public class MoveMouseToTransform : MonoBehaviour
{
    private InputSystemAPI inputAPI = new InputSystemAPI();
    [SerializeField] private float moveScale = 0.2f;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inputAPI.LeftClick)
            transform.AddPosX(inputAPI.MouseMoveDistance.x * moveScale);
    }
}
