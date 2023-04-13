using UnityEngine;

public class Characters : MonoBehaviour
{
    public Transform skin;
    public int life;
    
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
