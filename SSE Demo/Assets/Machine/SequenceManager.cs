using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.IO;

public class SequenceManager : MonoBehaviour
{
    public List<AnimationManager> Managers;
    public bool Finished;
    bool Completed;
    int Ind = 0;
    void Start()
    {
        //TcpListener listen = new TcpListener(IPAddress.Any, 1234);
        //modbus poll
        Managers[Ind].StartAnimation();
    }
    private void Update()
    {
        if(Completed)
            return;
        if (Finished)
        {
            Finished = false;
            Managers[Ind].EndAnimation();
            Ind++;
            if(Ind == Managers.Count)
            {
                Completed = true;
                return;
            }
            Managers[Ind].StartAnimation();
        }
    }
}
