using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropForCards : MonoBehaviour
{
    public GameObject card1;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    private GameObject mainCanvas;
    private GameObject startParent;
    List<GameObject> graveyardDeck = new List<GameObject>();

    private void Start()
    {
        GameObject mainCanvas = GameObject.Find("Canvas");

        //GameObject myObject2 = gameObject.transform.GetChild(0).gameObject;

        //GameObject myObject3 = GameObject.FindGameObjectWithTag("GameObject Tag").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            GameObject mainCanvas = GameObject.Find("Canvas");
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(mainCanvas.transform, true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;

    }
    public void StartDrag()
    {
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            // what happens when the card is played
            Destroy(this);
            Debug.Log("trigger to decide what happens!");
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}
