using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Запрашиваем у пользователя путь к папке для очистки
        Console.WriteLine("Введите путь к папке для очистки:");
        string? inputPath = Console.ReadLine();

        // Проверяем, не является ли введенное значение null или пустым
        if (string.IsNullOrWhiteSpace(inputPath))
        {
            Console.WriteLine("Ошибка: Путь к папке не был введен.");
            return;
        }

        // Преобразуем введенное значение к типу, не допускающему значение null
        string folderPath = inputPath;

        try
        {
            // Создаем объект DirectoryInfo для указанной папки
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            // Проверяем, существует ли папка
            if (!directoryInfo.Exists)
            {
                Console.WriteLine("Ошибка: Папка не существует.");
                return;
            }

            // Определяем временной интервал в 30 минут
            TimeSpan timeLimit = TimeSpan.FromMinutes(30);
            DateTime thresholdTime = DateTime.Now - timeLimit;

            int deletedFilesCount = 0;
            int deletedFoldersCount = 0;

            // Удаляем файлы, которые не использовались более 30 минут
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.LastAccessTime < thresholdTime)
                {
                    try
                    {
                        file.Delete();
                        Console.WriteLine($"Удален файл: {file.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при удалении файла {file.FullName}: {ex.Message}");
                    }
                }
            }

            // Удаляем папки, которые не использовались более 30 минут
            foreach (DirectoryInfo subDir in directoryInfo.GetDirectories())
            {
                if (subDir.LastAccessTime < thresholdTime)
                {
                    try
                    {
                        subDir.Delete(true);
                        Console.WriteLine($"Удалена папка: {subDir.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при удалении папки {subDir.FullName}: {ex.Message}");
                    }
                }
            }

            // Проверяем, если ничего не было удалено
            if (deletedFilesCount == 0)
            {
                Console.WriteLine("Не найдено файлов для удаления.");
            }

            if (deletedFoldersCount == 0)
            {
                Console.WriteLine("Не найдено папок для удаления.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}