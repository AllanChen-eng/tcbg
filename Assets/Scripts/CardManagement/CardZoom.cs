using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;
    private GameObject zoomCard;
    private GameObject playerArea;
    private GameObject dropZone;
    private float positionX;
    private float positionY;
    private int sizeOfCardX = 120;
    private int sizeOfCardY = 177;
    private int sizeCompensation = 60; // card is place from the CENTER so need to move up location to compensate when enlarging
    public void Awake()
    {
        canvas = GameObject.Find("Canvas");
        playerArea = GameObject.Find("PlayerArea");
        dropZone = GameObject.Find("DropZone");

        positionX = dropZone.GetComponent<RectTransform>().rect.height;
        //dropZone.transform.position.x;
        positionY =dropZone.GetComponent<RectTransform>().rect.height;
        //(int)playerArea.GetComponent<RectTransform>().rect.height;

    }
    public void OnHoverEnter()
    {
        zoomCard = Instantiate(gameObject, new Vector2(positionX/2,positionY+sizeCompensation), Quaternion.identity);
        Debug.Log(positionY);
        zoomCard.transform.SetParent(canvas.transform, false);

        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2((float)(sizeOfCardX*1.5), ((float)(sizeOfCardY * 1.5)));

    }
    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
