using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    /*
     T=2 Q=5 
     K=3 O=6
     C=4 P=1

     8  T K C Q O C K T
     7  P P P P P P P P 
     6  / / / / / / / /
     5  / / / / / / / /
     4  / / / / / / / /
     3  / / / / / / / /
     2  P P P P P P P P
     1  T K C Q O C K T
     
        A B C D E F G H
    */

    class Program
    {

        public static int Turn = 0;

        static void Main(string[] args)
        {

            // Init text             
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Font.SetConsoleFont("MS Gothic");

            // Init game
                Terrain Chess1 = new Terrain();
                DrawMap(Chess1);
                
            while (true)
            {

                Console.Write("[{0}]({1}) Move: ", Turn, Turn%2==0?"White":"Black" );
                    string info = Console.ReadLine();


                // Parse Info
                    string[] parsed_move = info.Split(' ');
                    Console.WriteLine("{0} -> {1}", parsed_move[0], parsed_move[1]);

                // Creating the move, by spliting position A=[0][0] 1=[0][1]                    
                    Move move = new Move();
                        move.from = new Vector(parsed_move[0][0], parsed_move[0][1]);
                        move.to = new Vector(parsed_move[1][0], parsed_move[1][1]);


                // Exectue the move
                   bool succes = Chess1.UpdatePosition(move);
            
                        if (succes)
                        { 
                            // Increase Turn count
                                Turn++;
                        }


                    Console.ReadKey();
                 // Drawn the updated terrain
                    Console.Clear();
                    DrawMap(Chess1);

            }


        }

        static void DrawMap(Terrain terrain)
        {
            string[] Y = { "1   │ ", "2   │ ", "3   │ ", "4   │ ", "5   │ ", "6   │ ", "7   │ ", "8   │ " };

            for (int i = 0; i < terrain.Board.GetLength(0); i++)
            {
                Console.WriteLine("     ───────────────────────────────");
                for (int j = 0; j < terrain.Board.GetLength(1); j++)
                {

                    if (terrain.Board[i, j].unit != null)
                    {
                        Console.Write(Y[i] + terrain.Board[i, j].unit + " │ ");
                    } else
                    {
                        Console.Write(Y[i] + "  │ ");
                    }

                    Y[i] = null;

                }
                
                Console.WriteLine();
            }
            Console.WriteLine("     ───────────────────────────────");
            Console.WriteLine("\n      A   B   C   D   E   F   G   H\n");
        }


    }

    public struct Vector
    {
        public int x;
        public int y;

        public Vector(char X, int Y)
        {
            // Convert ASCII to correcter Integer
                x = X-97;
                y = Y-49;
        }

    }

    public class Move
    {
         // A1 -> A2
         public Vector from { get; set; }
         public Vector to { get; set; }
    }

    public class Pons
    {
        public string unit;
        public string unit_;    // Without special char, used for regonisation in other function.. after we declared the piece, see the switch case.
        public int player;
        public bool first_move = false;

        public Pons(string UNIT = null, int PLAYER = 5) // Default Value
        {
            unit_ = UNIT;

            switch (UNIT)
            {

                case "T": unit = "\u2656"; break;
                case "K": unit = "\u2658"; break;
                case "C": unit = "\u2657"; break;
                case "O": unit = "\u265A"; break;
                case "Q": unit = "\u265B"; break;
                case "P": unit = "\u2659"; break;

            }
            player = PLAYER;
            
        }

    }

    public class Terrain
    {



        public Pons[,] Board = new Pons[8, 8]
           {
                {new Pons("T",0),new Pons("K",0),new Pons("C",0),new Pons("O",0),new Pons("Q",0),new Pons("C",0),new Pons("K",0),new Pons("T",0)},
                {new Pons("P",0),new Pons("P",0),new Pons("P",0),new Pons("P",0),new Pons("P",0),new Pons("P",0),new Pons("P",0),new Pons("P",0)},
                {new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons()},
                {new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons()},
                {new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons()},
                {new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons(),new Pons()},
                {new Pons("P",1),new Pons("P",1),new Pons("P",1),new Pons("P",1),new Pons("P",1),new Pons("P",1),new Pons("P",1),new Pons("P",1)},
                {new Pons("T",1),new Pons("K",1),new Pons("C",1),new Pons("O",1),new Pons("Q",1),new Pons("C",1),new Pons("K",1),new Pons("T",1)}

           };

        public bool UpdatePosition(Move move)
        {

            // Update terrain
                    Pons from = Board[move.from.y, move.from.x];
                    Pons to = Board[move.to.y, move.to.x];

                    Console.WriteLine("{0}:{1} -> {1}:{2}", move.from.y, move.from.x, move.to.y, move.to.x);

            // Check if the move is legal
                if (rules(from, to, move))
                { 

                    // Copy the piece and erase the previous one
                        Board[move.to.y, move.to.x] = Board[move.from.y, move.from.x];
                        Board[move.from.y, move.from.x] = new Pons();

                } else {

                    Console.WriteLine("Illegal Move!");
                    Console.ReadKey();
                    return false;

                }

            return true;
        }

        public bool rules(Pons PONS_FROM, Pons PONS_TO, Move move)
        {

            // Régle numéro 1: IT'S MY TURN!
                // Si le tour, modulo 2 == 0 blanc, n'est pas 1 == blanc alors zouh
                   if (( Program.Turn%2 == 0 && PONS_FROM.player != 1) || (Program.Turn % 2 != 0 && PONS_FROM.player == 1))
                   {
                        return false;
                   }


            // Régle numéro 2: Friendly Fire!
                if (!(PONS_TO.player != PONS_FROM.player)) { return false; }


            // Régle numéro 2: YOU CAN'T DO THAT DUDE
                
                // Vérifie si la destination est accesible par la pièce, et leurs comportement
                        // Autorise les pieces suivantes
                        switch (PONS_FROM.unit_)
                            {

                                // Roi
                                case "O":
                                    // Can only move 1 ahead and can't eat friendly piece

                                        if ( (move.from.x+1 >= move.to.x) || (move.from.y+1 >= move.to.y) )
                                        {

                                            return true;

                                        }

                                break;

                                // Reine
                                case "Q":

                                    return true;
                                break;

                                // Pion
                                case "P":


                                    // Can only go forward
                                        if (move.from.x != move.to.x)
                                        {
                                            return false;
                                        }

                                    // Trying a first move when already one happend
                                        if (move.from.x+2 <= move.to.x && PONS_FROM.first_move == false)
                                        {

                                             return false;

                                        }

                                        Console.WriteLine(move.from.y + ":" + move.to.y);

                                        if (move.from.y - move.to.y <1)
                                        {
                            
                                            return true;
                            
                                        }


                                   // Signale le premier mouvement, pas besoin de if, opération inutile.
                                          PONS_FROM.first_move = true;



                                break;

                                default: return false;

                            }



            return true;
        }


    }

}
