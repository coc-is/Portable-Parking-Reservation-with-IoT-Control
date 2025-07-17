using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    private Button btn;
    public Sprite idle;
    public Sprite down;
    public Sprite active;
    bool trig = false;
    public enum buttonfor{barrier,solar,home,refresh,login }
    public buttonfor funct;
    public static bool retrieving = false;
    public static bool barrieractive = false;
    private GameObject warning;

    private void Start()
    {
        btn = gameObject.GetComponent<Button>();
        if ((int)funct == 4)
        {
            warning = GameObject.Find("/Canvas/warning text");
            warning.SetActive(false);
        }
    }

    private void Update()
    {
        if ((int)funct == 0)
        {
            if (barrieractive)
            {
                btn.image.sprite = active;
                if (bt.receiveddata.Equals("00003"))
                {
                    bt.cleardat();
                    barrieractive = false;
                }
                else if (bt.receiveddata.Equals("00000"))
                {
                    bt.cleardat();
                    bt.WriteData("A00001E");
                }
            }
            else
            {
                btn.image.sprite = idle;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if ((int)funct != 3 && (int)funct != 2)
        {
            btn.image.sprite = down;
        }
        trig = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((int)funct != 3 && (int)funct != 2)
        {
            btn.image.sprite = ((int)funct!=0)?idle:barrieractive?btn.image.sprite:idle;
        }
        if (trig)
        {
            switch ((int)funct)
            {
                case 0:
                    {

                        if (bt.isConnected && !barrieractive)
                        {
                            barrier();
                        }
                        break;
                    }

                case 1:
                    {
                        solar();
                        break;
                    }

                case 2:
                    {
                        home();
                        break;
                    }


                case 3:
                    {
                        if (bt.isConnected && !retrieving)
                        {
                            retrievedat();
                        }
                        break;
                    }

                case 4:
                    {
                        login();
                        break;
                    }

            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        trig = false;

    }

    private void login()
    {
    }

    private void solar()
    {
        if (!barrieractive)
        {
            SceneManager.LoadScene("data");
        }
    }

    private void barrier()
    {
        barrieractive = true;
        bt.WriteData("A00001E");
    }

    private void retrievedat()
    {
        retrieving = true;
        bt.WriteData("A00002E");
        solarmanager.rst();
    }

    private void home()
    {
        if (!retrieving)
        {
            SceneManager.LoadScene("input");
        }
    }
}
