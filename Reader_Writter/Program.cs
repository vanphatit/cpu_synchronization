using System;
using System.Threading;

class Program
{
    static SemaphoreSlim db = new SemaphoreSlim(1); // Semaphore cho việc kiểm soát truy cập vào cơ sở dữ liệu
    static SemaphoreSlim mutex = new SemaphoreSlim(1); // Semaphore cho việc kiểm soát truy cập vào biến nReaders
    static int nReaders = 0; // Số lượng Readers
    static LinkedList<string> logs = new LinkedList<string>(); // Mảng chứa các log

    static void Main(string[] args)
    {
        int choice;
        Console.WriteLine("\n1.WRITER\n2.READER\n3.EXIT\n4.LOG\n");
        while (true) // Vòng lặp vô hạn để liên tục đọc lựa chọn của người dùng
        {
            Console.WriteLine("\nENTER YOUR CHOICE\n");
            try
            {
                choice = Convert.ToInt32(Console.ReadLine()); // Nhận lựa chọn từ người dùng
                switch (choice)
                {
                    case 1:
                        new Thread(Writer).Start(); // Khởi động một thread mới cho Writer
                        break;
                    case 2:
                        new Thread(Reader).Start(); // Khởi động một thread mới cho Reader
                        break;
                    case 3:
                        Environment.Exit(0); // Thoát chương trình
                        break;
                    case 4:
                        Console.WriteLine("================== LOG ===================");
                        foreach (string log in logs)
                        {
                            if (log != null)
                                Console.WriteLine(log);
                        }
                        Console.WriteLine("===========================================");
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please enter 1, 2 or 3.");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid choice! Please enter 1, 2 or 3.");
            }
            
        }
    }

    static void Writer()
    {
        db.Wait(); // Đảm bảo chỉ một Writer được phép truy cập vào cơ sở dữ liệu
        WriteToDB();
        db.Release(); // Giải phóng quyền truy cập vào cơ sở dữ liệu
    }

    static void Reader()
    {
        mutex.Wait(); // Đảm bảo chỉ một Reader được phép tăng biến nReaders
        if (nReaders == 0)
            db.Wait(); // Nếu đây là Reader đầu tiên, đợi quyền truy cập vào cơ sở dữ liệu
        nReaders++;
        mutex.Release(); // Giải phóng quyền truy cập vào biến nReaders

        ReadFromDB();

        mutex.Wait(); // Đảm bảo chỉ một Reader được phép giảm biến nReaders
        nReaders--;
        if (nReaders == 0)
            db.Release(); // Nếu đây là Reader cuối cùng, giải phóng quyền truy cập vào cơ sở dữ liệu
        mutex.Release(); // Giải phóng quyền truy cập vào biến nReaders
    }

    static void WriteToDB()
    {
        logs.AddLast("Writer is writing to the database...(3 sec)");
        Console.WriteLine(logs.Last.Value);
        for (int i = 0; i < 3; i++)// Giả sử việc ghi một bản ghi mất 3 giây
        {
            Console.WriteLine("Writing record " + i);
            Thread.Sleep(1000); 
        }
        logs.AddLast("Writer finished writing to the database.");
        Console.WriteLine(logs.Last.Value);
    }

    static void ReadFromDB()
    {
        logs.AddLast("Reader " + nReaders + " is reading from the database...(1 sec)");
        Console.WriteLine(logs.Last.Value);
        Thread.Sleep(2000); // Giả sử việc đọc dữ liệu mất 2 giây   
        logs.AddLast("Reader " + nReaders + " finished reading from the database.");
        Console.WriteLine(logs.Last.Value);
    }
}
