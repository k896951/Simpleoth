using System;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Text;

namespace Simpleoth
{
    enum Chips
    {
             blank
            , nonBoard
            , BlackChip
            , WhiteChip
            , BlueChip
    }

    struct Point
    {
        public int posx;
        public int posy;

        public Point(int x, int y)
        {
            posx = x;
            posy = y;
        }
    };

    struct Vector
    {
        public int vecx;
        public int vecy;

        public Vector(int vx, int vy)
        {
            vecx = vx;
            vecy = vy;
        }
    };

    /// <summary>
    /// ボードのクラス
    /// </summary>
    class PlayBoard
    {
        private Chips[,] playbd;
        private Point[][] thinkData = {
                                        new Point[]{  new Point(1,1) ,  new Point(8,8) ,  new Point(1,8) ,  new Point(8,1) },
                                        new Point[]{  new Point(3,4) ,  new Point(4,6) ,  new Point(6,5) ,  new Point(5,3)
                                                    , new Point(3,5) ,  new Point(6,4) ,  new Point(5,6) ,  new Point(4,3) },
                                        new Point[]{  new Point(1,4) ,  new Point(8,5) ,  new Point(5,1) ,  new Point(4,8)
                                                    , new Point(5,8) ,  new Point(8,4) ,  new Point(4,1) ,  new Point(1,5) },
                                        new Point[]{  new Point(8,3) ,  new Point(3,8) ,  new Point(6,1) ,  new Point(1,6)
                                                    , new Point(6,8) ,  new Point(8,6) ,  new Point(3,1) ,  new Point(1,3) },
                                        new Point[]{  new Point(4,2) ,  new Point(5,2) ,  new Point(2,4) ,  new Point(2,5)
                                                    , new Point(4,7) ,  new Point(5,7) ,  new Point(7,5) ,  new Point(7,4) },
                                        new Point[]{  new Point(1,2) ,  new Point(1,7) ,  new Point(2,8) ,  new Point(7,8)
                                                    , new Point(8,7) ,  new Point(8,2) ,  new Point(7,1) ,  new Point(2,1)
                                                    , new Point(2,6) ,  new Point(3,7) ,  new Point(6,7) ,  new Point(7,6)
                                                    , new Point(7,3) ,  new Point(6,1) ,  new Point(3,2) ,  new Point(2,3) },
                                        new Point[]{  new Point(2,2) ,  new Point(6,6) ,  new Point(6,3) ,  new Point(2,7)
                                                    , new Point(7,7) ,  new Point(3,3) ,  new Point(7,2) ,  new Point(3,6) },
                                        new Point[]{  new Point(4,4) ,  new Point(5,5) ,  new Point(4,5) ,  new Point(5,4) }
                                      };
        private Vector[] searchVecter = {
                                            new Vector( 1, 0), new Vector( 1, 1), new Vector( 0, 1), new Vector(-1, 1)
                                          , new Vector(-1, 0), new Vector(-1,-1), new Vector( 0,-1), new Vector( 1,-1)
                                        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayBoard()
        {
            playbd = new Chips[10, 10];

            //// ブランクで埋め尽くす
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    playbd[x, y] = Chips.blank;
                }
            }

            //// 外枠の設定
            for (int xy = 0; xy < 10; xy++)
            {
                playbd[xy, 0] = Chips.nonBoard;
                playbd[xy, 9] = Chips.nonBoard;
                playbd[0, xy] = Chips.nonBoard;
                playbd[9, xy] = Chips.nonBoard;
            }

            //// 中央の４つのコマ
            playbd[4, 4] = Chips.BlackChip;
            playbd[5, 5] = Chips.BlackChip;
            playbd[4, 5] = Chips.WhiteChip;
            playbd[5, 4] = Chips.WhiteChip;
        }

