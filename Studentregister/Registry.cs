using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistry
{
    internal class Registry
    {
        public void StartRegistry()
        {
            RegistryDbContext context = new RegistryDbContext();
            RegistryUI UI = new RegistryUI();
            if (context.Students.Count() < 2)
            {
                this.GenerateStudentData(context);
            }
            UI.MainMenu(this, context);
        }
        public void GenerateStudentData(RegistryDbContext context)
        {
            context.Add(new Student("Erik", "Eriksson", "Eriksberg"));
            context.Add(new Student("Erik", "Hansson", "Halmstad"));
            context.Add(new Student("Erik", "Andersson", "Arboga"));
            context.Add(new Student("Hanna", "Eriksson", "Eriksberg"));
            context.Add(new Student("Hanna", "Hansson", "Halmstad"));
            context.Add(new Student("Hanna", "Andersson", "Arboga"));
            context.Add(new Student("Anders", "Eriksson", "Eriksberg"));
            context.Add(new Student("Anders", "Hansson", "Halmstad"));
            context.Add(new Student("Anders", "Andersson", "Arboga"));
            context.SaveChanges();
        }
        public void AddNewStudent(RegistryDbContext context, Student tempStudent)
        {
            Student student = tempStudent;
            context.Add(student);
            context.SaveChanges();
        }
        public void UpdateExistingStudent(RegistryDbContext context, int targetStudentId, Student tempStudent)
        {
            Student targetStudent = context.Students.FirstOrDefault<Student>
                (s => s.StudentId == targetStudentId);
            targetStudent.FirstName = tempStudent.FirstName;
            targetStudent.LastName = tempStudent.LastName;
            targetStudent.City = tempStudent.City;
            context.SaveChanges();
        }
        public int FindTargetStudentId(RegistryDbContext context, int inputTargetStudentId)
        {
            int targetStudent = 0;
            foreach (Student entry in context.Students)
            {
                if (entry.StudentId == inputTargetStudentId)
                {
                    targetStudent = inputTargetStudentId;
                    return targetStudent;
                }
            }
            return 0;
        }
        public List<Student> FindTargetStudentName(RegistryDbContext context, string inputTargetStudentName)
        {
            List<Student> targetStudents = context.Students.Where
                (s => (s.FirstName == inputTargetStudentName) || (s.LastName == inputTargetStudentName)).ToList();
            return targetStudents;
        }
        public List<Student> FindTargetStudentCity(RegistryDbContext context, string inputTargetStudentCity)
        {
            List<Student> targetStudents = context.Students.Where
                (s => s.City == inputTargetStudentCity).ToList();
            return targetStudents;
        }
    }
}
