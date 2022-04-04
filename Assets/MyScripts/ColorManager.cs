using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Color[] colors;

    public void changeColor(int color)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (color == i)
            {
                material.color = colors[i];
            }
            else
            {
                continue;
            }
        }
    }
}
