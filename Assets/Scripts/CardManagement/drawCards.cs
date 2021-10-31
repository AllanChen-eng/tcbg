using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public bool GameStarted = false;
    List<GameObject> cards = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
       cards.Add(Card1); 
    }
    public void OnClick()
    {
        if (GameStarted == false)
        {
            for (int x = 0; x < 5; x++)
            {
                GameObject playerCard = Instantiate(cards[Random.Range(0,cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
                playerCard.transform.SetParent(PlayerArea.transform, false);
                GameStarted = true;
            }
        }
        else
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);
        }
    }

}
