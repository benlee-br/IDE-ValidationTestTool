
namespace CombineCSVFilesFromTwoFolders
{
    partial class frmCombineResult
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtFolder1 = new System.Windows.Forms.TextBox();
            this.txtFolder2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.btnCombine = new System.Windows.Forms.Button();
            this.txtErrorFileFound = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsFileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder 1";
            // 
            // txtFolder1
            // 
            this.txtFolder1.Location = new System.Drawing.Point(170, 60);
            this.txtFolder1.Margin = new System.Windows.Forms.Padding(4);
            this.txtFolder1.Name = "txtFolder1";
            this.txtFolder1.Size = new System.Drawing.Size(702, 26);
            this.txtFolder1.TabIndex = 1;
            // 
            // txtFolder2
            // 
            this.txtFolder2.Location = new System.Drawing.Point(170, 121);
            this.txtFolder2.Margin = new System.Windows.Forms.Padding(4);
            this.txtFolder2.Name = "txtFolder2";
            this.txtFolder2.Size = new System.Drawing.Size(702, 26);
            this.txtFolder2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 125);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Folder 2";
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(878, 59);
            this.btnBrowse1.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(37, 31);
            this.btnBrowse1.TabIndex = 4;
            this.btnBrowse1.Text = "...";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(878, 120);
            this.btnBrowse2.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(37, 31);
            this.btnBrowse2.TabIndex = 5;
            this.btnBrowse2.Text = "...";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // btnCombine
            // 
            this.btnCombine.Location = new System.Drawing.Point(807, 179);
            this.btnCombine.Margin = new System.Windows.Forms.Padding(4);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(96, 31);
            this.btnCombine.TabIndex = 6;
            this.btnCombine.Text = "Combine";
            this.btnCombine.UseVisualStyleBackColor = true;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // txtErrorFileFound
            // 
            this.txtErrorFileFound.Location = new System.Drawing.Point(54, 229);
            this.txtErrorFileFound.Multiline = true;
            this.txtErrorFileFound.Name = "txtErrorFileFound";
            this.txtErrorFileFound.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrorFileFound.Size = new System.Drawing.Size(903, 242);
            this.txtErrorFileFound.TabIndex = 8;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCount,
            this.tsProgress,
            this.tsFileName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 634);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1230, 28);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsCount
            // 
            this.tsCount.AutoSize = false;
            this.tsCount.Name = "tsCount";
            this.tsCount.Size = new System.Drawing.Size(150, 21);
            // 
            // tsProgress
            // 
            this.tsProgress.AutoSize = false;
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(150, 20);
            // 
            // tsFileName
            // 
            this.tsFileName.Name = "tsFileName";
            this.tsFileName.Size = new System.Drawing.Size(0, 21);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "File Contains Error";
            // 
            // frmCombineResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 662);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtErrorFileFound);
            this.Controls.Add(this.btnCombine);
            this.Controls.Add(this.btnBrowse2);
            this.Controls.Add(this.btnBrowse1);
            this.Controls.Add(this.txtFolder2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFolder1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCombineResult";
            this.Text = "Combine Files From Two Folders - Version 6";
            this.Load += new System.EventHandler(this.frmCombineResult_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFolder1;
        private System.Windows.Forms.TextBox txtFolder2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.TextBox txtErrorFileFound;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsCount;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.ToolStripStatusLabel tsFileName;
        private System.Windows.Forms.Label label3;
    }
}

