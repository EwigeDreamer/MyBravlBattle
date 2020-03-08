using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterTestMotherFucker : MonoBehaviour
{
    public Text text;
    public int fuck = 0;
    private void OnValidate()
    {
        if (text == null) text = GetComponent<Text>();
    }
    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ++fuck;
            if (text) text.text = fuck.ToString();
        }
    }
}
