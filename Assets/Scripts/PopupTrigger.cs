using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTrigger : MonoBehaviour
{
    public Image popupImage;
    public List<TMP_Text> text;
    Color textColour;

    bool popupFinished = true;

    void Start()
    {
        foreach (TMP_Text t in text)
        {
            textColour = t.color;
            popupImage.color = t.color = Color.clear;
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && popupFinished)
        StartCoroutine("ShowPopup");
    }

    IEnumerator ShowPopup()
    {
        popupImage.transform.SetAsLastSibling();
        
        popupFinished = false;
        for (int i = 0; i < 100; i++)
        {
            popupImage.color = Color.Lerp(popupImage.color, Color.white, (i / 1) * Time.deltaTime);

            foreach (TMP_Text t in text)
            t.color = Color.Lerp(t.color, textColour, (i / 1) * Time.deltaTime);
            
            yield return new WaitForSeconds(0.000001f);
        }

        yield return new WaitForSeconds(5);

        for (int i = 0; i < 100; i++)
        {
            popupImage.color = Color.Lerp(popupImage.color, Color.clear, (i / 1) * Time.deltaTime);

            foreach (TMP_Text t in text)
            t.color = Color.Lerp(t.color, Color.clear, (i / 1) * Time.deltaTime);
            
            yield return new WaitForSeconds(0.000001f);
        }

        popupFinished = true;
    }
}
