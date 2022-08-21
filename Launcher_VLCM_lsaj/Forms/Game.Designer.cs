namespace Launcher_VLCM_niua_lsaj.Forms
{
    partial class Game
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.FormBorder = new System.Windows.Forms.Panel();
            this.translationButton = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.minimize = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.maximize = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.exit = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.nameLabel = new System.Windows.Forms.Label();
            this.ApplicationSound = new Launcher_VLCM_niua_lsaj.CustomControls.CButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            this.FormBorder.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormBorder
            // 
            this.FormBorder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorder.Controls.Add(this.translationButton);
            this.FormBorder.Controls.Add(this.minimize);
            this.FormBorder.Controls.Add(this.maximize);
            this.FormBorder.Controls.Add(this.exit);
            this.FormBorder.Controls.Add(this.nameLabel);
            this.FormBorder.Controls.Add(this.ApplicationSound);
            this.FormBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormBorder.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.FormBorder.Location = new System.Drawing.Point(0, 0);
            this.FormBorder.Margin = new System.Windows.Forms.Padding(0);
            this.FormBorder.Name = "FormBorder";
            this.FormBorder.Size = new System.Drawing.Size(784, 30);
            this.FormBorder.TabIndex = 1;
            this.FormBorder.DoubleClick += new System.EventHandler(this.FormBorder_DoubleClick);
            this.FormBorder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormBorder_MouseDown);
            this.FormBorder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormBorder_MouseMove);
            this.FormBorder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormBorder_MouseUp);
            // 
            // translationButton
            // 
            this.translationButton.BackColor = System.Drawing.Color.Transparent;
            this.translationButton.BackgroundColor = System.Drawing.Color.Transparent;
            this.translationButton.BorderColor = System.Drawing.Color.Transparent;
            this.translationButton.BorderRadius = 0;
            this.translationButton.BorderSize = 0;
            this.translationButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.translationButton.FlatAppearance.BorderSize = 0;
            this.translationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.translationButton.ForeColor = System.Drawing.Color.Transparent;
            this.translationButton.Location = new System.Drawing.Point(235, 0);
            this.translationButton.Name = "translationButton";
            this.translationButton.Size = new System.Drawing.Size(30, 30);
            this.translationButton.TabIndex = 6;
            this.translationButton.TextColor = System.Drawing.Color.Transparent;
            this.translationButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.translationButton.UseVisualStyleBackColor = false;
            this.translationButton.Click += new System.EventHandler(this.translationButton_Click);
            // 
            // minimize
            // 
            this.minimize.BackColor = System.Drawing.Color.Transparent;
            this.minimize.BackgroundColor = System.Drawing.Color.Transparent;
            this.minimize.BorderColor = System.Drawing.Color.Transparent;
            this.minimize.BorderRadius = 0;
            this.minimize.BorderSize = 0;
            this.minimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.minimize.FlatAppearance.BorderSize = 0;
            this.minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimize.ForeColor = System.Drawing.Color.Transparent;
            this.minimize.Location = new System.Drawing.Point(694, 0);
            this.minimize.Name = "minimize";
            this.minimize.Size = new System.Drawing.Size(30, 30);
            this.minimize.TabIndex = 4;
            this.minimize.TextColor = System.Drawing.Color.Transparent;
            this.minimize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.minimize.UseVisualStyleBackColor = false;
            this.minimize.Click += new System.EventHandler(this.minimize_Click);
            // 
            // maximize
            // 
            this.maximize.BackColor = System.Drawing.Color.Transparent;
            this.maximize.BackgroundColor = System.Drawing.Color.Transparent;
            this.maximize.BorderColor = System.Drawing.Color.Transparent;
            this.maximize.BorderRadius = 0;
            this.maximize.BorderSize = 0;
            this.maximize.Dock = System.Windows.Forms.DockStyle.Right;
            this.maximize.FlatAppearance.BorderSize = 0;
            this.maximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maximize.ForeColor = System.Drawing.Color.Transparent;
            this.maximize.Location = new System.Drawing.Point(724, 0);
            this.maximize.Name = "maximize";
            this.maximize.Size = new System.Drawing.Size(30, 30);
            this.maximize.TabIndex = 3;
            this.maximize.TextColor = System.Drawing.Color.Transparent;
            this.maximize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.maximize.UseVisualStyleBackColor = false;
            this.maximize.Click += new System.EventHandler(this.maximize_Click);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.Transparent;
            this.exit.BackgroundColor = System.Drawing.Color.Transparent;
            this.exit.BorderColor = System.Drawing.Color.Transparent;
            this.exit.BorderRadius = 0;
            this.exit.BorderSize = 0;
            this.exit.Dock = System.Windows.Forms.DockStyle.Right;
            this.exit.FlatAppearance.BorderSize = 0;
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.ForeColor = System.Drawing.Color.Transparent;
            this.exit.Location = new System.Drawing.Point(754, 0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(30, 30);
            this.exit.TabIndex = 2;
            this.exit.TextColor = System.Drawing.Color.Transparent;
            this.exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.nameLabel.Font = new System.Drawing.Font("Script MT Bold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.nameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nameLabel.Location = new System.Drawing.Point(30, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(205, 21);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Launcher - lsaj.niua.com";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.nameLabel.DoubleClick += new System.EventHandler(this.nameLabel_DoubleClick);
            this.nameLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nameLabel_MouseDown);
            this.nameLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.nameLabel_MouseMove);
            this.nameLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.nameLabel_MouseUp);
            // 
            // ApplicationSound
            // 
            this.ApplicationSound.BackColor = System.Drawing.Color.Transparent;
            this.ApplicationSound.BackgroundColor = System.Drawing.Color.Transparent;
            this.ApplicationSound.BorderColor = System.Drawing.Color.Transparent;
            this.ApplicationSound.BorderRadius = 0;
            this.ApplicationSound.BorderSize = 0;
            this.ApplicationSound.Dock = System.Windows.Forms.DockStyle.Left;
            this.ApplicationSound.FlatAppearance.BorderSize = 0;
            this.ApplicationSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplicationSound.ForeColor = System.Drawing.Color.Transparent;
            this.ApplicationSound.Location = new System.Drawing.Point(0, 0);
            this.ApplicationSound.Name = "ApplicationSound";
            this.ApplicationSound.Size = new System.Drawing.Size(30, 30);
            this.ApplicationSound.TabIndex = 0;
            this.ApplicationSound.TextColor = System.Drawing.Color.Transparent;
            this.ApplicationSound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ApplicationSound.UseVisualStyleBackColor = false;
            this.ApplicationSound.Click += new System.EventHandler(this.ApplicationSound_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.FormBorder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.axShockwaveFlash, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 561);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // axShockwaveFlash
            // 
            this.axShockwaveFlash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axShockwaveFlash.Enabled = true;
            this.axShockwaveFlash.Location = new System.Drawing.Point(3, 33);
            this.axShockwaveFlash.Name = "axShockwaveFlash";
            this.axShockwaveFlash.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axShockwaveFlash.OcxState")));
            this.axShockwaveFlash.Size = new System.Drawing.Size(778, 525);
            this.axShockwaveFlash.TabIndex = 2;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(150, 150);
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher VLCM - lsaj.niua.com";
            this.Load += new System.EventHandler(this.Game_Load);
            this.Resize += new System.EventHandler(this.Game_Resize);
            this.FormBorder.ResumeLayout(false);
            this.FormBorder.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel FormBorder;
        private CustomControls.CButton translationButton;
        private CustomControls.CButton minimize;
        private CustomControls.CButton maximize;
        private CustomControls.CButton exit;
        private System.Windows.Forms.Label nameLabel;
        private CustomControls.CButton ApplicationSound;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash;
    }
}