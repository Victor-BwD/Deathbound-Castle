using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caracters : MonoBehaviour
{
    public int life;
    public Transform skin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            skin.GetComponent<Animator>().Play("Die", -1);
        }   
    }
}
