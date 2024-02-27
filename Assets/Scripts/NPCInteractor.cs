using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;

public class NPCInteractor : MonoBehaviour
{

    public CinemachineVirtualCamera cacamera;
    public DialogueRunner dialogueRunner;   
    
    bool triggerStay = false;

    public PlayerController playerController;
    public float vitesse;
    public float jump;
    public  float vitTemp;
    public float jumpTemp;

    public void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerStay)
        {
            dialogueRunner.StartDialogue("Introduction");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            triggerStay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            triggerStay = false;
        }
    }
    public void ChangerFOVDebutInteraction()
    {
        StartCoroutine(ChangeFOV(cacamera,35,0.5f));
    }

    public void FinirFOVCoroutine()
    {
        StartCoroutine(ChangeFOV(cacamera,60,0.3f));
    }

    [YarnCommand("ChangerFOVMalediction")]
    public void ChangerFOVMalediction()
    {
        StartCoroutine(ChangeFOV(cacamera,110,5));
    }

    [YarnCommand("FaireNoise")]
    public void FaireNoise()
    {
        print("caca");
        StartCoroutine(ChangeNoise(cacamera,0.75f,5));
    }

    [YarnCommand("AmeliorationJoueur")]
    public void AmeliorationJoueur(float vitesse, float jump)
    {
        playerController.speed = vitesse;
        playerController.jumpForce = jump;
    }

    public void ArreterMouvements()
    {
        vitTemp = playerController.speed;
        jumpTemp = playerController.jumpForce;
        playerController.speed = 0f;
        playerController.jumpForce = 0f;
    }

    public void ReprendreMouvements()
    {
        playerController.speed = vitTemp;
        playerController.jumpForce = jumpTemp;
    }

    public IEnumerator ChangeFOV(CinemachineVirtualCamera cam, float endFOV, float duration)
    {
        float startFOV = cam.m_Lens.FieldOfView;
        float time = 0;
        while(time < duration)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }
    public IEnumerator ChangeNoise(CinemachineVirtualCamera cam, float endNoise, float duration)
    {
        
        float startGain = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        float time = 0;
        while(time < duration)
        {
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(startGain, endNoise, time / duration);
            yield return null;
            time += Time.deltaTime;
        }
    }

}
