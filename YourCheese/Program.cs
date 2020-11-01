﻿
using HamsterCheese.AmongUsMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YourCheese
{
    class Program
    {
        static int tableWidth = 100;

       
        static List<PlayerData> playerDatas = new List<PlayerData>();

        static String[] Colors = new String[]
        {
            "Red",
            "Blue",
            "Green",
            "Pink",
            "Orange",
            "Yellow",
            "Black",
            "White",
            "Purple",
            "Brown",
            "Cyan",
            "Lime"
        };

        static void UpdateCheat()
        {
       
            while (true)
            { 
                Console.Clear();
                Console.WriteLine("Player Data");
                PrintRow("Name", "Color", "OwnerId", "PlayerId", "isImposter", "isAlive", "isConnected");
                PrintLine();

                PlayerComparer comparer = new PlayerComparer();

                playerDatas.Sort(comparer);

                foreach (var data in playerDatas)
                {
                    if (data.IsLocalPlayer)
                    {
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        //set your player name text renderer color
                        data.WriteMemory_SetNameTextColor(new Color(0,1,0,1)); 
                    }
                    if (data.PlayerInfo.Value.IsDead == 1 || data.PlayerInfo.Value.Disconnected == 1)
                        Console.ForegroundColor = ConsoleColor.Red;

                    var Name = HamsterCheese.AmongUsMemory.Utils.ReadString(data.PlayerInfo.Value.PlayerName);
                   PrintRow($"{Name}", $"{Colors[data.PlayerInfo.Value.ColorId]}", $"{data.Instance.OwnerId}", $"{data.Instance.PlayerId}", data.PlayerInfo.Value.IsImpostor == 1 ? $"true" : $"false", data.PlayerInfo.Value.IsDead == 1 ? $"false" : $"true", data.PlayerInfo.Value.Disconnected == 1 ? $"false" : $"true");
                    Console.ForegroundColor = ConsoleColor.White; 
            
                   PrintLine();
                }  
                System.Threading.Thread.Sleep(100);
            }
        }
        static void Main(string[] args)
        {
            // Cheat Init
            if (HamsterCheese.AmongUsMemory.Cheese.Init())
            { 
                // Update Player Data When Every Game
                HamsterCheese.AmongUsMemory.Cheese.ObserveShipStatus((x) =>
                {
                    
                    //stop observe state for init. 
                    foreach(var player in playerDatas) 
                        player.StopObserveState(); 


                    playerDatas = HamsterCheese.AmongUsMemory.Cheese.GetAllPlayers();
                    
                  
                 
                    foreach (var player in playerDatas)
                    {
                        player.onDie += (pos, colorId) => {
                            Console.WriteLine("OnPlayerDied! Color ID :" + colorId);
                        }; 
                        // player state check
                        player.StartObserveState();
                    }

                
                });

                // Cheat Logic
                CancellationTokenSource cts = new CancellationTokenSource();
                Task.Factory.StartNew(
                    UpdateCheat
                , cts.Token); 
            }

            System.Threading.Thread.Sleep(1000000);
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);

            
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        } 
    }
}


