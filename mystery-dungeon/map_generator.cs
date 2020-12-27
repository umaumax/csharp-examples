using System;
using System.Threading;

enum ChipType {
    RoomFloor,
    RoadFloor,
    Wall
}
class Chip {
    public ChipType type;
    public Chip() {
        type = ChipType.Wall;
    }
    public bool IsWall() { return type == ChipType.Wall; }
    public bool IsFloor() { return type == ChipType.RoomFloor || type == ChipType.RoadFloor; }
}
class Field {
    public int x { get; private set; }
    public int y { get; private set; }
    // WARN: use List?
    public Chip[,] chip_map { get; private set; }

    public Field(int x, int y) {
        Init(x, y);
    }

    public bool Init(int x, int y) {
        this.x = x;
        this.y = y;
        chip_map = new Chip[x, y];
        for (int j = 0; j < y; j++) {
            for (int i = 0; i < x; i++) {
                var chip = new Chip();
                chip_map[i, j] = chip;
            }
        }
        return true;
    }

    public bool CreatePath(int sx, int sy, int ex, int ey) {
        if (sx < 0 || sy < 0 || ex >= x || ey >= y) {
            // overflow from map
            return false;
        }
        // WARN: below code is redundant
        var rand = new Random();
        if (rand.Next(0, 1 + 1) == 0) {
            CreateXPath(sx, ex, sy);
            CreateYPath(sy, ey, ex);
        } else {
            CreateYPath(sy, ey, sx);
            CreateXPath(sx, ex, ey);
        }
        return true;
    }
    public bool CreateXPath(int sx, int ex, int by) {
        if (sx < 0 || ex < 0 || sx >= x || ex >= x || by < 0 || by >= y) {
            // overflow from map
            return false;
        }
        if (sx <= ex) {
            for (int i = sx; i <= ex; i++) {
                var chip = new Chip();
                chip.type = ChipType.RoadFloor;
                chip_map[i, by] = chip;
            }
        } else {
            for (int i = ex; i <= sx; i++) {
                var chip = new Chip();
                chip.type = ChipType.RoadFloor;
                chip_map[i, by] = chip;
            }
        }
        return true;
    }
    public bool CreateYPath(int sy, int ey, int bx) {
        if (sy < 0 || ey < 0 || sy >= y || ey >= y || bx < 0 || bx >= x) {
            // overflow from map
            return false;
        }
        if (sy <= ey) {
            for (int j = sy; j <= ey; j++) {
                var chip = new Chip();
                chip.type = ChipType.RoadFloor;
                chip_map[bx, j] = chip;
            }
        } else {
            for (int j = ey; j <= sy; j++) {
                var chip = new Chip();
                chip.type = ChipType.RoadFloor;
                chip_map[bx, j] = chip;
            }
        }
        return true;
    }

    public bool CreateRoomBasedCenter(int cx, int cy, int w, int h) {
        int sx = cx - w / 2;
        int sy = cy - h / 2;
        return CreateRoom(sx, sy, w, h);
    }

    public bool CreateRoom(int sx, int sy, int w, int h) {
        if (sx < 0 || sy < 0 || sx + w >= x || sy + h >= y) {
            // overflow from map
            return false;
        }
        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                int tx = sx + i;
                int ty = sy + j;
                var chip = new Chip();
                chip.type = ChipType.RoomFloor;
                chip_map[tx, ty] = chip;
            }
        }
        return true;
    }

    public void Print() {
        for (int j = 0; j < y; j++) {
            for (int i = 0; i < x; i++) {
                switch (chip_map[i, j].type) {
                    case ChipType.RoomFloor:
                        Console.Write(" ");
                        break;
                    case ChipType.RoadFloor:
                        Console.Write(" ");
                        // Console.Write(".");
                        break;
                    case ChipType.Wall:
                        Console.Write("*");
                        break;
                    default:
                        Console.Write("?");
                        break;
                }
            }
            Console.WriteLine();
        }
    }
}

class FieldGenerator {
    public int xsize;
    public int ysize;
    public int padding;
    public FieldGenerator() {
        xsize = 120;
        ysize = 30;
        padding = 8;
    }

    public Field Generate() {
        var field = new Field(xsize, ysize);
        var rand = new Random();
        int rx1 = rand.Next(0 + padding, xsize - padding - 1 + 1);
        int ry1 = rand.Next(0 + padding, ysize - padding - 1 + 1);
        int w = rand.Next(4, 16);
        int h = rand.Next(4, 10);
        field.CreateRoomBasedCenter(rx1, ry1, w, h);
        for (int i = 0; i < 10; i++) {
            int rx2 = rand.Next(0 + padding, xsize - padding - 1 + 1);
            int ry2 = rand.Next(0 + padding, ysize - padding - 1 + 1);
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
        return field;
    }
}
