using FileExportScheduler.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler.Controller
{
    class ConnectToCOM
    {
        private void GetConnectCOM(Dictionary<string, DeviceModel> deviceDic, SerialPort serialPort)
        {
            foreach (KeyValuePair<string, DeviceModel> deviceUnit in deviceDic)
            {
                if (deviceUnit.Value.Protocol == "Serial Port")
                {
                    /*serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;*/
                    serialPort = new SerialPort(((ComConfigModel)deviceUnit.Value).Com, ((ComConfigModel)deviceUnit.Value).Baud, ((ComConfigModel)deviceUnit.Value).parity, ((ComConfigModel)deviceUnit.Value).Databit, ((ComConfigModel)deviceUnit.Value).stopBits);
                    serialPort.Open();

                }
            }
        }
        
    }
}
