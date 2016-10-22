// Created by Laxale 21.10.2016
//
//


namespace Base.Tests.DefaultImpl 
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Freengy.Base.DefaultImpl;

    using NUnit.Framework;


    [TestFixture]
    public class TaskWrapperTests 
    {
        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Task worked, continuator also worked fine")]
        public void Wrap_GoodCase() 
        {
            // Arrange
            bool taskWorked = false;
            bool continuatorWorked = false;

            var wrapper = new TaskWrapper();

            Action task = 
                () =>
                {
                    taskWorked = true;
                };

            Action<Task> continuatorTask = 
                parentTask =>
                {
                    continuatorWorked = true;
                };

            // Act
            wrapper.Wrap(task, continuatorTask);

            Thread.Sleep(500);

            // Assert
            // таск выполнился, континуатор Тоже
            Assert.IsTrue(taskWorked && continuatorWorked);
        }

        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Task failed, continuator found exception")]
        public void Wrap_BadCase() 
        {
            // Arrange
            bool taskWorked = false;
            bool continuatorFoundException = false;

            var wrapper = new TaskWrapper();

            Action task =
                () =>
                {
                    throw new ArgumentException();
                };

            Action<Task> continuatorTask =
                parentTask =>
                {
                    if (parentTask.Exception != null)
                    {
                        continuatorFoundException = true;
                    }
                };

            // Act
            wrapper.Wrap(task, continuatorTask);

            Thread.Sleep(500);

            // Assert
            // таск или выполнился или упал, а континуатор или отработал или получил исключение начального таска
            Assert.IsTrue(!taskWorked && continuatorFoundException);
        }
    }
}