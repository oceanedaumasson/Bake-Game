using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Deals with bakery opening and closing
public class OpenClose : MonoBehaviour, IPointerClickHandler
{
    // Set open and closed sign objects in Unity
    public GameObject openSign;
    public GameObject closedSign;
    public static bool isClosed { get; private set; } = false;

    private static OpenClose instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep object throughout scenes
    }

    void Start()
    {
        // Set sign to open on start
        openSign.SetActive(true); // Show open sign
        closedSign.SetActive(false); // Hide closed sign
    }

    // Allow player to set sign to closed or open when clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClosed)
        {
            closedSign.SetActive(true);
            openSign.SetActive(false);
            // Wait for customers to leave before moving to menu screen
            StartCoroutine(WaitForCustomersToLeave());
        }
        else
        {
            closedSign.SetActive(false);
            openSign.SetActive(true);
            isClosed = !isClosed; // Set sign back to open
        }
    }

    private IEnumerator WaitForCustomersToLeave()
    {
        // Deals with money text, adds up money after one game loop/day
        MoneyManager.Instance.AddUpTotalMoney();

        // Wait until there are no more customers in the bakery
        while (FindObjectsOfType<CustomerScript>().Length > 0)
        {   
            yield return new WaitForSeconds(1f); 
        }

        // Change to the menu scene
        SceneManager.LoadScene("Menu");
    }
}