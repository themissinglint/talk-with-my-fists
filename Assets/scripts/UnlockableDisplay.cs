using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockableDisplay : MonoBehaviour {

    public static UnlockableDisplay Instance;
    public GameObject UnlockDisplayTemplate;

    private void Awake() {
        Instance = this;
    }

    public void DoUnlockDisplay(string textToShow) {
        GameObject newDisplay = Instantiate(UnlockDisplayTemplate, transform);
        newDisplay.SetActive(true);
        newDisplay.GetComponentInChildren<TextMeshProUGUI>().text = textToShow;
    }

}
