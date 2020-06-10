using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject player;
    public GameObject cursor;
    public RectTransform chargeRect; // This is the Rect Tranform of the charge meter
    public RectTransform hpRect; // This is the Rect Tranform of the HP meter
    public Text livesText;
    //Due to my inability to work with children i have taken the easy route to just manualy select them


    private GunControll playerguncontroll;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            GameObject[] plist = GameObject.FindGameObjectsWithTag("Player");
            player = plist[0];

        }
        playerguncontroll = player.GetComponent<GunControll>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponCharge();
        UpdatePlayerHPandLives();
        UpdateCursor();
    }


    void UpdateCursor()
    {
        // This changes the cursor color depending if the target can be grappled
        if (playerguncontroll.CheckIfCanGrapple())
            cursor.GetComponent<Image>().color = new Color32(0, 255, 0, 100);
        else
            cursor.GetComponent<Image>().color = new Color32(255, 255, 225, 50);
    }
    void UpdateWeaponCharge()
    {
        float chargepercentage = playerguncontroll.GetChargePercentage() * 100;
        float num = chargeRect.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        chargeRect.transform.localPosition = new Vector3(0, -num + (chargepercentage * (num / 100)), 0);
        chargeRect.sizeDelta = new Vector2(20f, chargepercentage);
    }

    void UpdatePlayerHPandLives()
    {
        float chargepercentage = player.GetComponent<Health>().healthPoints / player.GetComponent<Health>().respawnHealthPoints * 100;
        float num = hpRect.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;

        hpRect.transform.localPosition = new Vector3(0, num - (chargepercentage * (num / 100)), 0);
        hpRect.sizeDelta = new Vector2(20f, chargepercentage * 2);


        livesText.text = player.GetComponent<Health>().numberOfLives.ToString();
    }
}
