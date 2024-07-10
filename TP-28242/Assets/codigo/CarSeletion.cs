using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCarIndex = 0;

    private void Awake()
    {
        // Carregar a seleção do carro anterior se existir
        currentCarIndex = PlayerPrefs.GetInt("SelectedCar", 0);
        SelectCar(currentCarIndex);
    }

    private void SelectCar(int index)
    {
        // Atualizar botões de navegação
        previousButton.interactable = (index > 0);
        nextButton.interactable = (index < transform.childCount - 1);

        // Ativar apenas o carro selecionado
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void ChangeCar(int change)
    {
        int newIndex = currentCarIndex + change;

        // Verificar limites válidos
        if (newIndex >= 0 && newIndex < transform.childCount)
        {
            currentCarIndex = newIndex;
            SelectCar(currentCarIndex);

            // Salvar a seleção do carro
            PlayerPrefs.SetInt("SelectedCar", currentCarIndex);
        }
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
    }
}

