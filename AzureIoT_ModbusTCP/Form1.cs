using System;
using System.Linq;
using System.Windows.Forms;
using NModbus;
using System.Net.Sockets;

namespace AzureIoT_ModbusTCP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSlaveStart_Click(object sender, EventArgs e)
        {
            ushort[] addresses = new ushort[5];
            addresses[0] = 0;
            addresses[1] = 1;
            addresses[2] = 2;
            addresses[3] = 3;
            addresses[4] = 4;

            try
            {
                ushort[] startingValues = CreateSlave(edtIP.Text.ToString(), Convert.ToInt32(edtPort.Text), Convert.ToByte(edtSlaveAddress.Text));

                var addressesAndstartingValues = addresses.Zip(startingValues, (first, second) => first + "\t" + second);

                foreach (var item in addressesAndstartingValues)
                {
                        rtbDataModbus.AppendText("\n" + item);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            BtnSlaveStart.Enabled = false;
            edtIP.Enabled = false;
            edtPort.Enabled = false;
            edtSlaveAddress.Enabled = false;
            BtnUpdateValues.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnSlaveStart.Enabled = true;
            edtIP.Enabled = true;
            edtPort.Enabled = true;
            edtSlaveAddress.Enabled = true;
            BtnReadNewValues.Enabled = false;
            BtnUpdateValues.Enabled = false;
        }

        private void BtnUpdateValues_Click(object sender, EventArgs e)
        {
            UInt16 StartAddress = 0;
            UInt16[] Data = new UInt16[5];

            Data[0] = (ushort)Convert.ToInt16(edtDataPoint1.Text);
            Data[1] = (ushort)Convert.ToInt16(edtDataPoint2.Text);
            Data[2] = (ushort)Convert.ToInt16(edtDataPoint3.Text);
            Data[3] = (ushort)Convert.ToInt16(edtDataPoint4.Text);
            Data[4] = (ushort)Convert.ToInt16(edtDataPoint5.Text);

            using (TcpClient client = new TcpClient(edtIP.Text, Convert.ToInt16(edtPort.Text)))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);

                master.WriteMultipleRegisters(Convert.ToByte(edtSlaveAddress.Text), StartAddress, Data);
            }

            BtnReadNewValues.Enabled = true;
        }

        private void BtnReadNewValues_Click(object sender, EventArgs e)
        {
            ushort numInputs = 5;
            ushort startAddress = 0;

            using (TcpClient client = new TcpClient(edtIP.Text, Convert.ToInt16(edtPort.Text)))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);

                ushort[] registerValues = master.ReadHoldingRegisters(Convert.ToByte(edtSlaveAddress.Text), startAddress, numInputs);

                rtbDataModbus.Clear();

                rtbDataModbus.AppendText("Address, Data");

                int i = 0;
                foreach (var record in registerValues)
                {
                    rtbDataModbus.AppendText("\n" + i.ToString() + "\t" + record.ToString());
                    i++;
                }
            }
        }

        private void BtnSendToAzure_Click(object sender, EventArgs e)
        {
            ushort numInputs = 5;
            ushort startAddress = 0;

            using (TcpClient client = new TcpClient(edtIP.Text, Convert.ToInt16(edtPort.Text)))
            {
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(client);

                ushort[] registerValues = master.ReadHoldingRegisters(Convert.ToByte(edtSlaveAddress.Text), startAddress, numInputs);

                AzureIoTConnectionAndSending(registerValues);
            }
        }
    }
}
