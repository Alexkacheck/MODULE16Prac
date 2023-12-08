using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
namespace MODULE16Prac
{
   

    public class FileChangeLogger
    {
        private string directoryPath;
        private string logFilePath;

        public FileChangeLogger(string directoryPath, string logFilePath)
        {
            this.directoryPath = directoryPath;
            this.logFilePath = logFilePath;
        }

        public void StartLogging()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"Отслеживание начато: {DateTime.Now}");
                }

                using (FileSystemWatcher watcher = new FileSystemWatcher(directoryPath))
                {
                    watcher.IncludeSubdirectories = true;
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;
                    watcher.Changed += OnChanged;

                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine($"Отслеживание директории {directoryPath} запущено.");
                    Console.WriteLine($"Лог-файл: {logFilePath}");
                    Console.WriteLine("Нажмите любую клавишу для завершения...");

                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            LogChange($"{e.ChangeType} - {e.FullPath}");
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            LogChange($"Renamed - {e.OldFullPath} to {e.FullPath}");
        }

        private void LogChange(string logEntry)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - {logEntry}");
                }

                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в лог-файл: {ex.Message}");
            }
        }
    }

    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Приложение для логирования изменений в файлах");
            Console.WriteLine("--------------------------------------------");

            Console.Write("Введите путь к отслеживаемой директории: ");
            string directoryPath = Console.ReadLine();

            Console.Write("Введите путь к лог-файлу: ");
            string logFilePath = Console.ReadLine();

            FileChangeLogger fileChangeLogger = new FileChangeLogger(directoryPath, logFilePath);
            fileChangeLogger.StartLogging();
        }
    }

}
