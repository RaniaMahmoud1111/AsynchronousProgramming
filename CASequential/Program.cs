namespace CASequential
{
    internal class Program
    {
        static void Main(string[] args)
        {


            var wallet = new Wallet("Rania", 80);

            wallet.RunRandomTransactions();
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"{wallet}\n");


              wallet.RunRandomTransactions();
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"{wallet}\n");



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
            BitCoins -=amount;
        }
        
        public void Credit(int amount)//give bitcoins تعطيه
        {
            BitCoins +=amount;
        }




        public void RunRandomTransactions()
        {
            int[] amounts = { 10,20,30,-20,10,-10,30,40,-20};

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
                Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId }"+
                    $"Processor Id : {Thread.GetCurrentProcessorId()}] {i}");

            }

        }


        public override string ToString()
        {
            return $"[{Name} -> {BitCoins} Bitcoins]";
        }
    }

}
