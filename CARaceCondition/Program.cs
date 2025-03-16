namespace CARaceCondition
{
    // with multi threading we face some problems
    // 1. Race Condition 
    //Race Condition : is a scenaroi where the outcome of the proram is affected because of timing 
    //what is the solution of race condition (Lock keyword )i need to make a block of code that one thread can go into at a time 
    //2.DeadLock : 
    //A deadlock is a situation where two or more threads are frozen in thier execution because they are waiting for each other to finish 
    internal class Program
    {
        static void Main(string[] args)
        {

            var wallet = new Wallet("Rania", 50);
            //parallel
            //wallet.Debit(40);//10
            //wallet.Debit(30);// this here not make problem but in multi threading make a problem 

            var t1 = new Thread(() => wallet.Debit(40));
            var t2 = new Thread(() => wallet.Debit(30));


            t1.Start();
            t2.Start();

            t1.Join();// wait till thread 1 complete (here may before i tell wait , thread 2 may be  done )
            t2.Join();

            Console.WriteLine(wallet);




            Console.ReadKey();

        }

    }

    class Wallet
    {

        private readonly object  bitconinLock=new object();
        public Wallet(string name, int bitCoins)
        {
            Name = name;
            BitCoins = bitCoins;
        }

        public string Name { get; set; }
        public int BitCoins { get; set; }





        public void Debit(int amount)//take bitcoins تداين
        {
            // lock to avoid the Race Condition 
           lock(bitconinLock)// send any ref type as static or readonly field 
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
            BitCoins+=amount;
          
        }




   
        public override string ToString()
        {
            return $"[{Name} -> {BitCoins} Bitcoins]";
        }
    }

}
