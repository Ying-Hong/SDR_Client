namespace IQReceiver
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chartSpectrum = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartWaterfall = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkSaveIQ = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartWaterfall)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(877, 118);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(210, 75);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chartSpectrum
            // 
            chartArea3.Name = "ChartArea1";
            this.chartSpectrum.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartSpectrum.Legends.Add(legend3);
            this.chartSpectrum.Location = new System.Drawing.Point(12, 12);
            this.chartSpectrum.Name = "chartSpectrum";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartSpectrum.Series.Add(series3);
            this.chartSpectrum.Size = new System.Drawing.Size(625, 422);
            this.chartSpectrum.TabIndex = 2;
            this.chartSpectrum.Text = "chartSpectrum";
            // 
            // chartWaterfall
            // 
            chartArea4.Name = "ChartArea1";
            this.chartWaterfall.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartWaterfall.Legends.Add(legend4);
            this.chartWaterfall.Location = new System.Drawing.Point(516, 394);
            this.chartWaterfall.Name = "chartWaterfall";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartWaterfall.Series.Add(series4);
            this.chartWaterfall.Size = new System.Drawing.Size(625, 422);
            this.chartWaterfall.TabIndex = 3;
            this.chartWaterfall.Text = "chartWaterfall";
            // 
            // chkSaveIQ
            // 
            this.chkSaveIQ.AutoSize = true;
            this.chkSaveIQ.Location = new System.Drawing.Point(679, 142);
            this.chkSaveIQ.Name = "chkSaveIQ";
            this.chkSaveIQ.Size = new System.Drawing.Size(86, 28);
            this.chkSaveIQ.TabIndex = 4;
            this.chkSaveIQ.Text = "Save";
            this.chkSaveIQ.UseVisualStyleBackColor = true;
            this.chkSaveIQ.CheckedChanged += new System.EventHandler(this.chkSaveIQ_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 828);
            this.Controls.Add(this.chkSaveIQ);
            this.Controls.Add(this.chartWaterfall);
            this.Controls.Add(this.chartSpectrum);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartWaterfall)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpectrum;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartWaterfall;
        private System.Windows.Forms.CheckBox chkSaveIQ;
    }
}

