using UnityEngine;

public class Characters : MonoBehaviour
{
    public int life;
    public Transform skin;
    void Update()
    {
        if(life <= 0)
        {
            skin.GetComponent<Animator>().Play("Die", -1);
        }   
    }
}
