// Code modified from: http://www.vcskicks.com/3d-graphics-improved.php

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using Clifton.Core.ExtensionMethods;

namespace BeagleboneSensors
{
    public partial class FrmRender : Form
    {
        public FrmRender()
        {
            InitializeComponent();
			Program.receiver.GyroData += OnGyroData;
        }

		private float dx = 0;
		private float dy = 0;
		private float dz = 0;

		private void OnGyroData(object sender, GyroEventArgs e)
		{
			this.BeginInvoke(() =>
				{
					dx += (float)(e.GyroData.X / 4000.0);
					dy += (float)(e.GyroData.Y / 4000.0);
					dz += (float)(e.GyroData.Z / 4000.0);

					mainCube.RotateX = dx;
					mainCube.RotateY = dy;
					mainCube.RotateZ = dz;
					Render();
				});
		}

        Math3D.Cube mainCube;
        Point drawOrigin;

        private void FrmRender_Load(object sender, EventArgs e)
        {
            mainCube = new Math3D.Cube(200, 300, 175);
            drawOrigin = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
			mainCube.DrawWires = true;
			mainCube.FillFront = true;
			mainCube.FillBack = false;
			mainCube.FillLeft = true;
			mainCube.FillRight = true;
			mainCube.FillTop = true;
			mainCube.FillBottom = true;
        }

        private void Render()
        {
            // mainCube.RotateX = (float)tX.Value;
            // mainCube.RotateY = (float)tY.Value;
            // mainCube.RotateZ = (float)tZ.Value;

            pictureBox1.Image = mainCube.DrawCube(drawOrigin);
        }

        private void tX_Scroll(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void tY_Scroll(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void tZ_Scroll(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            mainCube = new Math3D.Cube(100, 200, 75); //Start over
            this.Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vckicks.110mb.com/");
        }
    }
}