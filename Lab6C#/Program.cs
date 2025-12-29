namespace Lab6C_
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DB.Initialize();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainPageForm(1600, 840));
        }
    }
}