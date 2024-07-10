using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLoader : MonoBehaviour
{
    private void Start()
    {
        // Carregar a seleção do carro
        int selectedCar = PlayerPrefs.GetInt("SelectedCar", 0);

        // Desativar todos os carros
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        // Ativar o carro selecionado
        transform.GetChild(selectedCar).gameObject.SetActive(true);
    }
}

