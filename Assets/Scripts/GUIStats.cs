using UnityEngine;
using System.Collections;

public class GUIStats : MonoBehaviour
{
    static public int guiScale = 1; //TODO
    static public Texture2D StaminaTexture, StaminaTextureFill;
    static public Texture2D HealthTexture, HealthTextureFill;

    private float staminaWidth, staminaHeight, staminaPosX, staminaPosY, healthPosY;
    private static float stamina, staminaFull, staminaGUI;
    private bool setStamina;
    //Texture2D pixelsStaminaTextureFill;
    void Start()
    {
        StaminaTexture = ObjectManager.Get().StaminaTextureBackground;
        StaminaTextureFill = ObjectManager.Get().StaminaTextureFill;
        staminaWidth = StaminaTexture.width / 2 * guiScale;
        staminaHeight = StaminaTexture.height / 2 * guiScale;
        staminaPosX = StaminaTexture.width /2 + StaminaTextureFill.width / 100 * guiScale;
        staminaPosY = StaminaTexture.height/ 10 * guiScale;

        HealthTexture = ObjectManager.Get().HealthTextureBackground;
        HealthTextureFill = ObjectManager.Get().HealthTextureFill;
        healthPosY = StaminaTexture.height * 0.64f * guiScale;

        //Color[] fillPixels = StaminaTextureFill.GetPixels(0, 0, staminaWidth, staminaHeight);
        //pixelsStaminaTextureFill = new Texture2D(staminaWidth, staminaHeight);
        //pixelsStaminaTextureFill.SetPixels(fillPixels);
        //pixelsStaminaTextureFill.Apply();

    }

    void OnGUI()
    {
        staminaFull = ObjectManager.Get().maxStamina;
        stamina = ObjectManager.Get().stamina;

        staminaGUI = stamina * staminaWidth / staminaFull;

        //else staminaGUI = 1;

        //print(staminaGUI);

        GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaGUI, staminaHeight), StaminaTextureFill);
        GUI.DrawTexture(new Rect(Screen.width - staminaPosX, staminaPosY, staminaWidth , staminaHeight), StaminaTexture);

        GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, staminaGUI, staminaHeight), HealthTextureFill);
        GUI.DrawTexture(new Rect(Screen.width - staminaPosX, healthPosY, staminaWidth, staminaHeight), HealthTexture);

    }

}
