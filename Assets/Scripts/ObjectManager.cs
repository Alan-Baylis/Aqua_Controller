﻿using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour
{
    public Texture2D StaminaTextureBackground, StaminaTextureFill;
    public float stamina, maxStamina;



    // singleton
    private static ObjectManager m_Instance = null;
    public static ObjectManager Get()
    {

        if (m_Instance == null)
            m_Instance = (ObjectManager)FindObjectOfType(typeof(ObjectManager));
        return m_Instance;
    }


    // class 
    public GUIStats guiStatsObject;

    void Awake()
    {

    }
}