using System.Collections;
using System.Collections.Generic;
using UnityEngine;




class Program
{

    private State state;
    static void Main(string[] args)
    {
        //Create a Product with Out Of Stock Status
        Subject RedMI = new Subject(false, "Snake");
        //User Agente1 will be created and user1 object will be registered to the subject
        Observer user1 = new Observer("Agente1", RedMI);
        //User Agente2 will be created and user1 object will be registered to the subject
        Observer user2 = new Observer("Agente2", RedMI);
        

        Debug.Log("Red MI Mobile current state : " + RedMI.getEncontrado());

        // Encontrado
   
        RedMI.setEncontrado(true);
        
    }
}



