namespace EighthBit.Can.Nmg
{
    partial class AxCanHost
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AxCanHost));
            this.axCanToUsb1 = new AxCanToUSBConverter.AxCanToUsb();
            ((System.ComponentModel.ISupportInitialize)(this.axCanToUsb1)).BeginInit();
            this.SuspendLayout();
            // 
            // axCanToUsb1
            // 
            this.axCanToUsb1.Enabled = true;
            this.axCanToUsb1.Location = new System.Drawing.Point(0, 0);
            this.axCanToUsb1.Name = "axCanToUsb1";
            this.axCanToUsb1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCanToUsb1.OcxState")));
            this.axCanToUsb1.Size = new System.Drawing.Size(32, 32);
            this.axCanToUsb1.TabIndex = 0;
            // 
            // AxCanHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.axCanToUsb1);
            this.Name = "AxCanHost";
            this.Text = "AxCanHost";
            ((System.ComponentModel.ISupportInitialize)(this.axCanToUsb1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxCanToUSBConverter.AxCanToUsb axCanToUsb1;
    }
}