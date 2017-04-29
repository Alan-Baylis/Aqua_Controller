using UnityEngine;
using System.Collections;

public class GUIStats : MonoBehaviour
{
    static public int guiScale = 1; //TODO
    static public Texture2D StaminaTexture, StaminaTextureFill;
    static public Texture2D HealthTexture, HealthTextureFill;

    private float staminaWidth, staminaHeight, staminaPosX, staminaPosY, healthPosY, healthWidth;
    private static float stamina, staminaFull, staminaGUI, health, healthFull, healthGUI;
    private bool setStamina;
    internal static bool enableGUI;

    //Texture2D pixelsStaminaTextureFill;
    void Start()
    {
        StaminaTexture = GameManager.Get().StaminaTextureBackground;
        StaminaTextureFill = GameManager.Get().StaminaTextureFill;
        staminaWidth = StaminaTexture.width / 2 * guiScale;
        staminaHeight = StaminaTexture.height / 2 * guiScale;
        staminaPosX = StaminaTexture.width /2 + StaminaTextureFill.width / 100 * guiScale;
        staminaPosY = StaminaTexture.height/ 10 * guiScale;

        HealthTexture = GameManager.Get().HealthTextureBackground;
        HealthTextureFill = GameManager.Get().HealthTextureFill;
        healthWidth = staminaWidth;
        healthPosY = StaminaTexture.height * 0.64f * guiScale;

    }

    void OnGUI()
    {

        staminaFull = GameManager.Get().maxStamina;
        stamina = GameManager.Get().stamina;
        staminaGUI = stamina * staminaWidth / staminaFull;

        healthFull = GameManager.Get().maxHealth;
        health = GameManager.Get().health;
        healthGUI = health * healthWidth / healthFull;;

        if (enableGUI)
        {
            //Stamina GUI
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaGUI, staminaHeight), StaminaTextureFill);
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaWidth, staminaHeight), StaminaTexture);
            //Health GUI
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, healthGUI, staminaHeight), HealthTextureFill);
            GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, staminaWidth, staminaHeight), HealthTexture);

            //Display framerate
            GUI.Label(new Rect(0, 0, 100, 100), (1.0f / Time.smoothDeltaTime).ToString());
        }
    }
}
