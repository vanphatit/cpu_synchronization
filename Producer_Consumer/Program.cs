using System;

class Program
{
    static int mutex = 1, full = 0, empty = 3, x = 0;

    static void Main(string[] args)
    {
        int n;
        Console.WriteLine("\n1.PRODUCER\n2.CONSUMER\n3.EXIT\n");
        while (true) // Vòng lặp vô hạn để liên tục đọc lựa chọn của người dùng
        {
            Console.WriteLine("\nENTER YOUR CHOICE\n");
            n = Convert.ToInt32(Console.ReadLine()); // Nhận lựa chọn từ người dùng
            switch (n)
            {
                case 1:
                    if ((mutex == 1) && (empty != 0)) // Kiểm tra có thể sản xuất hay không
                        Producer();
                    else
                        Console.WriteLine("BUFFER IS FULL");
                    break;
                case 2:
                    if ((mutex == 1) && (full != 0)) // Kiểm tra có thể tiêu thụ hay không
                        Consumer();
                    else
                        Console.WriteLine("BUFFER IS EMPTY");
                    break;
                case 3:
                    Environment.Exit(0); // Thoát chương trình
                    break;
            }
        }
    }

    static int Wait(int s) // Giảm giá trị của biến cờ
    {
        return (--s);
    }

    static int Signal(int s) // Tăng giá trị của biến cờ
    {
        return (++s);
    }

    static void Producer() // Hàm sản xuất
    {
        mutex = Wait(mutex); // Đợi mutex
        full = Signal(full); // Tăng full
        empty = Wait(empty); // Giảm empty
        x++; // Tăng x
        Console.WriteLine("\nproducer produces the item " + x); // Hiển thị sản phẩm được sản xuất
        mutex = Signal(mutex); // Báo hiệu kết thúc mutex
    }

    static void Consumer() // Hàm tiêu thụ
    {
        mutex = Wait(mutex); // Đợi mutex
        full = Wait(full); // Giảm full
        empty = Signal(empty); // Tăng empty
        Console.WriteLine("\nconsumer consumes item " + x); // Hiển thị sản phẩm được tiêu thụ
        x--; // Giảm x
        mutex = Signal(mutex); // Báo hiệu kết thúc mutex
    }
}
