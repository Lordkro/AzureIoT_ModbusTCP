namespace AzureIoT_ModbusTCP
{
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;
    using NModbus;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Forms;

    public static class Constants
    {
        public const string iotConnectionString = "HostName=ModbusTCP-IoTHub.azure-devices.net;DeviceId=ModBusDevice;SharedAccessKey=nL9qnx6hAB0iJdRK8HXvzvlZ0fR+f1A7GAIoTGaV378=";
    }

    public class ModbusData
    {
        public ushort[] Registers { get; set; }
    }

    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private ushort[] CreateSlave(string address, int port, byte slaveAddress)
        {
            IPAddress ipAddress = IPAddress.Parse(address);

            TcpListener slaveTcpListener = new TcpListener(ipAddress, port);
            slaveTcpListener.Start();

            var factory = new ModbusFactory();
            var network = factory.CreateSlaveNetwork(slaveTcpListener);

            IModbusSlave slave = factory.CreateSlave(slaveAddress);

            ushort[] slaveStartingValues = { 0, 0, 0, 0, 0 };

            slave.DataStore.HoldingRegisters.WritePoints(0, slaveStartingValues);

            network.AddSlave(slave);
            var listenTask = network.ListenAsync();

            return slaveStartingValues;
        }

        private async void AzureIoTConnectionAndSending(ushort[] registers)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(Constants.iotConnectionString);

            ModbusData data = new ModbusData { Registers = registers };

            string json = JsonConvert.SerializeObject(data);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(json));
            
            await deviceClient.SendEventAsync(message);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.edtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edtPort = new System.Windows.Forms.TextBox();
            this.BtnSlaveStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.edtSlaveAddress = new System.Windows.Forms.TextBox();
            this.rtbDataModbus = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.edtDataPoint1 = new System.Windows.Forms.TextBox();
            this.edtDataPoint2 = new System.Windows.Forms.TextBox();
            this.edtDataPoint3 = new System.Windows.Forms.TextBox();
            this.edtDataPoint4 = new System.Windows.Forms.TextBox();
            this.edtDataPoint5 = new System.Windows.Forms.TextBox();
            this.BtnUpdateValues = new System.Windows.Forms.Button();
            this.BtnReadNewValues = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.BtnSendToAzure = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address";
            // 
            // edtIP
            // 
            this.edtIP.Location = new System.Drawing.Point(129, 31);
            this.edtIP.Name = "edtIP";
            this.edtIP.Size = new System.Drawing.Size(101, 20);
            this.edtIP.TabIndex = 1;
            this.edtIP.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(81, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // edtPort
            // 
            this.edtPort.Location = new System.Drawing.Point(129, 63);
            this.edtPort.Name = "edtPort";
            this.edtPort.Size = new System.Drawing.Size(100, 20);
            this.edtPort.TabIndex = 3;
            this.edtPort.Text = "502";
            // 
            // BtnSlaveStart
            // 
            this.BtnSlaveStart.Location = new System.Drawing.Point(84, 135);
            this.BtnSlaveStart.Name = "BtnSlaveStart";
            this.BtnSlaveStart.Size = new System.Drawing.Size(105, 26);
            this.BtnSlaveStart.TabIndex = 4;
            this.BtnSlaveStart.Text = "Start Slave";
            this.BtnSlaveStart.UseVisualStyleBackColor = true;
            this.BtnSlaveStart.Click += new System.EventHandler(this.BtnSlaveStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(60, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Slave ID";
            // 
            // edtSlaveAddress
            // 
            this.edtSlaveAddress.Location = new System.Drawing.Point(129, 98);
            this.edtSlaveAddress.Name = "edtSlaveAddress";
            this.edtSlaveAddress.Size = new System.Drawing.Size(100, 20);
            this.edtSlaveAddress.TabIndex = 6;
            this.edtSlaveAddress.Text = "1";
            // 
            // rtbDataModbus
            // 
            this.rtbDataModbus.Location = new System.Drawing.Point(323, 16);
            this.rtbDataModbus.Name = "rtbDataModbus";
            this.rtbDataModbus.Size = new System.Drawing.Size(105, 145);
            this.rtbDataModbus.TabIndex = 7;
            this.rtbDataModbus.Text = "Address, Data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(58, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Master Data Update";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "DataPoint #1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "DataPoint #2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(43, 275);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "DataPoint #3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 299);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "DataPoint #4";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(43, 324);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "DataPoint #5";
            // 
            // edtDataPoint1
            // 
            this.edtDataPoint1.Location = new System.Drawing.Point(119, 217);
            this.edtDataPoint1.Name = "edtDataPoint1";
            this.edtDataPoint1.Size = new System.Drawing.Size(100, 20);
            this.edtDataPoint1.TabIndex = 14;
            this.edtDataPoint1.Text = "0";
            // 
            // edtDataPoint2
            // 
            this.edtDataPoint2.Location = new System.Drawing.Point(119, 246);
            this.edtDataPoint2.Name = "edtDataPoint2";
            this.edtDataPoint2.Size = new System.Drawing.Size(100, 20);
            this.edtDataPoint2.TabIndex = 15;
            this.edtDataPoint2.Text = "0";
            // 
            // edtDataPoint3
            // 
            this.edtDataPoint3.Location = new System.Drawing.Point(119, 272);
            this.edtDataPoint3.Name = "edtDataPoint3";
            this.edtDataPoint3.Size = new System.Drawing.Size(100, 20);
            this.edtDataPoint3.TabIndex = 16;
            this.edtDataPoint3.Text = "0";
            // 
            // edtDataPoint4
            // 
            this.edtDataPoint4.Location = new System.Drawing.Point(119, 296);
            this.edtDataPoint4.Name = "edtDataPoint4";
            this.edtDataPoint4.Size = new System.Drawing.Size(100, 20);
            this.edtDataPoint4.TabIndex = 17;
            this.edtDataPoint4.Text = "0";
            // 
            // edtDataPoint5
            // 
            this.edtDataPoint5.Location = new System.Drawing.Point(119, 321);
            this.edtDataPoint5.Name = "edtDataPoint5";
            this.edtDataPoint5.Size = new System.Drawing.Size(100, 20);
            this.edtDataPoint5.TabIndex = 18;
            this.edtDataPoint5.Text = "0";
            // 
            // BtnUpdateValues
            // 
            this.BtnUpdateValues.Location = new System.Drawing.Point(70, 352);
            this.BtnUpdateValues.Name = "BtnUpdateValues";
            this.BtnUpdateValues.Size = new System.Drawing.Size(133, 23);
            this.BtnUpdateValues.TabIndex = 19;
            this.BtnUpdateValues.Text = "Update MasterData";
            this.BtnUpdateValues.UseVisualStyleBackColor = true;
            this.BtnUpdateValues.Click += new System.EventHandler(this.BtnUpdateValues_Click);
            // 
            // BtnReadNewValues
            // 
            this.BtnReadNewValues.Location = new System.Drawing.Point(323, 181);
            this.BtnReadNewValues.Name = "BtnReadNewValues";
            this.BtnReadNewValues.Size = new System.Drawing.Size(105, 23);
            this.BtnReadNewValues.TabIndex = 20;
            this.BtnReadNewValues.Text = "Read New Values";
            this.BtnReadNewValues.UseVisualStyleBackColor = true;
            this.BtnReadNewValues.Click += new System.EventHandler(this.BtnReadNewValues_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(310, 240);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(169, 16);
            this.label10.TabIndex = 21;
            this.label10.Text = "Send DataPoints to Azure";
            // 
            // BtnSendToAzure
            // 
            this.BtnSendToAzure.Location = new System.Drawing.Point(303, 269);
            this.BtnSendToAzure.Name = "BtnSendToAzure";
            this.BtnSendToAzure.Size = new System.Drawing.Size(135, 23);
            this.BtnSendToAzure.TabIndex = 22;
            this.BtnSendToAzure.Text = "Send to IoT Hub Device";
            this.BtnSendToAzure.UseVisualStyleBackColor = true;
            this.BtnSendToAzure.Click += new System.EventHandler(this.BtnSendToAzure_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(31, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 18);
            this.label11.TabIndex = 23;
            this.label11.Text = "Step 1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 188);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 18);
            this.label12.TabIndex = 24;
            this.label12.Text = "Step 2";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(268, 184);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 18);
            this.label13.TabIndex = 25;
            this.label13.Text = "Step 3";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(255, 240);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 18);
            this.label14.TabIndex = 26;
            this.label14.Text = "Step 4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 394);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.BtnSendToAzure);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BtnReadNewValues);
            this.Controls.Add(this.BtnUpdateValues);
            this.Controls.Add(this.edtDataPoint5);
            this.Controls.Add(this.edtDataPoint4);
            this.Controls.Add(this.edtDataPoint3);
            this.Controls.Add(this.edtDataPoint2);
            this.Controls.Add(this.edtDataPoint1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtbDataModbus);
            this.Controls.Add(this.edtSlaveAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnSlaveStart);
            this.Controls.Add(this.edtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edtIP);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Azure IoT Device Data Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edtPort;
        private System.Windows.Forms.Button BtnSlaveStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox edtSlaveAddress;
        private System.Windows.Forms.RichTextBox rtbDataModbus;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox edtDataPoint1;
        private TextBox edtDataPoint2;
        private TextBox edtDataPoint3;
        private TextBox edtDataPoint4;
        private TextBox edtDataPoint5;
        private Button BtnUpdateValues;
        private Button BtnReadNewValues;
        private Label label10;
        private Button BtnSendToAzure;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
    }
}

