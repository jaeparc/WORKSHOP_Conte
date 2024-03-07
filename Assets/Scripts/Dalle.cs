using UnityEngine;

public class Dalle : MonoBehaviour
{
    private AudioSource audioSourceEnter;
    private AudioSource audioSourceExit;

    // Start is called before the first frame update
    void Start()
    {
        Transform enterSoundTransform = transform.Find("Pressure SFX Entrée");
        if (enterSoundTransform != null)
        {
            audioSourceEnter = enterSoundTransform.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("EnterSound AudioSource not found on the GameObject");
        }

        Transform exitSoundTransform = transform.Find("Pressure SFX Sortie");
        if (exitSoundTransform != null)
        {
            audioSourceExit = exitSoundTransform.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("ExitSound AudioSource not found on the GameObject");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioSourceEnter != null)
        {
            audioSourceEnter.Play();
            transform.position -= new Vector3(0,0.1f,0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioSourceExit != null)
        {
            audioSourceExit.Play();
            transform.position += new Vector3(0,0.1f,0);
        }
    }
}
