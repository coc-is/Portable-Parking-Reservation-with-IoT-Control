using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class cam : MonoBehaviour
{
    private WebCamTexture devcam;
    private RawImage box;
    public GameObject warn;
    private TextMeshProUGUI warntext;

    private bool nocam = true;
    private bool permission = true;
    public ocr tess;

    private void Start()
    {
        box = gameObject.GetComponent<RawImage>();
        box.uvRect = new Rect(0.48f, 0, 0.52f, 1f);
        warntext = warn.GetComponent<TextMeshProUGUI>();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                permission = false;
                return;
            }




            initbackcam();
        }
        else
        {

            initcam();
        }
    }


    private void Update()
    {

        if (!permission)
        {
            permission = Permission.HasUserAuthorizedPermission(Permission.Camera);
            if (permission)
            {
                initbackcam();
            }
            else
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
        }
        if (permission && nocam)
        {
            warn.SetActive(true);
        }

    }

    private void initbackcam()
    {
        WebCamDevice[] devs = WebCamTexture.devices;

        if (devs.Length == 0)
        {
            warntext.text = "Error! No camera!";
            warn.SetActive(true);
            return;
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (!devs[i].isFrontFacing)
            {
                devcam = new WebCamTexture(devs[i].name);
            }

        }

        if (devcam == null)
        {
            warntext.text = "Error! No back camera!";
            warn.SetActive(true);

            return;
        }

        nocam = false;


        box.texture = devcam;


        devcam.Play();
    }

    private void initcam()
    {
        devcam = new WebCamTexture();

        nocam = false;

        box.texture = devcam;

        devcam.Play();
    }

   public void takepic()
    {
        warn.SetActive(false);
        devcam.Pause();

        Texture2D frame = rotateTexture(GetReadableTexture2d(box.texture),true);

        int boxwidth = frame.width;

        int boxheight = (int)Math.Round(frame.height * (1-0.48f));
        
        //xy origin bottom left


        Texture2D crop = getcrop(frame,0, 0, boxwidth, boxheight);




        Texture2D okukad = getcrop(frame, (int)Math.Round(boxwidth * 0.18292683), 
            (int)Math.Round(boxheight * 0.58490566), 
            (int)Math.Round(boxwidth * 0.48780487805), 
            (int)Math.Round(boxheight * 0.1509434));

        Texture2D ic = getcrop(frame, 0,
            (int)Math.Round(boxheight * 0.41509434),
            (int)Math.Round(boxwidth * 0.365853658),
            (int)Math.Round(boxheight * 0.094339622));

        Texture2D id = getcrop(frame, (int)Math.Round(boxwidth * 0.63414634),
            0,
            (int)Math.Round(boxwidth * 0.365853658),
            (int)Math.Round(boxheight * 0.113207547));


        if (verify(okukad, ic, id))
        {
            devcam.Stop();
            SceneManager.LoadScene("input");
            return;
        }


            warntext.text = "Invalid, make sure that it is an OKU card and the texts are eligible!";
            warn.SetActive(true);

        //NativeGallery.SaveImageToGallery(crop, "new", "photo.png");
        //NativeGallery.SaveImageToGallery(okukad, "new", "photo1.png");
        //NativeGallery.SaveImageToGallery(ic, "new", "photo2.png");
        //NativeGallery.SaveImageToGallery(id, "new", "photo3.png");

        /*
        File.WriteAllBytes("C:\\Users\\Administrator\\Downloads\\photo1.png", crop.EncodeToPNG());

        File.WriteAllBytes("C:\\Users\\Administrator\\Downloads\\photo.png", okukad.EncodeToPNG());

        File.WriteAllBytes("C:\\Users\\Administrator\\Downloads\\photo2.png", ic.EncodeToPNG());

        File.WriteAllBytes("C:\\Users\\Administrator\\Downloads\\photo3.png", id.EncodeToPNG());
        */

        devcam.Play();
    }

    private bool verify(Texture2D okulabel, Texture2D iclabel, Texture2D idlabel)
    {

        String icstr = tess.rettext(iclabel);
        bool icisint=true;
        bool len13 = icstr.Length == 13;

        for(int i = 0; i < icstr.Length-1; i++)
        {
            if (!Char.IsDigit(icstr[i]))
            {
                icisint = false;
            }

        }
        if (icisint && len13)
        {
            retain.icnum = icstr.Substring(0, 12);
        }

        return /*tess.rettext(okulabel).Contains("KAD OKU") &&*/ icisint &&len13;

            
    }

    private static Texture2D GetReadableTexture2d(Texture texture)
    {
        var tmp = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );
        Graphics.Blit(texture, tmp);

        var previousRenderTexture = RenderTexture.active;
        RenderTexture.active = tmp;

        var texture2d = new Texture2D(texture.width, texture.height);
        texture2d.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2d.Apply();

        RenderTexture.active = previousRenderTexture;
        RenderTexture.ReleaseTemporary(tmp);
        return texture2d;
    }

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    Texture2D getcrop(Texture2D orig, int x, int y, int width, int height)
    {
        Texture2D cropped = new Texture2D(width, height);

        cropped.SetPixels(orig.GetPixels(x, y, width, height));

        cropped.Apply();

        return cropped;


    }
}
