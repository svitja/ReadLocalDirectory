using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ReadLocalDirectory
{
    class Program
    {
        static void Main()
        {
            string path1 = ConfigurationManager.AppSettings.Get("Folder1Path");
            if (!string.IsNullOrEmpty(path1))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(path1, "*.*");
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite;
                watcher.Created += OnCreated;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Вiдстеження нових файлiв у: {path1}");
            }
            string path2 = ConfigurationManager.AppSettings.Get("Folder2Path");
            if (!string.IsNullOrEmpty(path2))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(path2, "*.*");
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite;
                watcher.Created += OnCreated;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Вiдстеження нових файлiв у: {path2}");
            }
            string path3 = ConfigurationManager.AppSettings.Get("Folder3Path");
            if (!string.IsNullOrEmpty(path3))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(path3, "*.*");
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite;
                watcher.Created += OnCreated;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Вiдстеження нових файлiв у: {path3}");
            }

            Console.WriteLine("Натиснiть Enter для виходу...");
            Console.ReadLine();
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Новий файл знайдено: {e.FullPath}");
            WaitForFileReady(e.FullPath);
            Console.WriteLine($"Файл готовий до читання: {e.FullPath}");
            // Тут можна обробляти файл далі (читати, переміщати тощо)
        }

        private static void WaitForFileReady(string filePath)
        {
            const int maxRetries = 20; // максимум спроб
            const int delayMs = 500;   // пауза між спробами
            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    // Спроба відкрити файл для читання
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (stream.Length > 0)
                            return; // файл успішно відкрився — готовий
                    }
                }
                catch (IOException)
                {
                    // файл ще недоступний (зайнятий іншим процесом)
                }

                Thread.Sleep(delayMs);
                retries++;
            }

            Console.WriteLine($"⚠️ Не вдалося отримати доступ до файлу: {filePath}");
        }
    }
}
