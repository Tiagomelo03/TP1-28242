using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    [Header("Checkpoints")]
    public GameObject start;
    public GameObject end;
    public GameObject[] checkpoints;

    [Header("Settings")]
    public float laps = 1;

    [Header("UI Elements")]
    public Text currentTimeText;
    public Text bestTimeText;
    public Text checkpointWarningText;
    public Text finalMessageText;
    public GameObject dialogPanel;
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    public Text speedText; 

    [Header("Information")]
    private float currentCheckpoint;
    private float currentLap;
    private bool started;
    private bool finished;
    private bool isPaused;
    

    private float currentLapTime;
    private float bestLapTime;
    private float bestLap;

    private string checkpointWarning = "";

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Rigidbody carRigidbody;

    private void Start()
    {
        currentCheckpoint = 0;
        currentLap = 1;

        started = false;
        finished = false;

        currentLapTime = 0;

        bestLapTime = PlayerPrefs.GetFloat("BestLapTime", 0);
        bestLap = PlayerPrefs.GetFloat("BestLap", 0);

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        finalMessageText.gameObject.SetActive(false);
        dialogPanel.SetActive(false);
        
        carRigidbody = GetComponent<Rigidbody>(); // Obtém a referência ao Rigidbody

        UpdateUI();
    }

    private void Update()
    {
        if (started && !finished)
        {
            currentLapTime += Time.deltaTime;

            if (bestLap == 0)
            {
                bestLap = 1;
            }

            UpdateUI();
        }

        if (started && bestLap == currentLap)
        {
            bestLapTime = currentLapTime;
            UpdateUI();
        }

         if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowFinishMenu();
        }
        UpdateSpeedUI();
    }

    private void UpdateUI()
    {
        string formattedCurrentTime = $"Atual                      {Mathf.FloorToInt(currentLapTime / 60)}:{(currentLapTime % 60):00.000}";
        currentTimeText.text = formattedCurrentTime;

        string formattedBestTime = $"Melhor                   {Mathf.FloorToInt(bestLapTime / 60)}:{(bestLapTime % 60):00.000}";
        bestTimeText.text = formattedBestTime;

        checkpointWarningText.text = checkpointWarning;

        if (finished)
        {
            finalMessageText.text = "Corrida Finalizada";
            finalMessageText.gameObject.SetActive(true);
            dialogPanel.SetActive(true);
        }

    }

     private void UpdateSpeedUI()
    {
        if (carRigidbody != null && speedText != null)
        {
            float speed = carRigidbody.velocity.magnitude * 3.6f; // Convertendo de m/s para km/h
            speedText.text = $"{Mathf.RoundToInt(speed)}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            GameObject thisCheckpoint = other.gameObject;

            if (thisCheckpoint == start && !started)
            {
                Debug.Log("Started");
                started = true;
            }
            else if (thisCheckpoint == end && started)
            {
                if (currentLap == laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime || bestLapTime == 0)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime;
                            PlayerPrefs.SetFloat("BestLapTime", bestLapTime);
                            PlayerPrefs.SetFloat("BestLap", bestLap);
                        }

                        finished = true;
                        Debug.Log("Finished");
                        ShowFinishMenu();
                    }
                    else
                    {
                        Debug.Log("Did not go through all checkpoints");
                    }
                }
                else if (currentLap < laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime || bestLapTime == 0)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime;
                            PlayerPrefs.SetFloat("BestLapTime", bestLapTime);
                            PlayerPrefs.SetFloat("BestLap", bestLap);
                        }

                        currentLap++;
                        currentCheckpoint = 0;
                        currentLapTime = 0;
                        Debug.Log($"Started lap {currentLap} - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    }
                }
                else
                {
                    Debug.Log("Did not go through all the checkpoints");
                }
            }

            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (finished)
                    return;

                if (thisCheckpoint == checkpoints[i] && i == currentCheckpoint)
                {
                    Debug.Log($"Correct checkpoint - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                    checkpointWarning = "";
                }
                else if (thisCheckpoint == checkpoints[i] && i != currentCheckpoint)
                {
                    Debug.Log("Incorrect checkpoint");
                    checkpointWarning = "SENTIDO ERRADO!";
                }
            }

            UpdateUI();
        }
    }

    private void ShowFinishMenu()
    {

        // Exibe o menu de fim de corrida
        dialogPanel.SetActive(true);

        // Configura os botões
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartRace);
        menuButton.onClick.AddListener(QuitRace);
    }

    private void ResumeGame()
    {
        isPaused = false;

        // Esconde o menu de fim de corrida
        dialogPanel.SetActive(false);
    }


    private void RestartRace()
    {
        // Reinicia a corrida
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitRace()
    {
        // Volta ao menu
        SceneManager.LoadSceneAsync(0); // Substitua "MenuSceneName" pelo nome da cena do menu
    }
}
