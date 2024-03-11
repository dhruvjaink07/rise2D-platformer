using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    public TMP_Text coinText;
    public int currentCoins = 0;
    public TMP_Text endText; // Reference to the TextMesh Pro text to unhide
    public ParticleSystem confetti; // Reference to the confetti particle system

    // References to Toggle UI elements for special coins
    public Toggle toggleA;
    public Toggle toggleB;
    public Toggle toggleC;
    public Toggle toggleD;

    private bool debugLogged = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        coinText.text = currentCoins.ToString();
    }

    void Update()
    {
        // Check after 5 seconds
        if (!debugLogged && Time.timeSinceLevelLoad >= 15f)
        {
            Debug.Log("Current coins: " + currentCoins);
            Debug.Log("Toggle A: " + toggleA.isOn);
            Debug.Log("Toggle B: " + toggleB.isOn);
            Debug.Log("Toggle C: " + toggleC.isOn);
            Debug.Log("Toggle D: " + toggleD.isOn);
            debugLogged = true;
        }
    }

    public void IncreaseCoins(int v, bool isSpecial, string coinTag)
    {
        currentCoins += v;
        coinText.text = currentCoins.ToString();

        // Check if the collected coin is special and update the corresponding Toggle UI element
        if (isSpecial)
        {
            // Find and check the corresponding toggle checkbox based on the coin tag
            if (coinTag == "A" && toggleA != null)
            {
                toggleA.isOn = true;
            }
            else if (coinTag == "B" && toggleB != null)
            {
                toggleB.isOn = true;
            }
            else if (coinTag == "C" && toggleC != null)
            {
                toggleC.isOn = true;
            }
            else if (coinTag == "D" && toggleD != null)
            {
                toggleD.isOn = true;
            }
            else
            {
                Debug.LogError("Invalid special coin tag!");
            }
        }

        // Handle logic for coins (both normal and special)
        if (currentCoins >= 30 && AllSpecialCoinsCollected())
        {
            // Start a coroutine to handle end game actions
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        // Initialize the particle system
        confetti.Play();

        // Unhide the TextMesh Pro text
        endText.gameObject.SetActive(true);

        // Wait for a short delay
        yield return new WaitForSeconds(3f);

        // Load the specified scene
        SceneManager.LoadScene("End");
    }

    bool AllSpecialCoinsCollected()
    {
        // Check if all special coin toggle checkboxes are checked
        return toggleA.isOn && toggleB.isOn && toggleC.isOn && toggleD.isOn;
    }
}
