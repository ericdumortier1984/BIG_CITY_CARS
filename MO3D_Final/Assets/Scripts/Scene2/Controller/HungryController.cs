using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HungryController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] FastFood fastFood;

	[Header("Time")]
    [SerializeField] private Slider patienceBar;
    [SerializeField] private float patienceMultiplicator;

    [Header("Order")]
    // LISTAS DE SPRITES QUE DESEA CADA HUNGRY PEOPLE
	[SerializeField] private List<Sprite> drinkSprites;
	[SerializeField] private List<Sprite> foodSprites;
	[SerializeField] private List<Sprite> sauceSprites;
	[SerializeField] private List<Sprite> dessertSprites;
 
	// IMAGENES DE LO QUE DESEA CADA HUNGRY PEOPLE
	[SerializeField] private Image drinkOrderImage;
	[SerializeField] private Image foodOrderImage;
	[SerializeField] private Image sauceOrderImage;
	[SerializeField] private Image dessertOrderImage;


	// BOOL
	private bool isOrdering = false;
	private bool deliveredDrink = false;
	private bool deliveredFood = false;
	private bool deliveredSauce = false;
	private bool deliveredDessert = false;

	// FLOAT
	private float currentPatience;
    private float maxPatience = 100f;
    private float minPatience = 0f;

	// INT
	private int correctDeliveries = 0;
	private int deliveriesNeeded = 4;

	// LLAMO DESDE FAST FOOD SCRIPT
	public int drinkID { get; private set; }
	public int foodID { get; private set; }
	public int sauceID { get; private set; }
	public int dessertID { get; private set; }


	private void Start()
    {
        patienceBar.interactable = false;
        patienceBar.maxValue = maxPatience;
        currentPatience = maxPatience;
        patienceBar.minValue = minPatience;
	}

	private void Update()
	{
        UpdatePatienceBar();
	}

	public void StartOrdering()
    {
		deliveredDrink = deliveredFood = deliveredSauce = deliveredDessert = false;

		drinkOrderImage.color = Color.white;
		foodOrderImage.color = Color.white;
		sauceOrderImage.color = Color.white;
		dessertOrderImage.color = Color.white;

		correctDeliveries = 0;

		// IDs POR RANGOS
		drinkID = Random.Range(0, drinkSprites.Count);          
		foodID = Random.Range(10, 10 + foodSprites.Count);      
		sauceID = Random.Range(20, 20 + sauceSprites.Count);    
		dessertID = Random.Range(30, 30 + dessertSprites.Count); 

		drinkOrderImage.sprite = drinkSprites[drinkID - 0];
		foodOrderImage.sprite = foodSprites[foodID - 10];
		sauceOrderImage.sprite = sauceSprites[sauceID - 20];
		dessertOrderImage.sprite = dessertSprites[dessertID - 30];

		drinkOrderImage.gameObject.SetActive(true);
		foodOrderImage.gameObject.SetActive(true);
		sauceOrderImage.gameObject.SetActive(true);
		dessertOrderImage.gameObject.SetActive(true);

		// RESETEO BARRA DE PACIENCIA
		currentPatience = maxPatience;

		isOrdering = true;
	}

    public void StopOrdering()
    {
		isOrdering = false;

		drinkOrderImage.gameObject.SetActive(false);
		foodOrderImage.gameObject.SetActive(false);
		sauceOrderImage.gameObject.SetActive(false);
		dessertOrderImage.gameObject.SetActive(false);

		fastFood.NextHungryPeople();
	}

    private void UpdatePatienceBar()
    {
		if (!isOrdering) return;

		currentPatience -= Time.deltaTime * patienceMultiplicator;
		patienceBar.value = currentPatience;

		if (currentPatience <= minPatience)
		{
			fastFood.LoseMission();
		}
	}

	public void ReceiveFood(int id)
	{
		if (!isOrdering) return;

		// DRINK
		if (id == drinkID && !deliveredDrink)
		{
			deliveredDrink = true;
			drinkOrderImage.color = new Color(1, 1, 1, 0.3f);
			correctDeliveries++;
		}
		// FOOD
		else if (id == foodID && !deliveredFood)
		{
			deliveredFood = true;
			foodOrderImage.color = new Color(1, 1, 1, 0.3f);
			correctDeliveries++;
		}
		// SAUCE
		else if (id == sauceID && !deliveredSauce)
		{
			deliveredSauce = true;
			sauceOrderImage.color = new Color(1, 1, 1, 0.3f);
			correctDeliveries++;
		}
		// DESSERT
		else if (id == dessertID && !deliveredDessert)
		{
			deliveredDessert = true;
			dessertOrderImage.color = new Color(1, 1, 1, 0.3f);
			correctDeliveries++;
		}
		else
		{
			Debug.Log("WRONG FOOD");
			return;
		}

		if (correctDeliveries >= deliveriesNeeded)
		{
			StopOrdering();
		}
	}
}
