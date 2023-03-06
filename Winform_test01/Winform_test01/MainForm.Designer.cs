
namespace Winform_test01
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.cboOptions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTextcount = new System.Windows.Forms.Label();
            this.lblLinecount = new System.Windows.Forms.Label();
            this.lblLayercount = new System.Windows.Forms.Label();
            this.lstTextstyle = new System.Windows.Forms.ListBox();
            this.lstLinetype = new System.Windows.Forms.ListBox();
            this.lstLayer = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnDisplay);
            this.groupBox1.Controls.Add(this.cboOptions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 402);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option Container";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(215, 373);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(89, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(55, 373);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(93, 23);
            this.btnDisplay.TabIndex = 2;
            this.btnDisplay.Text = "Show Items";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // cboOptions
            // 
            this.cboOptions.FormattingEnabled = true;
            this.cboOptions.Items.AddRange(new object[] {
            "All",
            "Layer",
            "LineType",
            "TextStyle"});
            this.cboOptions.Location = new System.Drawing.Point(165, 21);
            this.cboOptions.Name = "cboOptions";
            this.cboOptions.Size = new System.Drawing.Size(173, 20);
            this.cboOptions.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Item to Display";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTextcount);
            this.groupBox2.Controls.Add(this.lblLinecount);
            this.groupBox2.Controls.Add(this.lblLayercount);
            this.groupBox2.Controls.Add(this.lstTextstyle);
            this.groupBox2.Controls.Add(this.lstLinetype);
            this.groupBox2.Controls.Add(this.lstLayer);
            this.groupBox2.Location = new System.Drawing.Point(386, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 402);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Displau Container";
            // 
            // lblTextcount
            // 
            this.lblTextcount.AutoSize = true;
            this.lblTextcount.Location = new System.Drawing.Point(274, 378);
            this.lblTextcount.Name = "lblTextcount";
            this.lblTextcount.Size = new System.Drawing.Size(17, 12);
            this.lblTextcount.TabIndex = 5;
            this.lblTextcount.Text = "...";
            // 
            // lblLinecount
            // 
            this.lblLinecount.AutoSize = true;
            this.lblLinecount.Location = new System.Drawing.Point(138, 378);
            this.lblLinecount.Name = "lblLinecount";
            this.lblLinecount.Size = new System.Drawing.Size(17, 12);
            this.lblLinecount.TabIndex = 4;
            this.lblLinecount.Text = "...";
            // 
            // lblLayercount
            // 
            this.lblLayercount.AutoSize = true;
            this.lblLayercount.Location = new System.Drawing.Point(7, 373);
            this.lblLayercount.Name = "lblLayercount";
            this.lblLayercount.Size = new System.Drawing.Size(17, 12);
            this.lblLayercount.TabIndex = 3;
            this.lblLayercount.Text = "...";
            // 
            // lstTextstyle
            // 
            this.lstTextstyle.FormattingEnabled = true;
            this.lstTextstyle.ItemHeight = 12;
            this.lstTextstyle.Location = new System.Drawing.Point(274, 31);
            this.lstTextstyle.Name = "lstTextstyle";
            this.lstTextstyle.Size = new System.Drawing.Size(122, 340);
            this.lstTextstyle.TabIndex = 2;
            // 
            // lstLinetype
            // 
            this.lstLinetype.FormattingEnabled = true;
            this.lstLinetype.ItemHeight = 12;
            this.lstLinetype.Location = new System.Drawing.Point(140, 32);
            this.lstLinetype.Name = "lstLinetype";
            this.lstLinetype.Size = new System.Drawing.Size(128, 340);
            this.lstLinetype.TabIndex = 1;
            // 
            // lstLayer
            // 
            this.lstLayer.FormattingEnabled = true;
            this.lstLayer.ItemHeight = 12;
            this.lstLayer.Location = new System.Drawing.Point(6, 31);
            this.lstLayer.Name = "lstLayer";
            this.lstLayer.Size = new System.Drawing.Size(128, 340);
            this.lstLayer.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.ComboBox cboOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTextcount;
        private System.Windows.Forms.Label lblLinecount;
        private System.Windows.Forms.Label lblLayercount;
        private System.Windows.Forms.ListBox lstTextstyle;
        private System.Windows.Forms.ListBox lstLinetype;
        private System.Windows.Forms.ListBox lstLayer;
    }
}