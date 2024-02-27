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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerStay)
        {
            dialogueRunner.StartDialogue("Pouffiasse");
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
    public void CommencerCoroutine()
    {
        StartCoroutine(ChangeFOV(cacamera,35,0.5f));
    }

    public void EndCoroutine()
    {
        StartCoroutine(ChangeFOV(cacamera,60,0.3f));
    }
    
    [YarnCommand("FaireNoise")]
    public void FaireNoise()
    {
        print("caca");
        StartCoroutine(ChangeNoise(cacamera,0.65f,5));
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
