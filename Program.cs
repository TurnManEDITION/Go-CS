using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    static async Task Main(string[] args)
    {
        Process process = new Process(); // Содание процесса
        process.StartInfo.FileName = "cmd.exe"; // Указываем командную строку
        process.StartInfo.Arguments = "/C go mod init main & go build -ldflags \"-H windowsgui\" -o main.exe & main.exe"; // Команда для выполнения
        process.StartInfo.UseShellExecute = false; // Отключаем использование оболочки
        process.StartInfo.CreateNoWindow = true; // Не показываем окно консоли
        process.Start(); // Запускаем процесс

        using HttpClient client = new HttpClient(); // Переменная для запроса
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:8080"); // Создание локального хоста
            response.EnsureSuccessStatusCode(); // Проверка успешного ответа
            string responseData = await response.Content.ReadAsStringAsync(); // Получаем данные
            Console.WriteLine(responseData); // Выводим данные в консоль
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // При возникновении ошибки
        }

        if (!process.HasExited) // Проверка на работу процессора
        {
            [DllImport("kernel32.dll")]
            static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);
            const uint PROCESS_TERMINATE = 0x0001;
            int processId = process.Id;
            IntPtr hProcess = OpenProcess(PROCESS_TERMINATE, false, processId);
            if (hProcess != IntPtr.Zero)
            {
                TerminateProcess(hProcess, 0);
                Console.WriteLine($"Process ID: {process.Id}, Name: {process.ProcessName}");
                Console.WriteLine("Процесс принудительно завершён.");
            }
            else
            {
                Console.WriteLine("Не удалось открыть процесс.");
            }
        }
        else
        {
            Console.WriteLine("Процесс уже завершен.");
        }
        string name = "main";//процесс, который нужно убить
        Process[] etc = Process.GetProcesses();//получим процессы
        foreach (Process anti in etc)//обойдем каждый процесс
            if (anti.ProcessName.ToLower().Contains(name.ToLower())) anti.Kill();//найдем нужный и убьем
        Console.WriteLine("Press enter");
        Console.ReadLine();
    }
}
