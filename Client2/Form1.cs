using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.IntegralTransforms; // 需安裝 MathNet.Numerics via NuGet
using System.Numerics;

namespace IQReceiver
{
    public partial class Form1 : Form
    {
        // ===== 常數 =====
        private const uint MAGIC = 0x53505244; // "SPRD"
        private const int HEADER_SIZE = 8;

        // ===== TCP =====
        private TcpClient client;
        private NetworkStream stream;
        private byte[] recvBuffer = new byte[65536];
        private List<byte> buffer = new List<byte>();

        // ===== FFT / Spectrum / Waterfall =====
        private float[] latestFFT;
        private Timer fftTimer;
        private int waterfallRow = 0;

        // ===== IQ 儲存 =====
        private bool saveIQ = false;
        private FileStream iqFile;

        public Form1()
        {
            InitializeComponent();
        }

        // ================= Form Load =================
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupChart();
            SetupWaterfall();

            fftTimer = new Timer();
            fftTimer.Interval = 33; // 30 FPS
            fftTimer.Tick += (s, ev) =>
            {
                if (latestFFT != null)
                {
                    UpdateSpectrum(latestFFT);
                    UpdateWaterfall(latestFFT);
                }
            };
            fftTimer.Start();
        }

        // ================= Setup Spectrum Chart =================
        private void SetupChart()
        {
            chartSpectrum.Series.Clear();
            var s = new Series("Spectrum");
            s.ChartType = SeriesChartType.FastLine;
            chartSpectrum.Series.Add(s);
            chartSpectrum.ChartAreas[0].AxisX.Minimum = 0;
            chartSpectrum.ChartAreas[0].AxisX.Maximum = 1024;
            chartSpectrum.ChartAreas[0].AxisY.Minimum = -150;
            chartSpectrum.ChartAreas[0].AxisY.Maximum = 10;
        }

        // ================= Setup Waterfall Chart =================
        private void SetupWaterfall()
        {
            chartWaterfall.Series.Clear();
            var s = new Series("Waterfall");
            s.ChartType = SeriesChartType.FastPoint;
            s.MarkerStyle = MarkerStyle.Square;
            s.MarkerSize = 2;
            chartWaterfall.Series.Add(s);
            chartWaterfall.ChartAreas[0].AxisX.Minimum = 0;
            chartWaterfall.ChartAreas[0].AxisX.Maximum = 1024;
            chartWaterfall.ChartAreas[0].AxisY.Minimum = 0;
            chartWaterfall.ChartAreas[0].AxisY.Maximum = 512;
        }

        // ================= Connect 按鈕 =================
        private void btnConnect_Click(object sender, EventArgs e)
        {
            StartClient();
        }

        // ================= TCP Client =================
        private void StartClient()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 12345);
            stream = client.GetStream();

            Task.Run(() => ReceiveLoop());
        }

        // ================= TCP Receive Loop =================
        private void ReceiveLoop()
        {
            try
            {
                while (true)
                {
                    int bytes = stream.Read(recvBuffer, 0, recvBuffer.Length);
                    if (bytes <= 0)
                    {
                        Console.WriteLine("Server disconnected");
                        break;
                    }
                    buffer.AddRange(recvBuffer.Take(bytes));
                    ParsePackets();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TCP Error: " + ex.Message);
            }
        }

        // ================= Parse Packets =================
        private void ParsePackets()
        {
            while (buffer.Count >= HEADER_SIZE)
            {
                uint magic = BitConverter.ToUInt32(buffer.ToArray(), 0);
                uint length = BitConverter.ToUInt32(buffer.ToArray(), 4);

                if (magic != MAGIC)
                {
                    buffer.RemoveAt(0); // 左移 1 byte
                    continue;
                }

                int totalLen = HEADER_SIZE + (int)length;
                if (buffer.Count < totalLen)
                    return;

                byte[] iq = buffer.Skip(HEADER_SIZE).Take((int)length).ToArray();

                if (saveIQ && iqFile != null)
                    iqFile.Write(iq, 0, iq.Length);

                AddIQBlock(iq);
                buffer.RemoveRange(0, totalLen);
            }
        }

        // ================= IQ -> FFT =================
        private void AddIQBlock(byte[] iq)
        {
            int n = iq.Length / 2; // 假設 I8+Q8
            Complex[] data = new Complex[n];

            for (int i = 0; i < n; i++)
            {
                float iVal = (sbyte)iq[i * 2];
                float qVal = (sbyte)iq[i * 2 + 1];
                data[i] = new Complex(iVal, qVal);
            }

            // FFT
            Fourier.Forward(data, FourierOptions.Matlab);

            float[] fft = new float[n];
            for (int i = 0; i < n; i++)
            {
                fft[i] = 20f * (float)Math.Log10(data[i].Magnitude + 1e-6); // dB
            }

            latestFFT = fft;
        }

        // ================= Update Spectrum =================
        private void UpdateSpectrum(float[] fft)
        {
            if (chartSpectrum.InvokeRequired)
            {
                chartSpectrum.Invoke(new Action(() => UpdateSpectrum(fft)));
                return;
            }

            var s = chartSpectrum.Series[0];
            s.Points.Clear();

            for (int i = 0; i < fft.Length; i++)
                s.Points.AddY(fft[i]);
        }

        // ================= Update Waterfall =================
        private void UpdateWaterfall(float[] fft)
        {
            if (chartWaterfall.InvokeRequired)
            {
                chartWaterfall.Invoke(new Action(() => UpdateWaterfall(fft)));
                return;
            }

            var s = chartWaterfall.Series[0];
            s.Points.Clear();

            int width = fft.Length;
            int height = 512;
            waterfallRow = (waterfallRow + 1) % height;

            for (int i = 0; i < width; i++)
            {
                // Y 軸 = row, X = freq, Color = dB
                double y = waterfallRow;
                double val = fft[i];
                double color = Math.Min(Math.Max(val, -150), 10);
                s.Points.AddXY(i, y, color);
            }
        }

        // ================= Save IQ Toggle =================
        private void chkSaveIQ_CheckedChanged(object sender, EventArgs e)
        {
            saveIQ = chkSaveIQ.Checked;
            if (saveIQ)
            {
                string filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".iq";
                iqFile = new FileStream(filename, FileMode.Create);
            }
            else
            {
                iqFile?.Close();
                iqFile = null;
            }
        }
    }
}
