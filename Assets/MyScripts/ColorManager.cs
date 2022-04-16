using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Texture[] textures; 
    //[SerializeField] Color[] colors;

    public void changeColor(int color)
    {
        for (int i = 0; i < textures.Length; i++)
        {
            if (color == i)
            {
                material.mainTexture = textures[i];
            }
            else
            {
                continue;
            }
        }
    }
}
