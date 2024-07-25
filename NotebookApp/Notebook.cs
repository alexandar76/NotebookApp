﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookApp
{
    class Notebook
    {
        public const string IntroMessage = "Welcome to Notebook program v1";
        public const string OutroMessage = "Thanks for using notebook program v1!";
        List<IPageable> pages = new List<IPageable>();

        public delegate void SimpleFunction(string command);
        public delegate void BooleanFunction(bool isOn);
        public event SimpleFunction ItemAdded, ItemRemoved, InputBadCommand;
        public event BooleanFunction loggingToggled;
        Dictionary<string, SimpleFunction> commandLineArgs = new Dictionary<string, SimpleFunction>();
        public readonly string show = "show", _new = "new", delete = "delete", log = "logger";

        public SimpleFunction this[string command]
        {
            get { return commandLineArgs[command];  }
        }

        public Notebook()
        {
            commandLineArgs.Add(show, Show);
            commandLineArgs.Add(_new, New);
            commandLineArgs.Add(delete, Delete);
            commandLineArgs.Add(log, Log);
        }

        public Notebook(params string[] commandLineKeywords) : this()
        {
            for (int i = 0; i < commandLineKeywords.Length; i++)
            {
                if(commandLineKeywords[i] == "")
                {
                    continue;
                }
                switch (i)
                {
                    case 0:
                        commandLineArgs.Remove(show);
                        commandLineArgs.Add(show = commandLineKeywords[i], Show);
                        break;
                    case 1:
                        commandLineArgs.Remove(_new);
                        commandLineArgs.Add(_new = commandLineKeywords[i], New);
                        break;
                    case 3:
                        commandLineArgs.Remove(delete);
                        commandLineArgs.Add(delete = commandLineKeywords[i], Delete);
                        break;
                }
            }
           
        }

        public void Show(string command)
        {
            switch(command)
            {
                case "":
                    Console.WriteLine("Show commands");
                    Console.WriteLine("pages         list all created pages");
                    Console.WriteLine("id of page    display that page");
                    break;
                case "pages":
                    Console.WriteLine("/----------------------------Pages---------------------------\\");
                    for (int i = 0; i < pages.Count; i++)
                    {
                        Console.WriteLine("ID: " + i + " " + pages[i].MyData.title);
                    }
                    break;
                default:
                    int number;
                    if(int.TryParse(command, out number))
                    {
                        Console.WriteLine("Showing page " + number);
                        if(number < pages.Count)
                        {
                            pages[number].Output();
                        }
                        else
                        {
                            if (InputBadCommand != null)
                            {
                                InputBadCommand("Your number was outside of the range of pages please try again");
                            }
                        }
                    } else
                    {
                        if (InputBadCommand != null)
                        {
                            InputBadCommand("You didnt enter pages or valid number please try again");
                        }
                    }
                    break;
            }
        }

        public void New(string command)
        {
           
            switch (command)
            {
                case "":
                    Console.WriteLine("New commands:");
                    Console.WriteLine("message     create new message page");
                    Console.WriteLine("list        create new list page");
                    Console.WriteLine("image       create new image page");
                    break;
                case "message":                  
                    pages.Add(new TextualMessage().Input());
                    if(ItemAdded != null)
                    {
                        ItemAdded("Textual Message");
                    }
                    break;
                case "list":
                    
                    pages.Add(new MessageList().Input());
                    if (ItemAdded != null)
                    {
                        ItemAdded("Message List");
                    }
                    break;
                case "image":
                    
                    pages.Add(new Image().Input());
                    if (ItemAdded != null)
                    {
                        ItemAdded("Image");
                    }
                    break;
                default:
                    if(InputBadCommand != null)
                    {
                        InputBadCommand("You didnt enter message, list or image please try again");
                    }
                    break;
            }
        }

        public void Delete(string command)
        {
            switch (command)
            {
                case "":
                    Console.WriteLine("Delete commands");
                    Console.WriteLine("all           delete all created pages");
                    Console.WriteLine("id of page    delete that page");
                    break;
                case "all":
                    
                    pages.Clear();
                    if(ItemRemoved != null)
                    {
                        ItemRemoved("");
                    }
                    break;
                default:
                    int number;
                    if (int.TryParse(command, out number))
                    {
                        Console.WriteLine("Deleting page " + number);
                        if(number < pages.Count)
                        {
                            pages.RemoveAt(number);

                            if (ItemRemoved != null)
                            {
                                ItemRemoved(number + "");
                            }

                        } else
                        {
                            if (InputBadCommand != null)
                            {
                                InputBadCommand("Your number was outside of the range of pages please try again");
                            }
                        }
                    }
                    else
                    {
                        if (InputBadCommand != null)
                        {
                            InputBadCommand("You didnt input all, pr your number was outside of the range of pages please try again");
                        }
                    }
                    break;
            }
        }

        private void Log(string command)
        {
            switch(command)
            {
                case "":
                    Console.WriteLine("Logger commands:");
                    Console.WriteLine("on           turn logger on");
                    Console.WriteLine("off          turn logger off");
                    break;
                case "on":
                    if (loggingToggled != null)
                    {
                        loggingToggled(true);
                    }
                    break;
                case "off":
                    if(loggingToggled != null)
                    {
                        loggingToggled(false);
                    }
                    break;
                default:
                    if(InputBadCommand != null)
                    {
                        InputBadCommand("Please enter on or off after inputting the log command");
                    }
                    break;
            }
        }
    }
}
