using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoop : MonoBehaviour
{
    public ParticleSystem[] WinningParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            for(int i = 0; i < WinningParticle.Length; i++)
            {
                WinningParticle[i].Play();
            }
        }
    }
}
