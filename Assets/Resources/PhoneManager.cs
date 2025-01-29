using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public bool isSelfieModeEnabled = false;

    public void OnSelfieModeEnter()
    {
        isSelfieModeEnabled = true;
    }

    public void OnSelfieModeExit()
    {
        isSelfieModeEnabled = false;
    }
}
