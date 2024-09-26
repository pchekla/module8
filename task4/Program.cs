using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class Student
{
    public string? Name { get; set; }
    public string? Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageGrade { get; set; }
}

public class Program
{
    public static void Main()
    {
        // Запрос пути к бинарному файлу у пользователя
        Console.WriteLine("Введите полный путь к бинарному файлу со студентами. Пример: /home/students.dat");
        string? binaryFilePath = Console.ReadLine();

        // Проверка существования файла
        if (!File.Exists(binaryFilePath))
        {
            Console.WriteLine("Файл не найден. Проверьте путь и попробуйте снова.");
            return;
        }

        // Путь к рабочему столу текущего пользователя
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string studentsDirectory = Path.Combine(desktopPath, "Students");

        // Создание директории Students на рабочем столе, если её нет
        if (!Directory.Exists(studentsDirectory))
        {
            Directory.CreateDirectory(studentsDirectory);
        }

        // Чтение студентов из бинарного файла
        List<Student> students = ReadStudentsFromBinaryFile(binaryFilePath);

        // Сортировка студентов по группам и сохранение в текстовые файлы
        SaveStudentsByGroup(students, studentsDirectory);
    }

    // Метод чтения студентов из бинарного файла
    private static List<Student> ReadStudentsFromBinaryFile(string filePath)
    {
        List<Student> students = new List<Student>();

        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                string name = reader.ReadString();
                string group = reader.ReadString();
                long dateOfBirthBinary = reader.ReadInt64();
                DateTime dateOfBirth = DateTime.FromBinary(dateOfBirthBinary);
                decimal averageGrade = reader.ReadDecimal();

                students.Add(new Student
                {
                    Name = name,
                    Group = group,
                    DateOfBirth = dateOfBirth,
                    AverageGrade = averageGrade
                });
            }
        }

        return students;
    }

    // Метод сохранения студентов по группам в текстовые файлы
    private static void SaveStudentsByGroup(List<Student> students, string directory)
    {
        // Группировка студентов по группам
        var groupedStudents = new Dictionary<string, List<Student>>();

        foreach (var student in students)
        {
            if (!groupedStudents.ContainsKey(student.Group))
            {
                groupedStudents[student.Group] = new List<Student>();
            }

            groupedStudents[student.Group].Add(student);
        }

        // Сохранение студентов в соответствующие файлы групп
        foreach (var group in groupedStudents)
        {
            string groupFilePath = Path.Combine(directory, $"{group.Key}.txt");

            using (StreamWriter writer = new StreamWriter(groupFilePath))
            {
                foreach (var student in group.Value)
                {
                    string studentLine = $"{student.Name}, {student.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}, {student.AverageGrade:F2}";
                    writer.WriteLine(studentLine);
                }
            }
        }

        Console.WriteLine($"Данные успешно сохранены в директории: {directory}");
    }
}