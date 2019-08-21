namespace ICAN.SIC.Plugin.ICANGEOLOCATE.Host
{
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnCallGeoRequest = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCallGeoRequest
            // 
            this.btnCallGeoRequest.Location = new System.Drawing.Point(613, 474);
            this.btnCallGeoRequest.Name = "btnCallGeoRequest";
            this.btnCallGeoRequest.Size = new System.Drawing.Size(175, 54);
            this.btnCallGeoRequest.TabIndex = 0;
            this.btnCallGeoRequest.Text = "Call";
            this.btnCallGeoRequest.UseVisualStyleBackColor = true;
            this.btnCallGeoRequest.Click += new System.EventHandler(this.btnCallGeoRequest_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(776, 31);
            this.txtInput.TabIndex = 1;
            this.txtInput.Text = "GetCurrentCoordinates";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 130);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(538, 398);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnCallGeoRequest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCallGeoRequest;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox textBox1;
    }
}

