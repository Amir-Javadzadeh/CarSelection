using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class CarSelectionManager : MonoBehaviour
{
    public GameObject[] Cars; // Array of all car GameObjects
    public GameObject[] CarsInfo; // Array of UI panels with car information

    public Button NextButton; // Button to go to the next car
    public Button BackButton; // Button to go to the previous car

    private int carsIndex = 0; // Current selected car index

    public TMP_Text[] CarsMoney; // Price of each car
    public TMP_Text PlayerMoney; // Player's current money (UI text)

    private float currentPlayerMoney; // Player's current money as a float

    public GameObject BuyCarButton; // Button to buy the selected car

    void Start()
    {
        // Load saved money and selected car index
        currentPlayerMoney = PlayerPrefs.GetFloat("MoneyNew", 200000);
        PlayerMoney.text = currentPlayerMoney.ToString();

        carsIndex = PlayerPrefs.GetInt("CarSave", carsIndex);

        // Deactivate all cars and info panels
        for (int i = 0; i < Cars.Length; i++)
        {
            Cars[i].SetActive(false);
            CarsInfo[i].SetActive(false);
        }

        // Activate the currently selected car
        Cars[carsIndex].SetActive(true);
        CarsInfo[carsIndex].SetActive(true);

        UpdateButton();
        UpdateCarNow();
    }

    public void Next()
    {
        if (carsIndex < Cars.Length - 1)
        {
            // Hide current car
            CarsInfo[carsIndex].SetActive(false);
            Cars[carsIndex].SetActive(false);

            // Show next car
            carsIndex++;
            Cars[carsIndex].SetActive(true);
            CarsInfo[carsIndex].SetActive(true);

            UpdateButton();
            UpdateCarNow();
        }
    }

    public void Back()
    {
        if (carsIndex > 0)
        {
            // Hide current car
            CarsInfo[carsIndex].SetActive(false);
            Cars[carsIndex].SetActive(false);

            // Show previous car
            carsIndex--;
            Cars[carsIndex].SetActive(true);
            CarsInfo[carsIndex].SetActive(true);

            UpdateButton();
            UpdateCarNow();
        }
    }

    void UpdateButton()
    {
        // Enable/disable navigation buttons based on current index
        NextButton.gameObject.SetActive(carsIndex < Cars.Length - 1);
        BackButton.gameObject.SetActive(carsIndex > 0);
    }

    public void BuyCar()
    {
        float CarMoneyInt = float.Parse(CarsMoney[carsIndex].text);

        if (currentPlayerMoney >= CarMoneyInt)
        {
            // Deduct money and save selection
            currentPlayerMoney -= CarMoneyInt;
            PlayerMoney.text = currentPlayerMoney.ToString();
            PlayerPrefs.SetInt("CarSave", carsIndex);
            PlayerPrefs.SetFloat("MoneyNew", currentPlayerMoney);
            PlayerPrefs.SetInt("CarNow" + carsIndex, 1);
            PlayerPrefs.Save();

            UpdateCarNow();
        }
        else
        {
            Debug.Log("You don't have enough money to buy this car!");
        }
    }

    public void UpdateCarNow()
    {
        // Check if the selected car is already purchased
        int SaveCarNow = PlayerPrefs.GetInt("CarNow" + carsIndex, 0);
        if (SaveCarNow == 1)
        {
            BuyCarButton.SetActive(false);
            CarsMoney[carsIndex].gameObject.SetActive(false);
        }
        else
        {
            BuyCarButton.SetActive(true);
            CarsMoney[carsIndex].gameObject.SetActive(true);
        }
    }

    public void DeleteSave()
    {
        // Delete all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}