        /// <summary>
        /// 指定の場所に駒を置いた場合いくつ取れるのか確認する
        /// </summary>
        /// <returns>ひっくり返せる駒の数</returns>
        private int SearchBoard(Chips myChip, int posx, int posy)
        {
            int count;
            Chips enemyChip;

            if ((1 > posx) || (8 < posx) || (1 > posy) || (8 < posy)) return 0;

            if (playbd[posx, posy] != Chips.blank) return 0;

            count = 0;
            switch (myChip)
            {
                case Chips.WhiteChip:
                    enemyChip = Chips.BlackChip;
                    break;

                case Chips.BlackChip:
                    enemyChip = Chips.WhiteChip;
                    break;

                default:
                    enemyChip = Chips.blank;
                    break;
            }

            //// ８方向に検索をかけてみる
            for (int i = 0; i < searchVecter.Length; i++)
            {
                int bx = posx + searchVecter[i].vecx;
                int by = posy + searchVecter[i].vecy;
                int vecCount = 0;

                while (playbd[bx, by] == enemyChip)
                {
                    vecCount++;
                    bx += searchVecter[i].vecx;
                    by += searchVecter[i].vecy;
                }

                switch (playbd[bx, by])
                {
                    case Chips.blank:
                    case Chips.nonBoard:
                        vecCount = 0;
                        break;
                }

                count += vecCount;
            }

            return count;
        }

        /// <summary>
        /// 指定の場所に駒を置いて相手の駒をひっくり返す
        /// </summary>
        /// <returns>ひっくり返した数</returns>
        public int ReverseChip(Chips myChip, int posx, int posy)
        {
            int count;
            Chips enemyChip;

            if ((1 > posx) || (8 < posx) || (1 > posy) || (8 < posy)) return 0;

            if (playbd[posx, posy] != Chips.blank) return 0;

            count = 0;
            switch (myChip)
            {
                case Chips.WhiteChip:
                    enemyChip = Chips.BlackChip;
                    break;

                case Chips.BlackChip:
                    enemyChip = Chips.WhiteChip;
                    break;

                default:
                    enemyChip = Chips.blank;
                    break;
            }

            //// ８方向に検索をかけてみる
            for (int i = 0; i < searchVecter.Length; i++)
            {
                int bx = posx + searchVecter[i].vecx;
                int by = posy + searchVecter[i].vecy;
                int vecCount = 0;

                while (playbd[bx, by] == enemyChip)
                {
                    vecCount++;
                    bx += searchVecter[i].vecx;
                    by += searchVecter[i].vecy;
                }

                switch (playbd[bx, by])
                {
                    case Chips.blank:
                    case Chips.nonBoard:
                        vecCount = 0;
                        break;

                    //// 自分の駒にたどり着いたなら戻りつつひっくり返す
                    default:
                        bx -= searchVecter[i].vecx;
                        by -= searchVecter[i].vecy;
                        while (playbd[bx, by] == enemyChip)
                        {
                            playbd[bx, by] = myChip;
                            bx -= searchVecter[i].vecx;
                            by -= searchVecter[i].vecy;
                        }
                        break;
                }

                count += vecCount;
            }

            if (count != 0)
            {
                playbd[posx, posy] = myChip;
            }

            return count;
        }

