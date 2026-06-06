namespace inventory_of_equipment_in_classrooms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var context = new ApplicationContext();

            var loginForm = new LoginForm();
            loginForm.FormClosed += (s, e) =>
            {
                if (Application.OpenForms.Count == 0)
                    context.ExitThread();
            };
            loginForm.Show();

            Application.Run(context);
        }
    }
}