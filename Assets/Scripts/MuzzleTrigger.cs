using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleTrigger : MonoBehaviour
{
    ParticleSystem m_ParticleSystem;
    Character character;
    void Awake()
    {
        character = GetComponentInParent<Character>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }

    
    void Update()
    {
        if (character.isAttacking)
        {
            m_ParticleSystem.Play();
            character.isAttacking = false;
        }

    }
}
