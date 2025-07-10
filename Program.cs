//  Jonathan Bodrero  
//  Jul 10, 2025  (building on code I wrote for Lab 7: Maze)
//  Lab 8:  Maze 2

using System.Diagnostics;  //  Allows stopwatch functionality
Stopwatch watch = new Stopwatch();
Random rand1 = new Random();
Random rand2 = new Random();

int locFromLeft = 0;    //  Set location variables.
int locFromTop = 0;
int proposedFromLeft = 0;
int proposedFromTop = 0;
int score = 0;
int bad1FromLeft = 15;
int bad1FromTop = 5;
int bad2FromLeft = 38;
int bad2FromTop = 15;


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
//mazeChar[locFromTop][locFromLeft] = '@';
//mazeChar[bad1FromTop][bad1FromLeft] = '%';

DrawMaze(locFromLeft, locFromTop, mazeChar, score, watch);
Console.SetCursorPosition(0, 2);
Console.Write("@");     //  Mark character location.
Console.SetCursorPosition(bad1FromLeft, bad1FromTop+2);
Console.Write("%");     //  Mark bad guy 1 location.
Console.SetCursorPosition(bad2FromLeft, bad2FromTop+2);
Console.Write("%");     //  Mark bad guy 2 location.
mazeChar[bad1FromTop][bad1FromLeft] = '%';
mazeChar[bad2FromTop][bad2FromLeft] = '%';

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
    //  Move main character
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
    //  Move bad guy 1;
   int rand1move = rand1.Next(1, 5);
    switch (rand1move)
    {
        case 1:     //  Move bad guy 1 to the right, if valid move.  Otherwise he stays put.
            if (TryMoveBad(bad1FromLeft+1, bad1FromTop, mazeChar) == true)
            {
                mazeChar[bad1FromTop][bad1FromLeft] = ' ';
                bad1FromLeft++;
                mazeChar[bad1FromTop][bad1FromLeft] = '%';
            }
            break;
        case 2:     //  Move bad guy 1 to the left
            if (TryMoveBad(bad1FromLeft-1, bad1FromTop, mazeChar) == true)
            {
                mazeChar[bad1FromTop][bad1FromLeft] = ' ';
                bad1FromLeft--;
                mazeChar[bad1FromTop][bad1FromLeft] = '%';
            }
            break;
        case 3:     //  Move bad guy 1 up
            if (TryMoveBad(bad1FromLeft, bad1FromTop-1, mazeChar) == true)
            {
                mazeChar[bad1FromTop][bad1FromLeft] = ' ';
                bad1FromTop--;
                mazeChar[bad1FromTop][bad1FromLeft] = '%';
            }
            break;
        case 4:     //  Move bad guy 1 down
            if (TryMoveBad(bad1FromLeft, bad1FromTop+1, mazeChar) == true)
            {
                mazeChar[bad1FromTop][bad1FromLeft] = ' ';
                bad1FromTop++;
                mazeChar[bad1FromTop][bad1FromLeft] = '%';
            }
            break;
        
    }
    //  Move bad guy 2;
    
    int rand2move = rand2.Next(1, 5);
    switch (rand2move)
    {
        case 1:     //  Move bad guy 2 to the right, if valid move.  Otherwise he stays put.
            if (TryMoveBad(bad2FromLeft+1, bad2FromTop, mazeChar) == true)
            {
                mazeChar[bad2FromTop][bad2FromLeft] = ' ';
                bad2FromLeft++;
                mazeChar[bad2FromTop][bad2FromLeft] = '%';
            }
            break;
        case 2:     //  Move bad guy 2 to the left
            if (TryMoveBad(bad2FromLeft-1, bad2FromTop, mazeChar) == true)
            {
                mazeChar[bad2FromTop][bad2FromLeft] = ' ';
                bad2FromLeft--;
                mazeChar[bad2FromTop][bad2FromLeft] = '%';
            }
            break;
        case 3:     //  Move bad guy 2 up
            if (TryMoveBad(bad2FromLeft, bad2FromTop-1, mazeChar) == true)
            {
                mazeChar[bad2FromTop][bad2FromLeft] = ' ';
                bad2FromTop--;
                mazeChar[bad2FromTop][bad2FromLeft] = '%';
            }
            break;
        case 4:     //  Move bad guy 2 down
            if (TryMoveBad(bad2FromLeft, bad2FromTop+1, mazeChar) == true)
            {
                mazeChar[bad2FromTop][bad2FromLeft] = ' ';
                bad2FromTop++;
                mazeChar[bad2FromTop][bad2FromLeft] = '%';
            }
            break;
        
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
    else if (mazeChar[proposedTop][proposedLeft] == '#' || mazeChar[proposedTop][proposedLeft] == '|' || mazeChar[proposedTop][proposedLeft] == '%' )//  Enforce walls.
    {
        return false;
    }
    else { return true; }   //  If valid move, proceed.
}

static bool TryMoveBad(int proposedLeft, int proposedTop, char [][] mazeChar)  // Check if move is valid.
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
    else if (mazeChar[proposedTop][proposedLeft] == '#' || mazeChar[proposedTop][proposedLeft] == '|' || mazeChar[proposedTop][proposedLeft] == '%' || mazeChar[proposedTop][proposedLeft] == '^' )//  Enforce walls & coins.
    {
        return false;
    }
    else { return true; }   //  If valid move, proceed.
}

