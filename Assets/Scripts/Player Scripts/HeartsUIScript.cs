using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsUIScript : MonoBehaviour
{
    // variables ^_^
    public int maxHealth = 5;
    public int currentHealth;
    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(i *60, 0); // change da value to increase/decrease space between hearts
            hearts.Add(heart);
        }
    }

    public void UpdateHealth(int health)
    {
        currentHealth = health;
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }
}
