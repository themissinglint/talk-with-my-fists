using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour {

    public static PlayerInstance Instance;

    private void Awake() {
        Instance = this;
    }

}
