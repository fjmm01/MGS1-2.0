using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamera : MonoBehaviour
{
    public CinemachineVirtualCamera camCen;
    public CinemachineVirtualCamera PovCam;
    public CinemachineVirtualCamera cornerCam;
    private PlayerInput playerInput;
    void Awake()
    {
        camCen.enabled = true;
        PovCam.enabled = false;
        cornerCam.enabled = false;
        playerInput = GetComponent<PlayerInput>();
    }

    
    void Update()
    {
        CameraSwitcher();
    }



    private void CameraSwitcher()
    {
        if(playerInput.isOnLeftEdge == true || playerInput.isOnRightEdge == true)
        {
            camCen.Priority = 0;
            cornerCam.Priority = 10;
            
           
        }
        else
        {
            cornerCam.Priority = 0;
            camCen.Priority = 10;
            
        }

        if(playerInput.canPassUnder == true && (playerInput.obstacle1.enabled == false || playerInput.obstacle2.enabled == false))
        {
            camCen.enabled = false;
            PovCam.enabled = true;
        }
        else 
        {
            PovCam.enabled = false;
            camCen.enabled = true;
        }
    }
}
