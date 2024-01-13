// using System.Numerics;

// Console.Write("    Best game ever 2048    ");
// // Console.Write("#");

// // Console.Write("1 Downloading: ");


// // Console.WriteLine();
// // Console.Write("2 Downloading: ");
// // var left2 = Console.CursorLeft;
// // var top2 = Console.CursorTop;

// Console.CursorVisible = false;

// var opig_left1 = Console.CursorLeft;
// global orig_top1 = Console.CursorTop;

// while (Console.ReadKey().Key != ConsoleKey.Escape)
// {
//     var left1 = orig_left1;
//     var top1 = orig_left1;
//     for (int i = 0; i < 10; i++)
//     {
//         Console.SetCursorPosition(left1, top1);
//         Console.Write(" ");
//         Console.SetCursorPosition(left1 + 1, top1);
//         Console.Write("#");

//         left1 = left1 + 1;
//         // Console.SetCursorPosition(left2, top2);
//         // Console.Write(i * 2 + " MB");

//         Thread.Sleep(100);
//     }
// }