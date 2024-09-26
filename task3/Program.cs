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

            // Вычисляем исходный размер папки
            long initialSize = GetDirectorySize(directoryInfo);
            Console.WriteLine($"Исходный размер папки: {initialSize} байт");

            // Определяем временной интервал в 30 минут
            TimeSpan timeLimit = TimeSpan.FromMinutes(30);
            DateTime thresholdTime = DateTime.Now - timeLimit;

            // Инициализируем счетчики для подсчета удаленных файлов и освобожденного места
            int deletedFilesCount = 0;
            long freedSpace = 0;

            // Удаляем файлы, которые не использовались более 30 минут
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.LastAccessTime < thresholdTime)
                {
                    try
                    {
                        long fileSize = file.Length;
                        file.Delete();
                        deletedFilesCount++;
                        freedSpace += fileSize;
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
                        long dirSize = GetDirectorySize(subDir);
                        subDir.Delete(true);
                        deletedFilesCount++;
                        freedSpace += dirSize;
                        Console.WriteLine($"Удалена папка: {subDir.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при удалении папки {subDir.FullName}: {ex.Message}");
                    }
                }
            }

            // Проверка, если не было удалено ни файлов, ни папок
            if (deletedFilesCount == 0)
            {
                Console.WriteLine("Не найдено файлов и папок для удаления.");
            }

            // Вычисляем текущий размер папки после очистки
            long currentSize = GetDirectorySize(directoryInfo);

            // Выводим информацию о размере папки и освобожденном месте
            Console.WriteLine($"Освобождено: {freedSpace} байт");
            Console.WriteLine($"Текущий размер папки: {currentSize} байт");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    // Метод для подсчета размера папки
    static long GetDirectorySize(DirectoryInfo directoryInfo)
    {
        long size = 0;

        // Подсчет размера всех файлов в папке
        try
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                size += file.Length;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при подсчете размера файлов: {ex.Message}");
        }

        // Рекурсивный подсчет размера вложенных папок
        try
        {
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                size += GetDirectorySize(dir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при подсчете размера вложенных папок: {ex.Message}");
        }

        return size;
    }
}