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
            components = new System.ComponentModel.Container();
            imageList1 = new System.Windows.Forms.ImageList(components);
            FormBorder = new System.Windows.Forms.Panel();
            reset = new CustomControls.CButton();
            restart = new CustomControls.CButton();
            translationButton = new CustomControls.CButton();
            minimize = new CustomControls.CButton();
            maximize = new CustomControls.CButton();
            exit = new CustomControls.CButton();
            nameLabel = new System.Windows.Forms.Label();
            ApplicationSound = new CustomControls.CButton();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            FormBorder.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormBorder
            // 
            FormBorder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            FormBorder.Controls.Add(reset);
            FormBorder.Controls.Add(restart);
            FormBorder.Controls.Add(translationButton);
            FormBorder.Controls.Add(minimize);
            FormBorder.Controls.Add(maximize);
            FormBorder.Controls.Add(exit);
            FormBorder.Controls.Add(nameLabel);
            FormBorder.Controls.Add(ApplicationSound);
            FormBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            FormBorder.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            FormBorder.Location = new System.Drawing.Point(0, 0);
            FormBorder.Margin = new System.Windows.Forms.Padding(0);
            FormBorder.Name = "FormBorder";
            FormBorder.Size = new System.Drawing.Size(784, 30);
            FormBorder.TabIndex = 1;
            FormBorder.DoubleClick += FormBorder_DoubleClick;
            FormBorder.MouseDown += FormBorder_MouseDown;
            FormBorder.MouseMove += FormBorder_MouseMove;
            FormBorder.MouseUp += FormBorder_MouseUp;
            // 
            // reset
            // 
            reset.BackColor = System.Drawing.Color.Transparent;
            reset.BackgroundColor = System.Drawing.Color.Transparent;
            reset.BorderColor = System.Drawing.Color.Transparent;
            reset.BorderRadius = 0;
            reset.BorderSize = 0;
            reset.Dock = System.Windows.Forms.DockStyle.Left;
            reset.FlatAppearance.BorderSize = 0;
            reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            reset.ForeColor = System.Drawing.Color.Transparent;
            reset.Location = new System.Drawing.Point(196, 0);
            reset.Name = "reset";
            reset.Size = new System.Drawing.Size(30, 30);
            reset.TabIndex = 8;
            reset.TextColor = System.Drawing.Color.Transparent;
            reset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            reset.UseVisualStyleBackColor = false;
            reset.Click += reset_Click;
            // 
            // restart
            // 
            restart.BackColor = System.Drawing.Color.Transparent;
            restart.BackgroundColor = System.Drawing.Color.Transparent;
            restart.BorderColor = System.Drawing.Color.Transparent;
            restart.BorderRadius = 0;
            restart.BorderSize = 0;
            restart.Dock = System.Windows.Forms.DockStyle.Right;
            restart.FlatAppearance.BorderSize = 0;
            restart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            restart.ForeColor = System.Drawing.Color.Transparent;
            restart.Location = new System.Drawing.Point(664, 0);
            restart.Name = "restart";
            restart.Size = new System.Drawing.Size(30, 30);
            restart.TabIndex = 7;
            restart.TextColor = System.Drawing.Color.Transparent;
            restart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            restart.UseVisualStyleBackColor = false;
            restart.Click += restart_Click;
            // 
            // translationButton
            // 
            translationButton.BackColor = System.Drawing.Color.Transparent;
            translationButton.BackgroundColor = System.Drawing.Color.Transparent;
            translationButton.BorderColor = System.Drawing.Color.Transparent;
            translationButton.BorderRadius = 0;
            translationButton.BorderSize = 0;
            translationButton.Dock = System.Windows.Forms.DockStyle.Left;
            translationButton.FlatAppearance.BorderSize = 0;
            translationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            translationButton.ForeColor = System.Drawing.Color.Transparent;
            translationButton.Location = new System.Drawing.Point(166, 0);
            translationButton.Name = "translationButton";
            translationButton.Size = new System.Drawing.Size(30, 30);
            translationButton.TabIndex = 6;
            translationButton.TextColor = System.Drawing.Color.Transparent;
            translationButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            translationButton.UseVisualStyleBackColor = false;
            translationButton.Click += translationButton_Click;
            // 
            // minimize
            // 
            minimize.BackColor = System.Drawing.Color.Transparent;
            minimize.BackgroundColor = System.Drawing.Color.Transparent;
            minimize.BorderColor = System.Drawing.Color.Transparent;
            minimize.BorderRadius = 0;
            minimize.BorderSize = 0;
            minimize.Dock = System.Windows.Forms.DockStyle.Right;
            minimize.FlatAppearance.BorderSize = 0;
            minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            minimize.ForeColor = System.Drawing.Color.Transparent;
            minimize.Location = new System.Drawing.Point(694, 0);
            minimize.Name = "minimize";
            minimize.Size = new System.Drawing.Size(30, 30);
            minimize.TabIndex = 4;
            minimize.TextColor = System.Drawing.Color.Transparent;
            minimize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            minimize.UseVisualStyleBackColor = false;
            minimize.Click += minimize_Click;
            // 
            // maximize
            // 
            maximize.BackColor = System.Drawing.Color.Transparent;
            maximize.BackgroundColor = System.Drawing.Color.Transparent;
            maximize.BorderColor = System.Drawing.Color.Transparent;
            maximize.BorderRadius = 0;
            maximize.BorderSize = 0;
            maximize.Dock = System.Windows.Forms.DockStyle.Right;
            maximize.FlatAppearance.BorderSize = 0;
            maximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            maximize.ForeColor = System.Drawing.Color.Transparent;
            maximize.Location = new System.Drawing.Point(724, 0);
            maximize.Name = "maximize";
            maximize.Size = new System.Drawing.Size(30, 30);
            maximize.TabIndex = 3;
            maximize.TextColor = System.Drawing.Color.Transparent;
            maximize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            maximize.UseVisualStyleBackColor = false;
            maximize.Click += maximize_Click;
            // 
            // exit
            // 
            exit.BackColor = System.Drawing.Color.Transparent;
            exit.BackgroundColor = System.Drawing.Color.Transparent;
            exit.BorderColor = System.Drawing.Color.Transparent;
            exit.BorderRadius = 0;
            exit.BorderSize = 0;
            exit.Dock = System.Windows.Forms.DockStyle.Right;
            exit.FlatAppearance.BorderSize = 0;
            exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            exit.ForeColor = System.Drawing.Color.Transparent;
            exit.Location = new System.Drawing.Point(754, 0);
            exit.Name = "exit";
            exit.Size = new System.Drawing.Size(30, 30);
            exit.TabIndex = 2;
            exit.TextColor = System.Drawing.Color.Transparent;
            exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            exit.UseVisualStyleBackColor = false;
            exit.Click += exit_Click;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Dock = System.Windows.Forms.DockStyle.Left;
            nameLabel.Font = new System.Drawing.Font("Script MT Bold", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            nameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            nameLabel.Location = new System.Drawing.Point(30, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(136, 22);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Launcher - LSAJ";
            nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            nameLabel.DoubleClick += nameLabel_DoubleClick;
            nameLabel.MouseDown += nameLabel_MouseDown;
            nameLabel.MouseMove += nameLabel_MouseMove;
            nameLabel.MouseUp += nameLabel_MouseUp;
            // 
            // ApplicationSound
            // 
            ApplicationSound.BackColor = System.Drawing.Color.Transparent;
            ApplicationSound.BackgroundColor = System.Drawing.Color.Transparent;
            ApplicationSound.BorderColor = System.Drawing.Color.Transparent;
            ApplicationSound.BorderRadius = 0;
            ApplicationSound.BorderSize = 0;
            ApplicationSound.Dock = System.Windows.Forms.DockStyle.Left;
            ApplicationSound.FlatAppearance.BorderSize = 0;
            ApplicationSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ApplicationSound.ForeColor = System.Drawing.Color.Transparent;
            ApplicationSound.Location = new System.Drawing.Point(0, 0);
            ApplicationSound.Name = "ApplicationSound";
            ApplicationSound.Size = new System.Drawing.Size(30, 30);
            ApplicationSound.TabIndex = 0;
            ApplicationSound.TextColor = System.Drawing.Color.Transparent;
            ApplicationSound.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            ApplicationSound.UseVisualStyleBackColor = false;
            ApplicationSound.Click += ApplicationSound_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(FormBorder, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(784, 561);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // Game
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(784, 561);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            KeyPreview = true;
            MinimumSize = new System.Drawing.Size(150, 150);
            Name = "Game";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Launcher VLCM - lsaj.niua.com";
            Load += Game_Load;
            Resize += Game_Resize;
            FormBorder.ResumeLayout(false);
            FormBorder.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
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
        private CustomControls.CButton restart;
        private CustomControls.CButton reset;
    }
}