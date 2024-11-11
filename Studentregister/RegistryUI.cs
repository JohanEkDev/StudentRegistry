using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;

namespace StudentRegistry
{
    internal class RegistryUI
    {
        public void MainMenu(Registry registry, RegistryDbContext context)
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO THE .NET STUDENT REGISTRY.");
            Console.WriteLine("ENTER A NUMBER TO SELECT DESIRED ACTION.");
            Console.WriteLine();
            Console.WriteLine("1.DISPLAY FULL REGISTRY\n2.REGISTER NEW STUDENT\n3.UPDATE EXISTING STUDENT\n4.EXIT REGISTRY");
            try
            {
                int inputMenu = Convert.ToInt32(Console.ReadLine());
                this.MainMenuInput(registry, context, inputMenu);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("INVALID ENTRY. ONLY NUMBERS ACCEPTABLE AS INPUT.");
                this.PressKeyMenu();
                this.MainMenu(registry, context);
            }
        }
        public void MainMenuInput(Registry registry, RegistryDbContext context, int inputMenu)
        {
            switch (inputMenu)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("LIST OF CURRENT STUDENTS IN REGISTRY:");
                    Console.WriteLine();
                    this.PrintStudentsInRegistry(context);
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("TO CREATE A NEW STUDENT ENTRY,\nSUBMIT INFORMATION AS REQUESTED.");
                    Console.WriteLine();
                    Student tempStudent = CreateTemporaryStudent();
                    registry.AddNewStudent(context, tempStudent);
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                case 3:
                    this.StudentSubmenu(registry, context);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("THANK YOUR FOR USING .NET REGISTRY.");
                    Console.WriteLine("PRESSING ANY KEY WILL END THE PROGRAM...");
                    context.SaveChanges();
                    Console.ReadKey();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("INVALID ENTRY. ONLY NUMBERS FROM MENU ACCEPTED AS INPUT.");
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
            }
        }
        public void StudentSubmenu(Registry registry, RegistryDbContext context)
        {
            Console.Clear();
            Console.WriteLine("THERE ARE THREE OPTIONS TO FIND TARGET STUDENT.");
            Console.WriteLine("ENTER A NUMBER TO CHOOSE YOUR DESIRED METHOD.");
            Console.WriteLine();
            Console.WriteLine("1.SELECT BY ID\n2.SEARCH BY NAME\n3.SEARCH BY CITY\n4.PREVIOUS MENU");
            try
            {
                int inputStudentSubmenu = Convert.ToInt32(Console.ReadLine());
                this.StudentSubmenuInput(registry, context, inputStudentSubmenu);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("INVALID ENTRY. ONLY NUMBERS ACCEPTABLE AS INPUT.");
                this.PressKeyMenu();
                this.MainMenu(registry, context);
            }
        }
        public void StudentSubmenuInput(Registry registry, RegistryDbContext context, int inputStudentSubmenu)
        {
            switch (inputStudentSubmenu)
            {
                case 1:
                    Console.Clear();
                    this.InputStudentUpdates(registry, context);
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                case 2:
                    Console.Clear();
                    this.EditByStudentName(registry, context);
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                case 3:
                    Console.Clear();
                    this.EditByStudentCity(registry, context);
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                case 4:

                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("INVALID ENTRY. ONLY NUMBERS FROM MENU ACCEPTED AS INPUT.");
                    this.PressKeyMenu();
                    this.MainMenu(registry, context);
                    break;

            }
        }
        public void EditByStudentCity(Registry registry, RegistryDbContext context)
        {
            Console.WriteLine("ENTER NAME OF TARGET STUDENT TO UPDATE:");
            string inputTargetStudentCity = Console.ReadLine();
            Console.Clear();
            List<Student> targetStudents = registry.FindTargetStudentCity(context, inputTargetStudentCity);
            if (targetStudents.Count() < 1)
            {
                Console.WriteLine("TARGET STUDENT NOT FOUND IN REGISTRY.");
            }
            else
            {
                Console.WriteLine($"TARGET MATCHING STUDENT(S) FOUND IN REGISTRY:");
                Console.WriteLine();
                this.PrintTargetStudents(targetStudents);
                Console.WriteLine();
                this.InputStudentUpdates(registry, context);
            }
        }
        public void EditByStudentName(Registry registry, RegistryDbContext context)
        {
            Console.WriteLine("ENTER NAME OF TARGET STUDENT TO UPDATE:");
            string inputTargetStudentName = Console.ReadLine();
            Console.Clear();
            List<Student> targetStudents = registry.FindTargetStudentName(context, inputTargetStudentName);
            if (targetStudents.Count() < 1)
            {
                Console.WriteLine("TARGET STUDENT NOT FOUND IN REGISTRY.");
            }
            else
            {
                Console.WriteLine($"TARGET MATCHING STUDENT(S) FOUND IN REGISTRY:");
                Console.WriteLine();
                this.PrintTargetStudents(targetStudents);
                Console.WriteLine();
                this.InputStudentUpdates(registry, context);
            }
        }
        public void InputStudentUpdates(Registry registry, RegistryDbContext context)
        {
            try
            {
                Console.WriteLine("ENTER ID OF TARGET STUDENT TO UPDATE:");
                int inputTargetStudentId = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                int targetStudentId = registry.FindTargetStudentId(context, inputTargetStudentId);
                if (targetStudentId == 0)
                {
                    Console.WriteLine($"TARGET STUDENT WITH ID {inputTargetStudentId} NOT FOUND IN REGISTRY.");
                }
                else
                {
                    Console.WriteLine($"TARGET STUDENT WITH ID {inputTargetStudentId} FOUND IN REGISTRY.");
                    Console.WriteLine("SUBMIT NEW INFORMATION AS REQUESTED.");
                    Console.WriteLine();
                    Console.WriteLine($"CURRENT - FIRST NAME:  {context.Students.FirstOrDefault<Student>
                (s => s.StudentId == targetStudentId).FirstName}  ");
                    Console.WriteLine($"CURRENT - LAST NAME:  {context.Students.FirstOrDefault<Student>
                (s => s.StudentId == targetStudentId).LastName}  ");
                    Console.WriteLine($"CURRENT - CITY:  {context.Students.FirstOrDefault<Student>
                (s => s.StudentId == targetStudentId).City}");
                    Console.WriteLine();
                    Student tempStudent = CreateTemporaryStudent();
                    registry.UpdateExistingStudent(context, targetStudentId, tempStudent);
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("INVALID ENTRY. ONLY NUMBERS ACCEPTABLE AS INPUT.");
            }
        }
        public Student CreateTemporaryStudent()
        {
            Console.WriteLine("FIRST NAME OF STUDENT:");
            string inputFirstName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("LAST NAME OF STUDENT:");
            string inputLastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("CITY OF STUDENT:");
            string inputCity = Console.ReadLine();
            Student tempStudent = new Student(inputFirstName, inputLastName, inputCity);
            return tempStudent;
        }
        public void PrintStudentsInRegistry(RegistryDbContext context)
        {
            foreach (Student entry in context.Students)
            {
                Console.WriteLine($"ID:  {entry.StudentId}  FIRST NAME:  {entry.FirstName}  " +
                    $"LAST NAME:  {entry.LastName}  CITY:  {entry.City}");
            }
        }
        public void PrintTargetStudents(List<Student> targetStudents)
        {
            foreach (Student entry in targetStudents)
            {
                Console.WriteLine($"ID:  {entry.StudentId}  FIRST NAME:  {entry.FirstName}" +
                    $"  LAST NAME:  {entry.LastName}  CITY:  {entry.City}");
            }
        }
        public void PressKeyMenu()
        {
            Console.WriteLine();
            Console.WriteLine("PRESSING ANY KEY WILL RETURN YOU TO THE MENU.");
            Console.ReadKey();
        }

    }
}
