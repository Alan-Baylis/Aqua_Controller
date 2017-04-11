using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Texture2D StaminaTextureBackground, StaminaTextureFill;
    public Texture2D HealthTextureBackground, HealthTextureFill;
    public float stamina, maxStamina, health, maxHealth;



    // singleton
    private static GameManager m_Instance = null;
    public static GameManager Get()
    {

        if (m_Instance == null)
            m_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
        return m_Instance;
    }









    // class 
    public GUIStats guiStatsObject;

    void Awake()
    {

    }
}