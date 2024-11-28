using System;
using System.Collections.Generic;
using System.Linq;




namespace InspectorLib
{
    public class FunctionInsp
    {
        private List<Worker> workers = new List<Worker>();
        private List<WorkerPosition> positions = new List<WorkerPosition>();
        private Worker mainInspector;
        private string carInspectionName;

        // Инициализация начальных данных через конструктор класса
        public FunctionInsp()
        {
            // Создаём должность с названием "Инспектор", окладом 75 тысяч и ID=2
            AddPosition("Инспектор", 75000.0, 2);

            AddPosition("Главный инспектор", 120000.0, 1);

            carInspectionName = "Автоинспекция г. Чита";


            // Добавляем начальные данные
            // При добавлении сотрудников чепез AddWorker(fullname), возраст = 0, должность выбирается первой из списка должностей positions
            AddWorker("Иванов И. И.");
            AddWorker("Зиронов Т. А.");
            AddWorker("Миронов А. В.");

            // Добавляем Главного инспектора через перегруженный метод, принимающий отдельно ФИО, возраст и ID должности
            AddWorker("Василий", "Васильев", "Иванович", 45, 1);

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
            if (fullname == null) {
                Console.WriteLine("Ошибка! Имя нового инспекотра не содержит значения.");
                return;
            }

            foreach (Worker worker in workers)
            {
                if (worker.getFullName() == fullname)
                {
                    mainInspector = worker;
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

            string[] symbols = { "а", "в", "е", "к", "м", "н", "о", "р", "с", "т", "у", "х"};

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

            if (number < 10)
            {
                result = result + "000" + number.ToString();
            }
            else if (number < 100)
            {
                result = result + "00" + number.ToString();
            }
            else if (number < 1000)
            {
                result = result + "0" + number.ToString();
            }
            else
            {
                result += number.ToString();
            }

            if (code < 10)
            {
                result += "_0" + code.ToString();
            }
            else
            {
                result += "_" + code.ToString();
            }

            

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
        public void AddWorker(string firtsName, string lastName, string patronymic, int age, int positionID)
        {
            if (positionID < 0)
            {
                Console.WriteLine("ID должности не может быть отрицательным!");
                return;
            }

            // для проверки, есть ли должность с заданным ID в списке
            WorkerPosition tempPos = new WorkerPosition(-1, "", 0);
            foreach (WorkerPosition pos in positions)
            {
                if (pos.getId() == positionID)
                {
                    tempPos = pos;
                    break;
                }
            }

            if (tempPos.id == -1)
            {
                Console.WriteLine("Не найдена должность с таким ID!");
                return;
            }

            workers.Add(new Worker(workers.Count, firtsName, lastName, patronymic, age, tempPos));
            Console.WriteLine($"Сотрудник {lastName} {firtsName} {patronymic} успешно добавлен!");
        }



        // Добавляет работника "заглушку"
        public void AddWorker()
        {
            if (positions.Count == 0)
            {
                Console.WriteLine("Невозможно добавить сотрудника! Нету должностей.");
                return;
            }
            workers.Add(new Worker(workers.Count, "Имя", "Фамилия", "Отчество", 0, positions[0]));
            Console.WriteLine($"Сотрудник {workers.Last().getFullName()} успешно добавлен!");
        }


        // Добавляет сотрудника, принимая на вход одну строку
        public void AddWorker(string fullname)
        {
            if (positions.Count == 0)
            {
                Console.WriteLine("Невозможно добавить сотрудника! Нету должностей.");
                return;
            }
            string[] nameParts = fullname.Split(' ');
            if (nameParts.Length != 3) {
                Console.WriteLine("Не удалось прочитать имя");
                return;
            }
            workers.Add(new Worker(workers.Count, nameParts[1], nameParts[0], nameParts[2], 0, positions[0]));
            Console.WriteLine($"Сотрудник {fullname} успешно добавлен!");
        }


        // Добавляет новую должность
        public void AddPosition(string name, double salary, int id)
        {
            if (salary < 0 || id < 0)
            {
                Console.WriteLine("Значения не могут быть отрицательными!");
                return;
            }
            foreach (WorkerPosition pos in positions)
            {
                if (pos.id == id)
                {
                    Console.WriteLine("Должность с таким ID уже существует!");
                    return;
                }
            }

            positions.Add(new WorkerPosition(id, name, salary));
        }
    }



    // Класс должности
    public class WorkerPosition
    {
        public int id;
        public string name;
        public double salary;

        public WorkerPosition(int id, string name, double salary)
        {
            this.id = id;
            this.name = name;
            this.salary = salary;
        }
        public int getId() { return id; }
    }



    // Класс работника
    public class Worker
    {
        int id;
        int age;
        string firstName;
        string lastName;
        string patronymic;

        WorkerPosition position;

        public Worker(int id, string firstName, string lastName, string patronymic, int age, WorkerPosition position)
        {
            this.id = id;
            this.age = age;
            this.firstName = firstName;
            this.lastName = lastName;
            this.patronymic = patronymic;
            this.position = position;
        }


        public void SetAge(int newAge) // Пример публичного метода "сеттера"
        {                            // Лучше использовать приватные переменные, доступ к которым производится с помощью специальных публичных методов.
            if (newAge < 0)           // Так, например, можно добавить проверку на корректность вводимых данных.
            {
                throw new ArgumentOutOfRangeException("Возраст не может быть меньше нуля!");
            }
            age = newAge;
        }


        public string getFullName()
        {
            return $"{lastName} {firstName} {patronymic}";
        }
    }
}
