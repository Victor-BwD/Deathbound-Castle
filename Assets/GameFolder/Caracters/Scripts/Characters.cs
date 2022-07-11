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

    public void PlayerTakaDamage(int damage)
    {
        life -= damage;
        skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
    }
}
