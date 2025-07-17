using System;
using UnityEngine;
using UnityEngine.UI;

public class ocr : MonoBehaviour
{
    private TesseractDriver _tesseractDriver;
    private string txt = "";
    private Texture2D _texture;

    private void Start()
    {
        

        _tesseractDriver = new TesseractDriver();

        _tesseractDriver.Setup(skip);
        bt.init();
        for (int i = 0; i < 144; i++)
        {
            retain.solardat[i] = new Vector2(i * 7, 0);
        }
    }

    private void skip()
    {

    }
    public String rettext(Texture2D img)
    {
        Texture2D texture = new Texture2D(img.width, img.height, TextureFormat.ARGB32, false);

        texture.SetPixels32(img.GetPixels32());
        texture.Apply();


        thresholdfilter(texture, 0.2f, false);

        _texture = texture;
        reg();

        return txt;

    }
    private void thresholdfilter(Texture2D img, float thres, bool rev)
    {
        Color32[] pixels = img.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            float scale = ((Color)pixels[i]).grayscale;
            int color = (scale > thres) ? rev ? 0 : 255 : rev ? 255 : 0;
            pixels[i][0] = (byte)color;
            pixels[i][1] = (byte)color;
            pixels[i][2] = (byte)color;
        }

        img.SetPixels32(pixels);
        img.Apply();

    }


    private void reg()
    {
        txt= _tesseractDriver.Recognize(_texture) + _tesseractDriver.GetErrorMessage();


            }

  
}