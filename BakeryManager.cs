using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryManager : MonoBehaviour
{
    public QueueManager queueManager; // Reference QueueManager script

    public GameObject[] customerPrefab; // Public arrays for customer prefabs, set in Unity Scene Manager object
    public int maxCustomers = 6; // Maximum number of customers at a time
    private List<GameObject> currentCustomers = new List<GameObject>(); // List of current customers in bakery
    private static bool customersSpawned = false; 
    private OpenClose openClose; // Reference OpenClose sign script

    void Start()
    {
        // Switch sign to open on start
        openClose = FindObjectOfType<OpenClose>();
        openClose.openSign.SetActive(true);
        openClose.closedSign.SetActive(false);

        // Spawn customers on start, if none are in the scene
        if (!customersSpawned)
        {
            StartCoroutine(InstantiateCustomer());
            customersSpawned = true; // Mark customers as spawned after instantiate coroutine
        }
        StartCoroutine(queueManager.UpdateQueuePositions()); // Update customer positions in queue
    }

    private IEnumerator InstantiateCustomer()
    {
        while (true) //Continuously check if customers can spawn during runtime
        {
            currentCustomers.RemoveAll(customer => customer == null); // Remove destroyed customers from list

            if (currentCustomers.Count < maxCustomers && !OpenClose.isClosed) // Check if the bakery is not closed
            {
                // Randomize customer prefab and instantiate
                int randomIndex = Random.Range(0, customerPrefab.Length);
                GameObject customer = Instantiate(customerPrefab[randomIndex]);

                DontDestroyOnLoad(customer); // Customers persist across scene loads
                currentCustomers.Add(customer); // Add customer to list
                queueManager.Enqueue(customer); // Add customers to queue

                // Immediately reset animator after instantiation to fix no animation problem
                Animator animator = customer.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Rebind();
                    animator.Update(0);
                }
            }

            // Randomize time between customers spawning
            float randomInterval = Random.Range(1f, 2f);
            yield return new WaitForSeconds(randomInterval);
        }
    }

}