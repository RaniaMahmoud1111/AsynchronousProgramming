namespace CADeadLock
{
    internal class Program
    {
        // with multi threading we face some problems
        // 1. Race Condition 
        //Race Condition : is a scenaroi where the outcome of the proram is affected because of timing 
        //what is the solution of race condition (Lock keyword )i need to make a block of code that one thread can go into at a time 
        //2.DeadLock : 
        //A deadlock is a situation where two or more threads are frozen in thier execution because they are waiting for each other to finish
        //what is the solution of DeadLock(1. order the operation,2. using monitor class show the cretical satuation )

        static void Main(string[] args)
        {


            #region  Happy Scenario


            var wallet1 = new Wallet() { Id=1,Name= "Mahmoud",BitCoins= 100 };
            var wallet2 = new Wallet() { Id=2,Name= "Rania",BitCoins= 50 };

            Console.WriteLine("********************* Happy Scenario ***********************");
            Console.WriteLine("\nBefore Transaction");
            Console.WriteLine("\n----------------------------");
            Console.WriteLine($"{wallet1} , {wallet2}");


            Console.WriteLine("\nAfter Transaction");
            Console.WriteLine("\n----------------------------");
            var transferManager = new TransferManager(wallet1, wallet2, 50);
            transferManager.Transfer();

            Console.WriteLine($"{wallet1} , {wallet2}");

            #endregion



            #region  Bad Scenario (they lock each other (to becomes from and from becomes to ))


            var AliWallet = new Wallet() { Id=3 ,Name="Ali",BitCoins= 100};
            var SaraWallet = new Wallet() { Id=4, Name= "Sara", BitCoins= 50 };

            Console.WriteLine("\n\n\n*********************** Bad Scenario *************************\n\nthey lock each other \n");


            Console.WriteLine("\nBefore Transaction");
            Console.WriteLine("\n----------------------------");
            Console.WriteLine($"{AliWallet} , {SaraWallet}");


            Console.WriteLine("\nAfter Transaction");
            Console.WriteLine("\n----------------------------");
            var transferManager1 = new TransferManager(AliWallet, SaraWallet, 50);
            var transferManager2 = new TransferManager(SaraWallet, AliWallet, 30);

            //exec on two different threads
            var t1 = new Thread(transferManager1.Transfer);
            t1.Name = "T1";
            var t2 = new Thread(transferManager2.Transfer);
            t2.Name = "T2";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
            Console.WriteLine($"{AliWallet} , {SaraWallet}"); 
            #endregion


            Console.ReadKey();
        }
    }



    class Wallet
    {

        private readonly object bitconinLock = new object();
        public Wallet()
        {
            
        }
        public Wallet( int id ,string name, int bitCoins)
        {
            Id = id;
            Name = name;
            BitCoins = bitCoins;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public int BitCoins { get; set; }



        public void Debit(int amount)//take bitcoins تداين
        {
            // lock to avoid the Race Condition 
            lock (bitconinLock)// send any ref type as static or readonly field 
            {
                if (BitCoins >= amount)
                {
                    Thread.Sleep(1000);

                    BitCoins -= amount;

                }

            }
        }

        public void Credit(int amount)//give bitcoins تعطيه
        {

            Thread.Sleep(1000);
            BitCoins += amount;

        }



        public override string ToString()
        {
            return $"[{Name} -> {BitCoins} Bitcoins]";
        }
    }


    // manager class :used to manage a set of objects of another class (to manager the transfer from one wallet to another )

    class TransferManager
    {
        private Wallet from;
        private Wallet to;
        private int amountToTransfer;


        public TransferManager(Wallet from, Wallet to, int amountToTransfer)
        {
            this.from = from;
            this.to = to;
            this.amountToTransfer = amountToTransfer;
        }


        #region  Transfer Method with DeadLock Problem 
        //public void Transfer()// with DeadLock Problem
        //{
        //    var lock1 = from.Id < to.Id ? from : to;
        //    var lock2 = from.Id < to.Id ? to : from;

        //    Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {from}");// wallet which i debit from i need to lock it to prevent any trnsaction on it at that time 

        //    lock (from)
        //    {
        //        Console.WriteLine($"{Thread.CurrentThread.Name} lock acquired  ...  {from}");
        //        Thread.Sleep(1000);

        //        Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {to}");

        //        lock (to)//nested lock
        //        {
        //            from.Debit(amountToTransfer);
        //            to.Credit(amountToTransfer);
        //        }
        //    }


        //}  
        #endregion

        #region  Transfer Method solution 1 for DeadLock Problem using Mointor Class
        //public void Transfer()// solution 1 fro DeadLock Problem 
        //{

        //    Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {from}");// wallet which i debit from i need to lock it to prevent any trnsaction on it at that time 

        //    lock (from)
        //    {
        //        Console.WriteLine($"{Thread.CurrentThread.Name} lock acquired  ...  {from}");
        //        Thread.Sleep(1000);

        //        Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {to}");


        //        if (Monitor.TryEnter(to, 1000))//نضمن ان واحد هشتغل 
        //        {
        //            Console.WriteLine($"{Thread.CurrentThread.Name} lock acquired   ...  {to}");

        //            try
        //            {
        //                from.Debit(amountToTransfer);
        //                to.Credit(amountToTransfer);
        //            }
        //            finally
        //            {
        //                Monitor.Exit(to);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"{Thread.CurrentThread.Name} unable to acquire lock on   ...  {to}");

        //        }


        //    }


        //}   
        #endregion


        #region Transfer Method solution 2 for DeadLock Problem by Ordering the operations
        public void Transfer()
        {
            var lock1 = from.Id < to.Id ? from : to;
            var lock2 = from.Id < to.Id ? to : from;

            Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {from}");// wallet which i debit from i need to lock it to prevent any trnsaction on it at that time 

            lock (lock1)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} lock acquired  ...  {from}");
                Thread.Sleep(1000);

                Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock  ...  {to}");


                lock (lock2)//nested lock
                {
                    from.Debit(amountToTransfer);
                    to.Credit(amountToTransfer);
                }


            }



        } 

        #endregion





    }



}
