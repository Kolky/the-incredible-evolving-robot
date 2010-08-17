namespace TierBossTemplateCreator
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
      this.pnlMain = new System.Windows.Forms.Panel();
      this.pnlSelect = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // pnlMain
      // 
      this.pnlMain.AutoSize = true;
      this.pnlMain.Location = new System.Drawing.Point(218, 12);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new System.Drawing.Size(628, 465);
      this.pnlMain.TabIndex = 0;
      // 
      // pnlSelect
      // 
      this.pnlSelect.AutoSize = true;
      this.pnlSelect.Location = new System.Drawing.Point(12, 12);
      this.pnlSelect.MaximumSize = new System.Drawing.Size(200, 2000);
      this.pnlSelect.Name = "pnlSelect";
      this.pnlSelect.Size = new System.Drawing.Size(200, 465);
      this.pnlSelect.TabIndex = 1;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(858, 489);
      this.Controls.Add(this.pnlSelect);
      this.Controls.Add(this.pnlMain);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel pnlMain;
    private System.Windows.Forms.Panel pnlSelect;
  }
}

