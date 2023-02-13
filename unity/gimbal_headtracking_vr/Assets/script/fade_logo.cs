using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade_logo : MonoBehaviour
{
    public bool fade_in;
    public bool fade_out;
    public float timer = 3f;
    // Start is called before the first frame update
    void Start()
    {

        //coroutine func init
        StartCoroutine(Timer());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Timer(){
        while(true){
            Debug.Log(System.DateTime.Now);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private StopTimer(){
        StopCoroutine(Timer());
    }
}
