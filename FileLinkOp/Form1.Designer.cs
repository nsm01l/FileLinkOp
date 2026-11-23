namespace FileLinkOp
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkOnTop = new System.Windows.Forms.CheckBox();
            this.lblDnD = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.chkOnTop);
            this.panel1.Controls.Add(this.lblDnD);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(142, 119);
            this.panel1.TabIndex = 0;
            // 
            // chkOnTop
            // 
            this.chkOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkOnTop.AutoSize = true;
            this.chkOnTop.Checked = true;
            this.chkOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnTop.Location = new System.Drawing.Point(33, 98);
            this.chkOnTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkOnTop.Name = "chkOnTop";
            this.chkOnTop.Size = new System.Drawing.Size(100, 16);
            this.chkOnTop.TabIndex = 1;
            this.chkOnTop.Text = "Always on Top";
            this.chkOnTop.UseVisualStyleBackColor = true;
            // 
            // lblDnD
            // 
            this.lblDnD.AllowDrop = true;
            this.lblDnD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDnD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDnD.Location = new System.Drawing.Point(9, 7);
            this.lblDnD.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDnD.Name = "lblDnD";
            this.lblDnD.Size = new System.Drawing.Size(125, 89);
            this.lblDnD.TabIndex = 0;
            this.lblDnD.Text = "Drag and Drop Here";
            this.lblDnD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDnD.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblDnD_DragDrop);
            this.lblDnD.DragEnter += new System.Windows.Forms.DragEventHandler(this.lblDnD_DragEnter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(142, 119);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(158, 158);
            this.Name = "Form1";
            this.Text = "FileLinkOp";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkOnTop;
        private System.Windows.Forms.Label lblDnD;
    }
}

