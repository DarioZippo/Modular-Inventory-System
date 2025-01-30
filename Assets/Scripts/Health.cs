using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 50; 

    private void Start(){
        Messenger<int, int>.Broadcast(MessengerGameEvent.HEALTH_UPDATE, maxHealth, currentHealth);
    }

    public void AddHealth(int value){
        currentHealth += value;
        if(currentHealth >= maxHealth){
            currentHealth = maxHealth;
        }

        Messenger<int, int>.Broadcast(MessengerGameEvent.HEALTH_UPDATE, maxHealth, currentHealth, MessengerMode.REQUIRE_LISTENER);
    }

    public void LoseHealth(int value){
        currentHealth -= value;
        if(currentHealth <= 0){
            currentHealth = 0;
        }

        Messenger<int, int>.Broadcast(MessengerGameEvent.HEALTH_UPDATE, maxHealth, currentHealth, MessengerMode.REQUIRE_LISTENER);
    }
}
