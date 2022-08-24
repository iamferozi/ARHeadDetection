using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Text scaleText;
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
        scaleText.text = headtransform.TrackablesParent.localScale.ToString();
        //currentx = head.lossyScale.x;
        //aRFaceManager.ontr += changesize();
        //aRFaceManager.facesChanged += ARFaceManager_facesChanged;
    }

    //private void ARFaceManager_facesChanged(ARFacesChangedEventArgs obj)
    //{
    //    aRFaceManager.facePrefab.transform.localScale = new Vector3(x, x, x);
    //}

    [SerializeField] XROrigin headtransform;
    //[SerializeField] ARFace s;
    //[SerializeField] ARFaceManager aRFaceManager;
    [SerializeField] float min, max/*,x,perx,valuex, currentx*/;
    [SerializeField] Transform head;
    public void scaleSizeX(Slider slider)
    {
#if !UNITY_EDITOR
        float f = Mathf.Lerp(min, max, slider.value);
        //head.localScale = new Vector3(f, f, f);
        for (int i = 0; i < headtransform.TrackablesParent.childCount; i++)
        {
            headtransform.TrackablesParent.GetChild(i).localScale = new Vector3(f, headtransform.TrackablesParent.GetChild(i).localScale.y, headtransform.TrackablesParent.GetChild(i).localScale.z);
        }
        scaleText.text = headtransform.TrackablesParent.GetChild(0).transform.localScale.ToString();

#endif
#if UNITY_EDITOR
        //float d = slider.value * (currentx - min);
        //currentx = min;
        //perx = currentx - min;
        //valuex = perx * max;
        float f = Mathf.Lerp(min, max, slider.value);
        head.localScale = new Vector3(f, head.localScale.y, head.localScale.z);
#endif
        //x = (slider.value + max) * 1;
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(x, x, x);
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(valuex, valuex, valuex);
        scaleText.text = headtransform.TrackablesParent.localScale.ToString();

        //ARFace ar = headtransform.TrackablesParent(s.trackableId);
        //if(ar!=null)
    }
    public void scaleSizeY(Slider slider)
    {
#if !UNITY_EDITOR
        float f = Mathf.Lerp(min, max, slider.value);
        //head.localScale = new Vector3(f, f, f);
        for (int i = 0; i < headtransform.TrackablesParent.childCount; i++)
        {
            headtransform.TrackablesParent.GetChild(i).localScale = new Vector3(headtransform.TrackablesParent.GetChild(i).localScale.x, f, headtransform.TrackablesParent.GetChild(i).localScale.z);
        }
        scaleText.text = headtransform.TrackablesParent.GetChild(0).transform.localScale.ToString();
#endif
#if UNITY_EDITOR
        //float d = slider.value * (currentx - min);
        //currentx = min;
        //perx = currentx - min;
        //valuex = perx * max;
        float f = Mathf.Lerp(min, max, slider.value);
        head.localScale = new Vector3(head.localScale.x, f, head.localScale.z);
#endif
        //x = (slider.value + max) * 1;
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(x, x, x);
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(valuex, valuex, valuex);
        //ARFace ar = headtransform.TrackablesParent(s.trackableId);
        //if(ar!=null)
    }
    public void scaleSizeZ(Slider slider)
    {
#if !UNITY_EDITOR
        float f = Mathf.Lerp(min, max, slider.value);
        //head.localScale = new Vector3(f, f, f);
        for (int i = 0; i < headtransform.TrackablesParent.childCount; i++)
        {
            headtransform.TrackablesParent.GetChild(i).localScale = new Vector3(headtransform.TrackablesParent.GetChild(i).localScale.x, headtransform.TrackablesParent.GetChild(i).localScale.y, f);
        }
        scaleText.text = headtransform.TrackablesParent.GetChild(0).transform.localScale.ToString();

#endif
#if UNITY_EDITOR
        //float d = slider.value * (currentx - min);
        //currentx = min;
        //perx = currentx - min;
        //valuex = perx * max;
        float f = Mathf.Lerp(min, max, slider.value);
        head.localScale = new Vector3(head.localScale.x, head.localScale.y, f);
#endif
        //x = (slider.value + max) * 1;
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(x, x, x);
        //aRFaceManager.facePrefab.transform.localScale = new Vector3(valuex, valuex, valuex);

        //ARFace ar = headtransform.TrackablesParent(s.trackableId);
        //if(ar!=null)
    }

    //public void changesize()
    //{
    //    aRFaceManager.facePrefab.transform.localScale = new Vector3(x, x, x);
    //}

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
