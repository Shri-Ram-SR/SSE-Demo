using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Data.Common;
using Unity.VisualScripting;

public class Mediator : MonoBehaviour
{
    ushort usAddress;
    //Private vars
    //UModbusTCP
    UModbusTCP m_oUModbusTCP;
    UModbusTCP.ResponseData m_oUModbusTCPResponse;
    UModbusTCP.ExceptionData m_oUModbusTCPException;

    public string Clg_IP_Address;
    public string SSE_IP_Address;
    public bool SSE;
    public ushort Port;
    bool m_bUpdateResponse;
    int m_iResponseValue;

    public static Mediator Instance;
    void Awake()
    {
        Instance = this;
        //UModbusTCP
        m_oUModbusTCP = null;
        m_oUModbusTCPResponse = null;
        m_oUModbusTCPException = null;
        m_bUpdateResponse = false;
        m_iResponseValue = -1;
        m_oUModbusTCP = UModbusTCP.Instance;
    }
    void Update()
    {
        if (m_bUpdateResponse)
        {
            m_bUpdateResponse = false;
            m_oUModbusTCP.Dispose();
        }

    }
    public void ButtonReadCoils()
    {
        //Reset response
        m_bUpdateResponse = false;
        m_iResponseValue = -1;

        usAddress = 1;

        if (!m_oUModbusTCP.connected)
        {
            m_oUModbusTCP.Connect(SSE ? SSE_IP_Address : Clg_IP_Address, Port);
            IOManager.Instance.Connect();
        }

        if (m_oUModbusTCPResponse != null)
        {
            m_oUModbusTCP.OnResponseData -= m_oUModbusTCPResponse;
        }
        m_oUModbusTCPResponse = new UModbusTCP.ResponseData(UModbusTCPOnResponseData);
        m_oUModbusTCP.OnResponseData += m_oUModbusTCPResponse;

        //Exception callback
        if (m_oUModbusTCPException != null)
        {
            Debug.Log("Expection " + m_oUModbusTCPException);
            m_oUModbusTCP.OnException -= m_oUModbusTCPException;
        }
        m_oUModbusTCPException = new UModbusTCP.ExceptionData(UModbusTCPOnException);
        m_oUModbusTCP.OnException += m_oUModbusTCPException;

        //Read coils
        m_oUModbusTCP.ReadCoils(1, 1, usAddress, 1);

    }
    void UModbusTCPOnResponseData(ushort _oID, byte _oUnit, byte _oFunction, byte[] _oValues)
    {
        //Number of values
        int iNumberOfValues = _oValues[8];
        if (iNumberOfValues == 0)
        {
            IOManager.Instance.SetOutPut(false,0);
            return;
        }
        IOManager.Instance.SetOutPut(true,iNumberOfValues);

        //Get values pair with 2
        int oCounter = 0;
        for(int i = 0; i < iNumberOfValues; i += 2) {
            byte[] oResponseFinalValues = new byte[2];
            for(int j = 0; j < 2; ++j) {
                oResponseFinalValues[j] = _oValues[9 + i + j];
                Debug.Log(oResponseFinalValues[j]);
            }
            ++oCounter; //More address
        }

        //Get values
        if (usAddress == 1)
        {
            byte[] oResponseFinalValues = new byte[iNumberOfValues];
            for (int i = 0; i < iNumberOfValues; ++i)
            {
                oResponseFinalValues[i] = _oValues[9 + i];
            }

            int[] iValues = UModbusTCPHelpers.GetIntsOfBytes(oResponseFinalValues);
            m_iResponseValue = iValues[0];
            m_bUpdateResponse = true;
        }
        if (usAddress == 2)
        {
            byte[] oResponseFinalValues = new byte[iNumberOfValues];
            for (int i = 0; i < iNumberOfValues; ++i)
            {
                oResponseFinalValues[i] = _oValues[9 + i];
            }

            int[] iValues = UModbusTCPHelpers.GetIntsOfBytes(oResponseFinalValues);
            m_bUpdateResponse = true;
        }
        if (usAddress == 3)
        {
            byte[] oResponseFinalValues = new byte[iNumberOfValues];
            for (int i = 0; i < iNumberOfValues; ++i)
            {
                oResponseFinalValues[i] = _oValues[9 + i];
            }

            int[] iValues = UModbusTCPHelpers.GetIntsOfBytes(oResponseFinalValues);
            m_bUpdateResponse = true;
        }
    }

    void UModbusTCPOnException(ushort _oID, byte _oUnit, byte _oFunction, byte _oException)
    {
        Debug.Log("Exception: " + _oException);
    }

   
}