using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private static int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0){
            playerDie();
        }
    }

    public void takeDamage(){
        health--;
        Debug.Log("Current Health: " + health);
    }

    void playerDie(){

    }
}
