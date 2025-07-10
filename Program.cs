//  Jonathan Bodrero  
//  Jul 10, 2025  (building on code I wrote for Lab 7: Maze)
//  Lab 8:  Maze 2

using System.Diagnostics;  //  Allows stopwatch functionality
Stopwatch watch = new Stopwatch();

int locFromLeft = 0;    //  Set location variables.
int locFromTop = 0;
int proposedFromLeft = 0;
int proposedFromTop = 0;
int score = 0;
int bad1FromLeft = 0;
int bad1FromTop = 0;


Console.Clear();        //  Give instructions
Console.WriteLine("Welcome to our aMAZEing game.  (Please ensure your console is at least 25 lines tall.)");
Console.WriteLine("Use the arrow keys to move your character, @, from the start (top left) to the * symbol.");
Console.WriteLine("Collect coins, ^, while avoiding bad guys, %.");
Console.WriteLine("Once you've collected all the coins, the gate will open allowing you to get gems, $, and reach the end, *.");
Console.WriteLine("To exit, press the Escape key.");
//Console.WriteLine("If you want a hard maze, press 'H' (and make sure your console is at least 16 lines tall), \notherwise press any other key and we'll give you an easy maze.");

string[] maze = File.ReadAllLines("maze.txt");
char[][] mazeChar = maze.Select(item => item.ToArray()).ToArray();

int mazeWidth = mazeChar[0].Length;
int mazeHeight = mazeChar.Length;

Console.ReadKey(true);      //  Checks for key to start maze
Console.Clear();        


//  Start character at top left and draw maze
DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
Console.SetCursorPosition(0, 02);    //Start 2 lines down to show score and time.
Console.Write("@");     //  Mark character location.
ConsoleKeyInfo keyIn;
Console.CursorVisible = false;  //  Hide cursor for game play.
watch.Start();      // Start watch
do
{
    keyIn = Console.ReadKey(true);      //  Read character

    if (keyIn.Key == ConsoleKey.Escape) //  If escape, quit game.
    {
        Console.WriteLine("\nThanks for playing.  Goodbye.");
        break;
    }


    else if (keyIn.Key == ConsoleKey.RightArrow) //  Right arrow move
    {
        proposedFromLeft = locFromLeft + 1;
        proposedFromTop = locFromTop;
        if (TryMove(proposedFromLeft, proposedFromTop, mazeChar))    // Check valid move.
        {
            locFromLeft++;
            DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
        }
    }
    else if (keyIn.Key == ConsoleKey.LeftArrow) //  Left arrow move
    {
        proposedFromLeft = locFromLeft - 1;
        proposedFromTop = locFromTop;
        if (TryMove(proposedFromLeft, proposedFromTop, mazeChar))    // Check valid move.
        {
            locFromLeft--;
            DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
        }
    }
    else if (keyIn.Key == ConsoleKey.UpArrow) //  Up arrow move
    {
        proposedFromLeft = locFromLeft;
        proposedFromTop = locFromTop - 1;
        if (TryMove(proposedFromLeft, proposedFromTop, mazeChar))    // Check valid move.
        {
            locFromTop--;

            DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
        }
    }
    else if (keyIn.Key == ConsoleKey.DownArrow) //  Down arrow move
    {
        proposedFromLeft = locFromLeft;
        proposedFromTop = locFromTop + 1;
        if (TryMove(proposedFromLeft, proposedFromTop, mazeChar))    // Check valid move.
        {
            locFromTop++;
            DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
        }
    }

    if (mazeChar[locFromTop][locFromLeft] == '*')   //  Detect win.
    {
        watch.Stop();   //  Stop watch and compute time to complete.
        Console.Clear();
        Console.WriteLine($"Congratulations!  You completed the maze in {watch.ElapsedMilliseconds / 1000} seconds.");
        break;
    }

    if (mazeChar[locFromTop][locFromLeft] == '^')   //  Detect coin.
    {
        score = score + 100;
        mazeChar[locFromTop][locFromLeft] = ' ';    //  Collect coin
    }

    if (mazeChar[locFromTop][locFromLeft] == '$')   //  Detect gem.
    {
        score = score + 200;
        mazeChar[locFromTop][locFromLeft] = ' ';    //  Collect gem
    }
    
    if (score == 1000)                              //  Open gate
    {
        mazeChar[09][18] = ' ';
        mazeChar[10][18] = ' ';
        mazeChar[11][18] = ' ';
    }
    if (mazeChar[locFromTop][locFromLeft] == '%')   //  Detect loss.
        {
            watch.Stop();   //  Stop watch and compute time to complete.
            Console.Clear();
            Console.WriteLine($"Sorry, you lose.");
            break;
        }
}
while (keyIn.Key != ConsoleKey.Escape); //  Play game while any key but escape

Console.CursorVisible = true;   //  Make cursor visible in console after game.


static void DrawMaze(int fromLeft, int fromTop, char[][] mazeChar, int score, Stopwatch watch)   //  Method to draw maze.
{

    int maxWidth = mazeChar[0].Length;
    int maxHeight = mazeChar.Length;

    Console.Clear();
    Console.WriteLine($"Score = {score} \nTime: {watch.ElapsedMilliseconds / 1000} seconds."); // Track score and time.

    for (int i = 0; i < maxHeight; i++)
    {
        foreach (char c in mazeChar[i])
        {
            Console.Write(c);

        }
        Console.WriteLine("");
    }
    Console.SetCursorPosition(fromLeft, fromTop+2);
    Console.Write("@");
}


static bool TryMove(int proposedLeft, int proposedTop, char [][] mazeChar)  // Check if move is valid.
{
    int maxWidth = mazeChar[0].Length;
    int maxHeight = mazeChar.Length;

    if (proposedLeft < 0 || proposedLeft > maxWidth)    //  If outside of width, do nothing
    {
        return false;
    }
    else if (proposedTop < 0 || proposedTop >= maxHeight - 1)  //  If outside of height, do nothing
    {
        return false;
    }
    else if (mazeChar[proposedTop][proposedLeft] == '#' || mazeChar[proposedTop][proposedLeft] == '|' )//  Enforce walls.
    {
        return false;
    }
    else { return true; }   //  If valid move, proceed.
}


