using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name, bool isWeighted) : base(name, isWeighted)
        {
            Type = Enums.GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
            {
                throw new InvalidOperationException("Ranked grade required a minimum of 5 students to work");
            }

            var sortedGrades = new List<double>();

            foreach (var student in Students)
            {
                sortedGrades.Add(student.AverageGrade);
            }

            sortedGrades.Sort();
            var maxNumberOfStudentIndex = sortedGrades.Count();
            var percentThreshold = Math.Round(0.2 * maxNumberOfStudentIndex);
            IDictionary<int, char> letterGradeTranslation = new Dictionary<int, char>();
            letterGradeTranslation.Add(1, 'A');
            letterGradeTranslation.Add(2, 'B');
            letterGradeTranslation.Add(3, 'C');
            letterGradeTranslation.Add(4, 'D');

            //Say the sorted list consists of - [40 50 60 70 80 85 90 91 91]
            //If we get an input of 65, where would that fall?
            //Total number of students = 9, 20% = 1.8 student
            //Round to the nearest integer? 1.8 = 2 students
            //The input less than list[N-2] = drop to B
            //The input less than list[N-4] = drop to C
            //The input less than list[N-6] = drop to D
            //The input less than list[N-8] = drop to F
            //The input is top 20%          = give the man an A!!

            for (var i = 1; i <= 4; i++)
            {
                if (averageGrade >= sortedGrades[maxNumberOfStudentIndex - (int)percentThreshold * i])
                {
                    return letterGradeTranslation[i];
                }
            }
            return 'F';
        }

        public override void CalculateStatistics()
        {
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }
            base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
           if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }
            base.CalculateStudentStatistics(name);
        }
    }
}
