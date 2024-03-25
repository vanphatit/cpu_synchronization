using System;
using System.Threading;

class DiningPhilosophers
{
    enum State { THINKING, HUNGRY, EATING }; // Định nghĩa trạng thái của mỗi triết gia
    static State[] state = new State[5]; // Mảng lưu trữ trạng thái của mỗi triết gia
    static object[] self = new object[5]; // Mảng các object self để sử dụng lock

    static void Main(string[] args)
    {
        // Khởi tạo các đối tượng self
        for (int i = 0; i < 5; i++)
        {
            self[i] = new object();
        }

        // Tạo và bắt đầu các luồng (threads) đại diện cho các triết gia
        Thread[] threads = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int philosopher = i;
            threads[i] = new Thread(() => Philosopher(philosopher));
            threads[i].Start();
        }

        Console.ReadLine(); // Đợi đến khi nhận được Enter từ người dùng
        Environment.Exit(0); // Thoát chương trình
    }

    static void Philosopher(int i)
    {
        // Vòng lặp vô hạn của mỗi triết gia
        while (true)
        {
            Think(i); // Triết gia suy nghĩ
            Pickup(i); // Triết gia muốn ăn
            Eat(i); // Triết gia ăn
            PutDown(i); // Triết gia kết thúc việc ăn
        }
    }

    static void Think(int i)
    {
        Console.WriteLine($"Philosopher {i} is thinking."); // In ra thông điệp cho triết gia đang suy nghĩ
        Thread.Sleep(1000); // Giả sử triết gia suy nghĩ trong 1 giây (1000 milliseconds)
    }

    static void Eat(int i)
    {
        Console.WriteLine($"Philosopher {i} is eating."); // In ra thông điệp cho triết gia đang ăn
        Thread.Sleep(5000); // Giả sử triết gia ăn trong 5 giây
    }

    static void Pickup(int i)
    {
        state[i] = State.HUNGRY; // Triết gia đó đang đói
        Test(i); // Kiểm tra xem triết gia đó có thể ăn được không
        lock (self[i]) // Sử dụng lock để đảm bảo sự an toàn khi thực hiện Monitor.Wait và Monitor.Pulse
        {
            if (state[i] != State.EATING) // Nếu triết gia không ăn được
                Monitor.Wait(self[i]); // Triết gia đợi cho đến khi được phép ăn
        }
    }

    static void PutDown(int i)
    {
        state[i] = State.THINKING; // Triết gia kết thúc việc ăn, đang suy nghĩ
        Test((i + 4) % 5); // Kiểm tra xem triết gia bên trái có thể ăn được không
        Test((i + 1) % 5); // Kiểm tra xem triết gia bên phải có thể ăn được không
    }

    static void Test(int i)
    {
        if (state[(i + 4) % 5] != State.EATING && // Nếu triết gia bên trái không ăn
            state[i] == State.HUNGRY && // Và triết gia đó đang đói
            state[(i + 1) % 5] != State.EATING) // Và triết gia bên phải không ăn
        {
            state[i] = State.EATING; // Triết gia đó được ăn
            lock (self[i]) // Sử dụng lock để đảm bảo sự an toàn khi thực hiện Monitor.Pulse
            {
                Monitor.Pulse(self[i]); // Báo cho triết gia đó biết rằng nó được phép ăn
            }
        }
    }
}
