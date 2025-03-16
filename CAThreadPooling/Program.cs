

namespace CAThreadPool
{

    // Thread Pool 
    // creating thread make overhead in time and memory 
    //pool of pre-created recyclable :Hepls mitigate the issue of performance by reducing the number of threads
    //with thread pool (you cannot name a thread - thread pool always background- Ideal for short running process)

    internal class Program
    {
        static void Main(string[] args)
        {
            //1
            Console.WriteLine("Using Thread Pool ");
            ThreadPool.QueueUserWorkItem(new WaitCallback(Print));// take delegate 

            //2 
            Console.WriteLine("Using Task ");
            Task.Run(Print);


             var employee =new Employee() { Rate=10,TotalHour=40};
            ThreadPool.QueueUserWorkItem(new WaitCallback(CalcolateSalary),employee);



            Console.ReadKey();
        }

        private static void Print()
        {
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}\nThread Name: {Thread.CurrentThread.Name}\n" +
                          $"Is Pooled Thread: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Cycle {i + 1}");
            }

        }

        private static void CalcolateSalary(object? employee)
        {
            var emp=employee as Employee;
            if (emp != null)
            {
                emp.TotalSalary=emp.TotalHour*emp.Rate;
                Console.WriteLine(emp.TotalSalary.ToString("c"));
            }
        }

        private static void Print(object? state)
        {

            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}\nThread Name: {Thread.CurrentThread.Name}\n" +
                $"Is Pooled Thread: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Cycle {i+1}");
            }


        }
    }



    class Employee
    {
        public decimal TotalHour{ get; set; }
        public decimal Rate { get; set; }
        public decimal TotalSalary { get; set; }

    }

}
