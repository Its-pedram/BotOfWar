using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpToSciFi : MonoBehaviour
{
    void OnTriggerEnter()
    {
        SceneManager.LoadScene("Test_Map");
    }
}
