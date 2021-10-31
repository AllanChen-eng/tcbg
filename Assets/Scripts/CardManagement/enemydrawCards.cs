using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public bool GameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnClick()
    {
        if (GameStarted == false)
        {
            for (int x = 0; x < 5; x++)
            {
                GameObject playerCard = Instantiate(Card1, new Vector3(0, 0, 0), Quaternion.identity);
                playerCard.transform.SetParent(EnemyArea.transform, false);
                GameStarted = true;
            }
        }
        else
        {
            GameObject playerCard = Instantiate(Card1, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(EnemyArea.transform, false);
        }
    }

}
