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
}
class Field {
    private int x;
    private int y;
    // WARN: use List?
    private Chip[,] chip_map;

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
        for (int i = sx; i <= ex; i++) {
            var chip = new Chip();
            chip.type = ChipType.RoadFloor;
            chip_map[i, sy] = chip;
        }
        for (int i = ex; i <= sx; i++) {
            var chip = new Chip();
            chip.type = ChipType.RoadFloor;
            chip_map[i, sy] = chip;
        }
        for (int j = sy; j <= ey; j++) {
            var chip = new Chip();
            chip.type = ChipType.RoadFloor;
            chip_map[ex, j] = chip;
        }
        for (int j = ey; j <= sy; j++) {
            var chip = new Chip();
            chip.type = ChipType.RoadFloor;
            chip_map[ex, j] = chip;
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
                        Console.Write(".");
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
