using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.ExceptionServices;
using System.Text;
namespace MazeExtend
{

    public class Maze
    {
        public int[,] field;
        public int fieldX;
        public int fieldY;
        public int tilesmin = 0;
        public int playerX;
        public int playerY;
        public Maze(int x, int y)
        {
            fieldX = x;
            fieldY = y;
            if (x < 5) fieldX = 5;
            if (y < 5) fieldY = 5;
            if (fieldX % 2 == 0) fieldX++;
            if (fieldY % 2 == 0) fieldY++;
            this.field = new int[fieldX, fieldY];
            for (int wx = 0; wx < fieldX; wx++)
            {
                field[wx, 0] = 8;
                field[wx, fieldY - 1] = 8;
            }
            for (int wy = 0; wy < fieldY; wy++)
            {
                field[0, wy] = 8;
                field[fieldX - 1, wy] = 8;
            }
            //Console.ReadKey(false);
        }
        public void Build()
        {
            int putX, putY, direction; //掘削位置と方向
            int pXMAX = fieldX - 2;
            int pYMAX = fieldY - 2;

            // いずれも壁を置ける最大位置
            Random r = new Random();
            Func<int, int> Evennumber = MAX => {
                Random r = new Random();
                int i = 1;
                while (i % 2 == 1)
                {
                    i = r.Next(1, MAX+1);
                }
                return i;

            };
            while (!Built())
            {
                putX = Evennumber(pXMAX);
                putY = Evennumber(pYMAX);

                while (field[putX, putY] != 8)
                {
                    field[putX, putY] = 8;
                    if (putX <= pXMAX && putY <= pYMAX)
                    {
                        direction = r.Next(1, 5);

                        //Console.ReadKey(false);
                        switch (direction)
                        {
                            case 1: //上
                                field[putX, putY - 1] = 8;
                                putY -= 2;
                                //Console.WriteLine("上");
                                break;
                            case 2: //右
                                field[putX + 1, putY] = 8;
                                putX += 2;
                                //Console.WriteLine("右");
                                break;
                            case 3: //下
                                field[putX, putY + 1] = 8;
                                putY += 2;
                                //Console.WriteLine("下");
                                break;
                            case 4: //左
                                field[putX - 1, putY] = 8;
                                putX -= 2;
                                //Console.WriteLine("左");
                                break;
                        }
                        if (putY < 0) putY = 0;
                        if (putX >= pXMAX) putX = pXMAX ;
                        if(putY>=pYMAX)putY= pYMAX;
                        if(putX < 0) putX = 0;

                    }



                }
            }
        }
        public bool Putlinkedwall(int x, int y, int getvalue, int setvalue)
        {
            if (x > 0 && y > 0 && x < fieldX - 1 && y < fieldY - 1)
            {
                if (field[x, y - 1] == setvalue && field[x, y] == getvalue)
                {
                    field[x, y] = setvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x + 1, y] == setvalue && field[x, y] == getvalue)
                {
                    field[x, y] = setvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x, y + 1] == setvalue && field[x, y] == getvalue)
                {
                    field[x, y] = setvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x - 1, y] == setvalue && field[x, y] == getvalue)
                {
                    field[x, y] = setvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
            }
            return false;
        }
        public bool Linkwall(int x, int y, int getvalue, int setvalue)
        {
            if (x > 0 && y > 0 && x < fieldX - 2 && y < fieldY - 2)
            {
                if (field[x, y - 1] == setvalue && field[x, y + 1] == getvalue)
                {
                    field[x, y] = getvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x + 1, y] == setvalue && field[x - 1, y] == getvalue)
                {
                    field[x, y] = getvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x, y + 1] == setvalue && field[x, y - 1] == getvalue)
                {
                    field[x, y] = getvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
                if (field[x - 1, y] == setvalue && field[x + 1, y] == getvalue)
                {
                    field[x, y] = getvalue;
                    //Console.Clear();
                    //Print();
                    return true;
                }
            }
            return false;
        }
        public bool Getisolatedwalls()
        {
            bool replaced=false;
            for (int x = 1; x < fieldX - 1; x++)
            {
                for (int y = 1; y < fieldY - 1; y++)
                {

                    if (Putlinkedwall(x, y, 8, 16)) replaced=true;
                }

            }
            return replaced;
        }
        public bool Checklinked()
        {
            for (int x = 0; x < fieldX; x++)
            {
                for (int y = 0; y < fieldY; y++)
                {
                    if (field[x, y] == 8) return true;
                }

            }
            return false;
        }
        public void Finish(int getvalue, int setvalue)
        {
            for (int x = 0; x < fieldX; x++)
            {
                for (int y = 0; y < fieldY; y++)
                {
                    if (field[x, y] == getvalue) field[x, y] = setvalue;
                }

            }
        }
        public void Print()
        {
            String put = ".";
            //https://smdn.jp/programming/dotnet-samplecodes/arrays/3ac2dc0c024111eb907175842ffbe222/
            for (var d1 = 0; d1 < field.GetLength(0); d1++)
            {
                for (var d2 = 0; d2 < field.GetLength(1); d2++)
                {
                    if (field[d1, d2] == 16) { put = "?"; }
                    if (field[d1, d2] == 8) { put = "#"; }
                    if (field[d1, d2] == 0) { put = "."; }
                    Console.Write(put);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool Built()
        {
            for (int wy = 2; wy < fieldY - 1; wy += 2)
            {
                for (int wx = 2; wx < fieldX - 1; wx += 2)
                {
                    if (field[wx, wy] == 0)
                    {
                        foreach (var x in field)
                        //Console.Clear();
                        //Print();
                        return false;
                    }
                }
            }

            //Console.Clear();
            //Print();

            return true;
        }
    }

    public class Program
    {
        public static (int x, int y) input()
        {
            for (; ; )
            {
                Console.WriteLine("迷路の横、縦の幅を入力してください");
                Console.WriteLine("（x,y）半額数字を,で区切って入力 xは5以上31以下 yは5以上23以下");
                Console.WriteLine("奇数のみ入力可");
                Console.Write("> ");
                string s = Console.ReadLine();
                string[] strings=s.Split(",");
                int[] array = { 0, 0 };
                if (strings.Length == 2)
                {
                    if ((int.TryParse(strings[0], out array[0]))&& (int.TryParse(strings[1], out array[1])))
                    {
                        if (array[0] >= 5 && array[0] <= 31 && array[1]>=5&&array[1]<=23)
                        {
                            if (array[0]%2==1 && array[1]%2==1)return (array[0],array[1]);
                        }
                    }
                }
                Console.WriteLine("入力値が不正です");
            }
        }
        public static void Main(string[] args)
        {
            var wh = input();
            Maze m = new Maze(wh.x,wh.y);
            m.Build();
            for (int wx = 0; wx < m.fieldX; wx++)
            {
                m.field[wx, 0] = 16;
                m.field[wx, m.fieldY - 1] = 16;
            }
            for (int wy = 0; wy < m.fieldY; wy++)
            {
                m.field[0, wy] = 16;
                m.field[m.fieldX - 1, wy] = 16;
            }
            ///Console.Clear();
            //m.Print();
            Random r = new Random();
         

                for (; ; )
                {
                    if (!m.Getisolatedwalls()&&!m.Linkwall(r.Next(0, m.fieldX), r.Next(0, m.fieldY), 8, 16)) { 
                        if (!m.Checklinked())break ;
                    };
                }
            m.Finish(16, 8);

                

            Console.Clear();
            m.Print();
            Console.WriteLine("x:{0} y:{1}", m.fieldX, m.fieldY);
            Console.ReadKey(false);
        }
    }

}