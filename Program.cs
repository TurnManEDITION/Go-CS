using System.Diagnostics;

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
            process.Kill(); // Завершаем процесс
            Console.WriteLine("Процесс завершен."); // Выводим то, что процесс завершён
        }
        else
        {
            Console.WriteLine("Процесс уже завершен."); // Выводим то, что процесс уже был завершён
        }
        Console.WriteLine("Press enter");
        Console.ReadLine();
    }
}
