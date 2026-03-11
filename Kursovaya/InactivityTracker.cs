using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Smirnov_kursovaya.mainForm;

namespace Smirnov_kursovaya.Helpers
{
    public static class InactivityTracker
    {
        private static Timer inactivityTimer;
        private static int timeoutSeconds;

        public static void Start()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["InactivityTimeoutSeconds"], out timeoutSeconds))
                timeoutSeconds = 30;

            inactivityTimer = new Timer();
            inactivityTimer.Interval = timeoutSeconds * 1000;
            inactivityTimer.Tick += OnInactivityTimeout;
            inactivityTimer.Start();

            Application.AddMessageFilter(new ActivityMessageFilter());
        }

        public static void ResetTimer()
        {
            inactivityTimer?.Stop();
            inactivityTimer?.Start();
        }

        private static void OnInactivityTimeout(object sender, EventArgs e)
        {
            inactivityTimer?.Stop();

            // Проверяем, активен ли цикл сообщений (приложение не завершается)
            if (!Application.MessageLoop)
                return;

            // Копируем список открытых форм
            var forms = Application.OpenForms.Cast<Form>().ToArray();

            // Ищем форму авторизации
            Authentication authForm = forms.FirstOrDefault(f => f is Authentication) as Authentication;

            // Закрываем все формы, кроме авторизации
            foreach (Form form in forms)
            {
                if (form != authForm && !form.IsDisposed)
                    form.Close();
            }

            // Если форма авторизации существует и не уничтожена, очищаем поля и показываем
            if (authForm != null && !authForm.IsDisposed)
            {
                authForm.ClearInputFields(); // метод, который мы добавили в Authentication
                authForm.Show();
                authForm.Activate();
            }
            else
            {
                // Создаём новую форму авторизации (если приложение ещё работает)
                authForm = new Authentication();
                authForm.Show();
            }

            ResetTimer();
        }

        private class ActivityMessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg >= 0x200 && m.Msg <= 0x20A) || m.Msg == 0x100 || m.Msg == 0x101)
                {
                    ResetTimer();
                }
                return false;
            }
        }
    }
}