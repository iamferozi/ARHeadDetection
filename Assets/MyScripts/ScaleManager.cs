using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;
public class ScaleManager : MonoBehaviour
{

    [SerializeField] GameObject[] Meshes;
    [SerializeField] GameObject[] XRObjects;


    public enum sizes
    {
        small = 0,
        medium = 1,
        large = 2,
        normal =3
    }
    [SerializeField] ARFaceManager aRFaceManager;
    private void Awake()
    {
        aRFaceManager = XRObjects[0].GetComponent<ARFaceManager>();

        for (int i = 0; i < Meshes.Length; i++)
        {
            if(i == (int)defaultHeadSize)
            {
                aRFaceManager.facePrefab = Meshes[i];
                XRObjects[0].SetActive(true);
                Meshes[i].SetActive(true);
            }
            else
            {
                Meshes[i].SetActive(false);
            }
        }
    }


    public sizes defaultHeadSize;

    public void changeHead(int size)
    {
        for (int i = 0; i < Meshes.Length; i++)
        {
            if(i == size)
            {
                //ARSession ars = XRObjects[0].GetComponent<ARSession>();
                //ars.Reset();
                aRFaceManager.facePrefab = Meshes[i];
                Meshes[size].SetActive(true);
                //XRObjects[i].SetActive(true);
            }
            else
            {
                Meshes[i].SetActive(false);
                //XRObjects[i].SetActive(false);
            }
        }
    }

}
