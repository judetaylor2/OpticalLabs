using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientSound : MonoBehaviour
{
    public AudioSource[] audioSources;
    int audioIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 11")
        {
            audioIndex=1;

            foreach (AudioSource a in audioSources)
            {
                if (a != audioSources[audioIndex])
                a.Stop();
            }
        }
        
        if (!audioSources[audioIndex].isPlaying)
        audioSources[audioIndex].Play();
        
    }
}
