using UnityEngine;
using UnityEngine.UI;
using System;

// Randomizes customer orders and displays it in ordersUI text
public class OrdersText : MonoBehaviour
{
    public Text text;

    // Arrays of possible cake layers, flavors, toppings
    // Can add to it later in game
    private static string[] layers = {"one", "two", "three"};
    private static string[] flavor = { "vanilla", "chocolate", "strawberry" };
    private static string[] toppings = { "strawberries\n", "blueberries\n", "cherries\n", "whipped cream\n", "candy\n", "sprinkles\n", "\n" };

    void Start()
    {
        // Allows randomization to be different every time the game runs
        int seed = DateTime.Now.Ticks.GetHashCode() ^ gameObject.GetInstanceID();;
        System.Random random = new System.Random(seed);

        // Randomly select layer and flavor from lists
        string randomLayer = layers[random.Next(0, layers.Length)];
        string randomFlavor = flavor[random.Next(0, flavor.Length)];

        // Randomly select 1 to 3 toppings and shuffle the order they appear in
        Shuffle(toppings, random);
        string randomToppings = string.Join("", toppings, 0, 2);

        // Output orders text with randomized items
        text.text = $"{randomFlavor} cake\n\n{randomLayer}";
        if (randomLayer == "two" || randomLayer == "three")
        {
            text.text += " layers";
        }
        else
        {
            text.text += " layer";
        }
        text.text += $"\n{randomFlavor} icing\n\nmust have:\n{randomToppings}\n\n";
    }

    // Shuffle the items in the toppings array
    private void Shuffle<T>(T[] array, System.Random random)
    {
        int n = array.Length;
        // Unless there is one item
        while (n > 1)
        {
            int k = random.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
