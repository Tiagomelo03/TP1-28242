using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimacao : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    private Vector3 inicialPosition;

    private void Awake(){

        inicialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1F);
        
    }

    private void OnDisable()
    {
        transform.position = inicialPosition;
    }
}
