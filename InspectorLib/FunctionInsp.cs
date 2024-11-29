using System;
using System.Collections.Generic;
using System.Linq;




namespace InspectorLib
{
    public class FunctionInsp
    {
        private List<Worker> workers = new List<Worker>();
        private Worker mainInspector;
        private string carInspectionName;

        // Инициализация начальных данных через конструктор класса
        public FunctionInsp()
        {
            carInspectionName = "Автоинспекция г. Чита";

            // Добавляем начальные данные
            // При добавлении сотрудников чепез AddWorker(fullname), возраст = 0, должность выбирается первой из списка должностей positions
            AddWorker("Иванов И. И.");
            AddWorker("Зиронов Т. А.");
            AddWorker("Миронов А. В.");

            // Добавляем Главного инспектора через перегруженный метод, принимающий отдельно ФИО, возраст и ID должности
            AddWorker("Василий", "Васильев", "Иванович");

            // Устанавливаем главного инспетора в первый раз
            SetInspector("Васильев Василий Иванович");
        }


        // Возвращает ФИО главного инспектора
        public string GetInspector()
        {
            if (mainInspector == null)
            {
                throw new Exception("Ошибка! Отсутствует значение для переменной главного инспектора.");
            }
            return mainInspector.getFullName();
        }


        // Возвращает название автоинспекции
        public string GetCarInspection()
        {
            if (carInspectionName == null)
            {
                throw new Exception("Ошибка! Отсутствует значение для переменной названия автоинспекции.");
            }
            return carInspectionName;
        }



        // Устанавливает нового главного инспектора
        public void SetInspector(string fullname)
        {
            if (fullname == null)
            {
                Console.WriteLine("Ошибка! Имя нового инспекотра не содержит значения.");
                return;
            }

            foreach (Worker worker in workers)
            {
                if (worker.getFullName() == fullname)
                {
                    mainInspector = worker;
                    Console.WriteLine("Установлен новый главный инспектор!");
                    return;
                }
            }
            Console.WriteLine("Сотрудник с таким именем не найден!");
        }



        // Перегрузка метода генерации, если нет параметров, генерирует случайный номер с кодом региона 75,
        // иначе генерирует случайный номер с полученным параметром в качестве кода региона.
        public string GenerateNumber(int code = 75)
        {
            var rand = new Random();

            string[] symbols = { "а", "в", "е", "к", "м", "н", "о", "р", "с", "т", "у", "х" };

            return GenerateNumber(rand.Next(1, 9999), symbols[rand.Next(0, symbols.Length)], code);
        }



        // Проверяет входные данные на корректность и генерирует полицейский гос номер
        public string GenerateNumber(int number, string symbol = "", int code = 75)
        {
            string result;
            string[] allowedSymbols = { "а", "в", "е", "к", "м", "н", "о", "р", "с", "т", "у", "х" };


            if (number < 0)
            {
                throw new Exception("Номер должен быть больше нуля!");
            }
            if (code < 0)
            {
                throw new Exception("Код региона должен быть больше нуля!");
            }
            if (number > 9999)
            {
                throw new Exception("Слишком большой номер!");
            }
            if (code > 99)
            {
                throw new Exception("Слишком большой код региона!");
            }
            if (!allowedSymbols.Contains(symbol.ToLower()))
            {
                throw new Exception("Неверно введён символ!");
            }

            result = symbol.ToUpper();

            result += number.ToString("0000");

            result += code.ToString("_00");

            return result;
        }



        // Возвращает массив, состоящий из ФИО работников
        public string[] GetWorker()
        {
            string[] result = new string[workers.Count];
            for (int i = 0; i < workers.Count; i++)
            {
                result[i] = workers[i].getFullName();
            }
            return result;
        }



        // Добавляет работника с указанными данными
        public void AddWorker(string firtsName, string lastName, string patronymic)
        {
            workers.Add(new Worker(workers.Count, firtsName, lastName, patronymic));
            Console.WriteLine($"Сотрудник {lastName} {firtsName} {patronymic} успешно добавлен!");
        }



        // Добавляет работника "заглушку"
        public void AddWorker()
        {
            workers.Add(new Worker(workers.Count, "Имя", "Фамилия", "Отчество"));
            Console.WriteLine($"Сотрудник {workers.Last().getFullName()} успешно добавлен!");
        }



        // Добавляет сотрудника, принимая на вход одну строку
        public void AddWorker(string fullname)
        {
            string[] nameParts = fullname.Split(' ');
            if (nameParts.Length != 3)
            {
                Console.WriteLine("Не удалось прочитать имя");
                return;
            }
            workers.Add(new Worker(workers.Count, nameParts[1], nameParts[0], nameParts[2]));
            Console.WriteLine($"Сотрудник {fullname} успешно добавлен!");
        }



        // Класс работника
        public class Worker
        {
            int id;
            string firstName;
            string lastName;
            string patronymic;

            public Worker(int id, string firstName, string lastName, string patronymic)
            {
                this.id = id;
                this.firstName = firstName;
                this.lastName = lastName;
                this.patronymic = patronymic;
            }

            public string getFullName()
            {
                return $"{lastName} {firstName} {patronymic}";
            }
        }
    }
}
