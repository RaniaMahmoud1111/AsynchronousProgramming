namespace CAMultiThreading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Thread";// naming the othread for debuging and tracing  ddbug=> windos

            Console.WriteLine(Thread.CurrentThread.Name);//main thread
                                                         //

            Console.WriteLine($"Background Thread: {Thread.CurrentThread.IsBackground}");

            var wallet = new Wallet("Rania", 80);
            Thread t1 = new Thread(wallet.RunRandomTransactions);
            t1.Name = "T1";

            Console.WriteLine($"T1 Background Thread: {t1.IsBackground}");
            // by defualt thread is foreground (make app alive running )
            Console.WriteLine($"after declaration {t1.Name} state is : {t1.ThreadState} ");


            t1.Start();//t1 run
            Console.WriteLine($"after start {t1.Name} state is : {t1.ThreadState} ");

            t1.Join();//wait t1 to terminate then start in t2
            // joining a thread : allowing one thread to wait for the completion of anothe 
            Thread t2=new Thread(new ThreadStart(wallet.RunRandomTransactions) );
            t2.Name = "T2";
            t2.Start();
            Console.WriteLine($"after start {t1.Name} state is : {t1.ThreadState} ");

            Console.ReadKey();
        }
    }





    class Wallet
    {
        public Wallet(string name, int bitCoins)
        {
            Name = name;
            BitCoins = bitCoins;
        }

        public string Name { get; set; }
        public int BitCoins { get; set; }





        public void Debit(int amount)//take bitcoins تداين
        {
            Thread.Sleep(1000);

            BitCoins -= amount;
            Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}--{Thread.CurrentThread.Name}" +
                   $" , Processor Id : {Thread.GetCurrentProcessorId()}]  -{amount}");

        }

        public void Credit(int amount)//give bitcoins تعطيه
        {

            Thread.Sleep(1000);
            BitCoins += amount;
            Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}--{Thread.CurrentThread.Name}" +
                   $" , Processor Id : {Thread.GetCurrentProcessorId()}]  +{amount}");

        }




        public void RunRandomTransactions()
        {
            int[] amounts = { 10, 20, 30, -20, 10, -10, 30, 40, -20 };

            foreach (var i in amounts)
            {
                var absValue = Math.Abs(i);
                if (i < 0)
                {
                    Debit(absValue);
                }
                else
                {
                    Credit(absValue);
                }
               
            }

        }


        public override string ToString()
        {
            return $"[{Name} -> {BitCoins} Bitcoins]";
        }
    }


}
