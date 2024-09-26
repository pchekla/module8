using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Запрашиваем у пользователя путь к папке
        Console.WriteLine("Введите путь к папке для подсчета размера:");
        string? folderPath = Console.ReadLine();

        // Проверяем, не является ли введенное значение null или пустым
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            Console.WriteLine("Ошибка: Путь к папке не был введен.");
            return;
        }

        try
        {
            // Создаем объект DirectoryInfo для указанной папки
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            // Проверяем, существует ли папка
            if (!directoryInfo.Exists)
            {
                Console.WriteLine("Ошибка: Папка не найдена.");
                return;
            }

            // Проверяем, является ли папка пустой
            if (IsDirectoryEmpty(directoryInfo))
            {
                Console.WriteLine("Папка пуста.");
                return;
            }

            // Считаем размер папки
            long folderSize = GetDirectorySize(directoryInfo);

            // Выводим размер папки
            Console.WriteLine($"Размер папки: {folderSize} байт.");
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

        // Подсчет размера вложенных папок
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

    // Метод для проверки, пуста ли папка
    static bool IsDirectoryEmpty(DirectoryInfo directoryInfo)
    {
        try
        {
            // Проверяем, есть ли файлы или папки внутри
            return directoryInfo.GetFiles().Length == 0 && directoryInfo.GetDirectories().Length == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при проверке пустоты папки: {ex.Message}");
            return false;
        }
    }
}