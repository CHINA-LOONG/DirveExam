using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLightController : MonoBehaviour {

    public Light light;

    public void SetLight(bool show){
        light.enabled = show;
    }
}
