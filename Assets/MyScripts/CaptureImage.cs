using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using NativeGalleryNamespace;


public class CaptureImage : MonoBehaviour
{
    [SerializeField] AudioSource captureSound;
    //Set your screenshot resolutions
    public int captureWidth = 1920;
    public int captureHeight = 1080;
    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.JPG;
    // folder to write output (defaults to data path)
    private string outputFolder;
    // private variables needed for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    //[SerializeField] Text screenShotpath;
    //[SerializeField] Image pictureInScene;
    //Initialize Directory
    private void Start()
    {
        captureWidth = Screen.width;
        captureHeight = Screen.height;

        outputFolder = Application.persistentDataPath + "/Screenshots/";
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        Debug.Log("Save Path will be : " + outputFolder);
        NativeGallery.RequestPermission(NativeGallery.PermissionType.Write);

    }

    private string CreateFileName(int width, int height)
    {
        //timestamp to append to the screenshot filename
        string timestamp = DateTime.Now.ToString("yyyyMMddTHHmmss");
        // use width, height, and timestamp for unique file 
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", outputFolder, width, height, timestamp, format.ToString().ToLower());
        // return filename
        return filename;
    }
    bool isProcessing;
    private void CaptureScreenshot()
    {
        captureSound.Play();
        isProcessing = true;
        // create screenshot objects
        if (renderTexture == null)
        {
            // creates off-screen render texture to be rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }
        // get main camera and render its output into the off-screen render texture created above
        Camera camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();
        // mark the render texture as active and read the current pixel data into the Texture2D
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        // reset the textures and remove the render texture from the Camera since were done reading the screen data
        camera.targetTexture = null;
        RenderTexture.active = null;
        // get our filename
        string filename = CreateFileName((int)rect.width, (int)rect.height);
        // get file header/data bytes for the specified image format
        byte[] fileHeader = null;
        byte[] fileData = null;
        //pictureInScene.sprite = Sprite.Create(screenShot, new Rect(0, 0, 100, 100), new Vector2(.5f,.5f));
        //screenShot.Apply();
        //Set the format and encode based on it
        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
        {
            fileData = screenShot.EncodeToJPG();
        }
        else //For ppm files
        {
            // create a file header - ppm files
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }
        // create new thread to offload the saving from the main thread
        new System.Threading.Thread(() =>
        {
            var file = System.IO.File.Create(filename);
            if (fileHeader != null)
            {
                file.Write(fileHeader, 0, fileHeader.Length);
            }
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            Debug.Log(string.Format("Screenshot Saved {0}, size {1}", filename, fileData.Length));
            //if (screenShotpath != null)
            //{
            //    screenShotpath.text = string.Format("Screenshot Saved {0}, size {1}", filename, fileData.Length);
            //}
            isProcessing = false;
        }).Start();

        //NativeToolkit.SaveImage(screenShot, "GalleryTest", "png");
        //SaveImageToGallery(screenShot, "GalleryTest", "THIS IS A TEST IMAGE");
        NativeGallery.SaveImageToGallery(screenShot, "HeadFilter", filename, (success, path) => Debug.Log("Media save result: " + success + " " + path));
        
        Destroy(renderTexture);
        renderTexture = null;
        screenShot = null;

        //StartCoroutine(capturePhoto());
    }

    IEnumerator capturePhoto()
    {
        yield return new WaitForEndOfFrame();
        Rect regiontoread = new Rect(0, 0, Screen.width, Screen.height);
        screenShot.ReadPixels(regiontoread, 0, 0, false);
        screenShot.Apply();
        showphoto();
    }

    private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
    public static string SaveImageToGallery(Texture2D texture2D, string title, string description)
    {
        using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
        {
            using (var cr = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                var image = Texture2DToAndroidBitmap(texture2D);
                var imageUrl = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                return imageUrl;
            }
        }
    }

    public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
    {
        byte[] encoded = texture2D.EncodeToPNG();
        using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
        }
    }
    private static AndroidJavaObject _activity;
    public static AndroidJavaObject Activity
    {
        get
        {
            if (_activity == null)
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _activity;
        }
    }

    void showphoto()
    {
        Sprite photoToShow = Sprite.Create(screenShot,new Rect(0,0,screenShot.width,screenShot.height),new Vector2(.5f,.5f),100);
        //pictureInScene.sprite = photoToShow;
    }

    protected internal static Texture2D LoadTextrure(string fileName)
    {
        return LoadTextrure(string.Empty, fileName);
    }
    protected internal static Texture2D LoadTextrure(string path, string fileName)
    {
        Texture2D tex = new Texture2D(0, 0);
        //texTexture2D finalTex;

        string dataPath = path + fileName;
        byte[] imageData;
        try
        {
            if (System.IO.File.Exists(dataPath))
            {
                imageData = System.IO.File.ReadAllBytes(dataPath);
                tex.LoadImage(imageData);
                tex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
                tex.LoadImage(imageData);
            }
            else
            {
                tex = null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            tex = null;
        }
        return tex;
    }

    public void TakeScreenShot()
    {
        if (!isProcessing)
        {
            CaptureScreenshot();
        }
        else
        {
            Debug.Log("Currently Processing");
        }
    }

}
