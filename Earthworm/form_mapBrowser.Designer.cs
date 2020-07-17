namespace Earthworm
{
    partial class form_mapBrowser
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
            this.viewport = new GMap.NET.WindowsForms.GMapControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.shapefileChecklist = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // viewport
            // 
            this.viewport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewport.AutoScroll = true;
            this.viewport.AutoSize = true;
            this.viewport.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.viewport.Bearing = 0F;
            this.viewport.CanDragMap = true;
            this.viewport.EmptyTileColor = System.Drawing.Color.Navy;
            this.viewport.GrayScaleMode = true;
            this.viewport.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.viewport.ImeMode = System.Windows.Forms.ImeMode.On;
            this.viewport.LevelsKeepInMemmory = 5;
            this.viewport.Location = new System.Drawing.Point(12, 12);
            this.viewport.MarkersEnabled = true;
            this.viewport.MaxZoom = 2;
            this.viewport.MinZoom = 2;
            this.viewport.MouseWheelZoomEnabled = true;
            this.viewport.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.viewport.Name = "viewport";
            this.viewport.NegativeMode = false;
            this.viewport.PolygonsEnabled = true;
            this.viewport.RetryLoadTile = 0;
            this.viewport.RoutesEnabled = true;
            this.viewport.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.viewport.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.viewport.ShowTileGridLines = false;
            this.viewport.Size = new System.Drawing.Size(896, 654);
            this.viewport.TabIndex = 0;
            this.viewport.Zoom = 2D;
            this.viewport.Load += new System.EventHandler(this.gMapControl1_Load);
            this.viewport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gmap_MouseClick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Location = new System.Drawing.Point(914, 479);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(371, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "Reset Crop Boundary";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Location = new System.Drawing.Point(914, 536);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(371, 130);
            this.button2.TabIndex = 2;
            this.button2.Text = "Crop Shapefiles";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.shapefileChecklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shapefileChecklist.CheckBoxes = true;
            this.shapefileChecklist.HideSelection = false;
            this.shapefileChecklist.LabelWrap = false;
            this.shapefileChecklist.Location = new System.Drawing.Point(914, 12);
            this.shapefileChecklist.Name = "listView1";
            this.shapefileChecklist.Size = new System.Drawing.Size(371, 457);
            this.shapefileChecklist.TabIndex = 4;
            this.shapefileChecklist.UseCompatibleStateImageBehavior = false;
            this.shapefileChecklist.View = System.Windows.Forms.View.List;
            // 
            // form_mapBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1297, 678);
            this.Controls.Add(this.shapefileChecklist);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.viewport);
            this.DoubleBuffered = true;
            this.Name = "form_mapBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Earthworm Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public GMap.NET.WindowsForms.GMapControl viewport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView shapefileChecklist;
    }
}