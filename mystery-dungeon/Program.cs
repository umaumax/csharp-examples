using System;

class Program {
    static void Main(string[] args) {
        int x = 120;
        int y = 30;
        int padding = 8;
        Field field = new Field(x, y);
        var rand = new Random();
        int rx1 = rand.Next(0 + padding, x - padding - 1 + 1);
        int ry1 = rand.Next(0 + padding, y - padding - 1 + 1);
        int w = rand.Next(4, 16);
        int h = rand.Next(4, 10);
        field.CreateRoomBasedCenter(rx1, ry1, w, h);
        for (int i = 0; i < 10; i++) {
            int rx2 = rand.Next(0 + padding, x - padding - 1 + 1);
            int ry2 = rand.Next(0 + padding, y - padding - 1 + 1);
            field.CreatePath(rx1, ry1, rx2, ry2);
            w = rand.Next(4, 16);
            h = rand.Next(4, 10);
            var room = field.CreateRoomBasedCenter(rx2, ry2, w, h);
            Console.WriteLine("room {0}, {1}, {2}, {3}", rx2, ry2, w, h);
            if (room) {
                Console.WriteLine("creat room");
            }
            rx1 = rx2;
            ry1 = ry2;
        }
        field.Print();
    }
}

