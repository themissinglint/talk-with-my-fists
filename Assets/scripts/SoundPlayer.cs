using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public AudioSource Source;

    public void PlaySound() {
        Source.Play();
    }
    
}
