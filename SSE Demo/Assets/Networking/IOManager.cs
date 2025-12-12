using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IOManager : MonoBehaviour
{
    public static IOManager Instance;
    int x = 0;
    public Image Image;
    public TextMeshProUGUI Txt_Log;

    public bool Activate;
    private void Awake()
    {
        Instance = this;
    }
    public void ForceActivate(bool b)
    {
        if(b)
            SetOutPut(b, 10);
    }
    public void SetOutPut(bool Got,int i)
    {
        x = i;
        Activate = Got;
        Debug.Log(Got);
        Invoke("Testing",0);
    }
    public void NotConnected(string s)
    {
        Txt_Log.text = s;
    }
    public void Testing()
    {
        if (Activate)
        {
            Image.color = Color.green;
            Txt_Log.text += "Got response " + x.ToString();
        }
        else
        {
            Image.color = Color.red;
            Txt_Log.text += "Got no response " + x.ToString();
        }
        Activate = false;
    }
    public void Connect()
    {
        Txt_Log.text = "Connected  ";
    }
    public void ChangeSSE(bool b)
    {
        Mediator.Instance.SSE = b;
    }
}
