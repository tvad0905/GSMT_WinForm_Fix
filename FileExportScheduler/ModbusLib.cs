using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExportScheduler
{
    class ModbusLib : SerialPort
    {
        public event MessageRecivedEventHandler MessageRecived;

        public delegate void MessageRecivedEventHandler(byte[] b);

        private string _modbusStatus;

        public string modbusStatus
        {
            get
            {
                return _modbusStatus;
            }
            set
            {
                _modbusStatus = value;
            }
        }

        public bool Connect(string portName, int baudRate, int databits, Parity prty, StopBits stpBts)
        {

            // Ensure port isn't already opened:
            if ((!this.IsOpen))
            {
                // Assign desired settings to the serial port:
                this.PortName = portName;
                this.BaudRate = baudRate;
                this.DataBits = databits;
                this.Parity = System.IO.Ports.Parity.Space;
                this.StopBits = stpBts;
                // These timeouts are default and cannot be editted through the class at this point:
                this.ReadTimeout = 1000;
                this.WriteTimeout = 1000;

                try
                {
                    this.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }
        }

        public new bool Close()
        {
            // Ensure port is opened before attempting to close:
            if ((IsOpen))
            {
                try
                {
                    base.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + this.PortName + ": " + err.Message;
                    return false;
                }

                modbusStatus = this.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = this.PortName + " is not open";
                return false;
            }
        }

        private int GetCRC(byte[] message)
        {
            // Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            // return the CRC values:

            // I copy the function from the Arduino Sketch. Function Encode =Function Decode 
            UInt16 CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF;
            byte CRCLow = 0xFF;
            byte CRCLSB;
            int i, j;

            for (i = 0; i <= Information.UBound(message) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (j = 0; j <= 7; j++)
                {
                    CRCLSB = (byte)(CRCFull & 0x1);
                    CRCFull = (ushort)(CRCFull >> 1);

                    if ((CRCLSB == 1))
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRCHigh = (byte)(CRCFull >> 8);
            CRCFull = (ushort)(CRCFull << 8 | CRCHigh);
            CRCFull = (ushort)(CRCFull & 0xFFFF);

            return (CRCFull);
        }

        private void BuildMessage(byte address, byte type, UInt16 start, UInt16 registers, byte[] message)
        {
            // Array to receive CRC bytes:
            int crc;
            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)(start & 0xFFFF);
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)(registers & 0xFFFF);
            crc = GetCRC(message);
            message[message.Length - 2] = (byte)(crc >> 8);
            message[message.Length - 1] = (byte)(crc & 0xFF);
        }

        private bool CheckResponse(byte[] response)
        {
            // Perform a basic CRC check:
            int CRC;
            CRC = GetCRC(response);
            if ((CRC >> 8 == response[response.Length - 2] & (CRC & 0xFF) == response[response.Length - 1]))
                return true;
            else
                return false;
        }

        public bool SendFc1(byte address, UInt16 start, UInt16 registers)
        {
            // Function 1
            // Ensure port is open:
            if ((this.IsOpen))
            {
                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Function 1 request is always 8 bytes:
                byte[] message = new byte[8];
                // Function 1 response buffer:
                int nW = registers / 8;
                if ((registers % 8) > 0)
                    nW = nW + 1;
                byte[] response = new byte[5 + nW + 1];

                // Build outgoing modbus message:
                BuildMessage(address, 1, start, registers, message);
                // Send modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);
                }

                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool SendFc2(byte address, UInt16 start, UInt16 registers)
        {
            // Function 2
            // Ensure port is open:
            if ((this.IsOpen))
            {
                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Function 2 request is always 8 bytes:
                byte[] message = new byte[8];
                // Function 2 response buffer:
                int nW = registers / 8;
                if ((registers % 8) > 0)
                    nW = nW + 1;
                byte[] response = new byte[5 + nW + 1];

                // Build outgoing modbus message:
                BuildMessage(address, 2, start, registers, message);
                // Send modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool SendFc3(byte address, UInt16 start, UInt16 registers)
        {
            // Function 3
            // Ensure port is open:
            if ((this.IsOpen))
            {

                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Function 3 request is always 8 bytes:
                byte[] message = new byte[8];
                // Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers + 1];



                // Build outgoing modbus message:
                BuildMessage(address, 3, start, registers, message);
                // Send modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);
                }

                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
            }

            return true;
        }

        public bool SendFc4(byte address, UInt16 start, UInt16 registers)
        {
            // Function 4
            // Ensure port is open:
            if ((this.IsOpen))
            {
                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Function 4 request is always 8 bytes:
                byte[] message = new byte[8];
                // Function 4 response buffer:
                byte[] response = new byte[5 + 2 * registers + 1];

                // Build outgoing modbus message:
                BuildMessage(address, 4, start, registers, message);
                // Send modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);
                }

                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
            }
            return true;
        }

        public bool SendFc5(byte address, UInt16 start, UInt16 registers, UInt16[] values)
        {
            // Function 5

            // Ensure port is open:
            if ((this.IsOpen))
            {

                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Message is 1 addr + 1 fcn + 2 start + 2 * reg vals + 2 CRC
                byte[] message = new byte[5 + 2 * registers + 1];


                // Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                // Add bytecount to message:
                message[6] = System.Convert.ToByte(registers * 2);
                // Put write values into message prior to sending:
                int i;
                for (i = 0; i <= registers - 1; i++)
                {
                    message[4 + 2 * i] = System.Convert.ToByte(values[i] >> 8);
                    message[5 + 2 * i] = System.Convert.ToByte(values[i] & 0xFF);
                }
                // Build outgoing message:
                BuildMessage(address, 5, start, registers, message);

                // Send Modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);

                    return true;
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }

        public bool SendFc16(byte address, UInt16 start, UInt16 registers, UInt16[] values)
        {
            // Function 16
            // Ensure port is open:
            if ((this.IsOpen))
            {
                // Clear in/out buffers:
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                // Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[8 + 2 * registers + 1];


                // Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                // Add bytecount to message:
                message[6] = System.Convert.ToByte(registers * 2);
                // Put write values into message prior to sending:
                int i;
                for (i = 0; i <= registers - 1; i++)
                {
                    message[7 + 2 * i] = System.Convert.ToByte(values[i] >> 8);
                    message[8 + 2 * i] = System.Convert.ToByte(values[i] & 0xFF);
                }
                // Build outgoing message:
                BuildMessage(address, 16, start, registers, message);

                // Send Modbus message to Serial Port:
                try
                {
                    this.Write(message, 0, message.Length);

                    return true;
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }

        byte[] aIn = new byte[6];
        int BytesCount;

        private void Modbus_DataReceived(SerialPort sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // IMPORTANT: this catch the incoming message, if you want read what is incoming, 
            // catch the message MessageReceive (b() as Byte) from this class, you will lease the whole buffer
            // inclusive sclave and CRC

            BytesCount = 5; // Sclave, function,count + crc 1 & 2
            int i = 0;
            TimeSpan tim = new TimeSpan();
            DateTime tmp = DateTime.Now;
            tim = tmp.Subtract(DateTime.Now);

            // the speed of pc is higher than Arduino send, therefore I force him to stay here for 200 ms, in order to be sure that read whole 
            // msg

            while (BytesCount > 0 & tim.TotalMilliseconds < 200)
            {
                while (sender.BytesToRead > 0)
                {
                    tmp = DateTime.Now;
                    aIn[i] = (byte)sender.ReadByte();
                    BytesCount = BytesCount - 1;

                    // TODO: to improve, too much ReDim. 
                    if (i == 1 && aIn[1] == 16)
                        BytesCount = BytesCount + 3;
                    else if (i == 2 && aIn[1] < 129 & aIn[1] != 16)
                        BytesCount = BytesCount + aIn[2];
                    i = i + 1;


                    if (BytesCount == 0)
                    {
                        i = 0;
                        BytesCount = 5;
                        int j;
                        if (CheckResponse(aIn))
                            MessageRecived?.Invoke(aIn);
                    }
                }
                tim = DateTime.Now.Subtract(tmp);
            }
            return;
        }


    }

}