        /// <summary>
        /// 駒をおけるのか確認する
        /// </summary>
        /// <returns>置けるなら true</returns>
        public bool IsPutting(Chips myChip)
        {
            for (int lvl = 0; lvl < thinkData.Length; lvl++)
            {
                for (int sel = 0; sel < thinkData[lvl].Length; sel++)
                {
                    if (0 < SearchBoard(myChip, thinkData[lvl][sel].posx, thinkData[lvl][sel].posy))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// お勧めの場所を教えてもらう
        /// </summary>
        /// <returns>無ければ(0,0),あればその場所のpoint構造体が返る</returns>
        public Point SearchBestPoint(Chips myChip)
        {
            int count = 0;
            Point bestPoint = new Point(0, 0);

            for (int lvl = 0; lvl < thinkData.Length; lvl++)
            {
                int selCount = 0;
                for (int sel = 0; sel < thinkData[lvl].Length; sel++)
                {
                    selCount = SearchBoard(myChip, thinkData[lvl][sel].posx, thinkData[lvl][sel].posy);
                    if (count < selCount)
                    {
                        count = selCount;
                        bestPoint.posx = thinkData[lvl][sel].posx;
                        bestPoint.posy = thinkData[lvl][sel].posy;
                    }
                }
                if (count != 0) break;
            }

            return bestPoint;
        }

        /// <summary>
        /// ボードのひょうじ
        /// </summary>
        public void Display()
        {
            string sy = "ａｂｃｄｅｆｇｈ";

            Console.WriteLine("　１２３４５６７８");

            for (int y = 1; y < 9; y++)
            {
                Console.Write(sy.Substring(y - 1, 1));

                for (int x = 1; x < 9; x++)
                {
                    switch(playbd[x,y])
                    {
                        case Chips.BlackChip:
                            Console.Write("○");
                            break;

                        case Chips.WhiteChip:
                            Console.Write("●");
                            break;

                        case Chips.blank:
                            Console.Write("・");
                            break;
                    }
                }

                Console.WriteLine("");
            }

            Console.WriteLine("");
        }

        /// <summary>
        /// 入力
        /// </summary>
        public Point Input(Chips myChip)
        {
            Point pos = new Point(0, 0);

            while (1 == 1)
            {
                Console.Write("場所を入力(1-8,a-h) 例:4,f >");

                string inline = Console.ReadLine();
                if (inline.Length > 2)
                {
                    pos.posx = "12345678".LastIndexOf(inline.Substring(0, 1)) + 1;
                    pos.posy = "abcdefgh".LastIndexOf(inline.Substring(2, 1)) + 1;

                    if ((0 < pos.posx) && (pos.posx < 9) && (0 < pos.posy) && (pos.posy < 9))
                    {
                        if (0 < SearchBoard(myChip, pos.posx, pos.posy) ) break;
                    }
                }
            }
            return pos;
        }

    }


    /// <summary>
    /// 主処理
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            PlayBoard pbd = new PlayBoard();
            Chips turn = Chips.BlackChip;

            if (args.Length > 1)
            {
                switch (args[0])
                {
                    case "black":
                        turn = Chips.BlackChip;
                        break;

                    case "white":
                        turn = Chips.WhiteChip;
                        break;

                    default:
                        turn = Chips.BlackChip;
                        break;
                }
            }
            else
            {
                turn = Chips.BlackChip;
            }

            Console.WriteLine("PCと楽しむおせろもどき");
            int count_m = 2;
            int count_e = 2;

            while (1 == 1)
            {
                pbd.Display();

                if ((false == pbd.IsPutting(Chips.BlackChip)) && (false == pbd.IsPutting(Chips.WhiteChip)))
                {
                    Console.WriteLine("ゲーム終了");
                    Console.WriteLine("あなた: {0},  PC: {1} ", count_m, count_e);

                    if (count_m > count_e)
                    {
                        Console.WriteLine("あなた の勝ちっす");
                    }
                    if (count_m < count_e)
                    {
                        Console.WriteLine("PC の勝ちっす");
                    }
                    if (count_m == count_e)
                    {
                        Console.WriteLine("だっせぇ、同点っす");
                    }
                    Console.Write("<キーを押して終了>");
                    Console.ReadLine();
                    break;
                }


                if (turn == Chips.BlackChip)
                {
                    Console.WriteLine("あなた(○)のターンです");

                    if (true == pbd.IsPutting(Chips.BlackChip))
                    {
                        Point p = pbd.Input(Chips.BlackChip);
                        count_m+=pbd.ReverseChip(Chips.BlackChip, p.posx, p.posy);

                        pbd.Display();
                    }
                    else
                    {
                        Console.WriteLine("打つ場所がないのでパスします。");
                    }

                    turn = Chips.WhiteChip;
                }

                //Console.Write("<Enterキーを押してねー>");
                //Console.ReadLine();
                //Console.Clear();


                if (turn == Chips.WhiteChip)
                {
                    Console.WriteLine("PC(●)のターンです");

                    if (true == pbd.IsPutting(Chips.WhiteChip))
                    {
                        Point p = pbd.SearchBestPoint(Chips.WhiteChip);
                        count_e += pbd.ReverseChip(Chips.WhiteChip, p.posx, p.posy);
                        Console.WriteLine("{0},{1} に打ちました", p.posx, "abcdefgh".Substring(p.posy - 1, 1));
                    }
                    else
                    {
                        Console.WriteLine("打つ場所がないのでパスします。");
                    }

                    turn = Chips.BlackChip;
                }

            }

            return ;

        }
    }
}
