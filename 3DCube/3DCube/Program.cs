using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BeagleboneSensors
{
    static class Program
    {
		public static RabbitMqReceiver receiver;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			receiver = new RabbitMqReceiver();
			receiver.CreateConnection();

			Form form = new FrmRender();
			form.FormClosing += (sender, args) =>
				{
					receiver.Shutdown();
				};

            Application.Run(form);
        }
    }
}