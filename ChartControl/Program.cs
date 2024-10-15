namespace Charts
{
    internal static class Program
    {
        /// <summary>
        /// The main entry PointFor the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}