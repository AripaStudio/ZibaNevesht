using Gma.System.MouseKeyHook;
using GregsStack.InputSimulatorStandard;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GregsStack.InputSimulatorStandard.Native;
using ZibaNevesht.UserControls;
using static ZibaNevesht.Services.MishmareManager;
using static ZibaNevesht.ViewModels.VMzibaNeveshChakameNevisanPage;
using Application = System.Windows.Application;
using Timer = System.Threading.Timer;

namespace ZibaNevesht.Services
{


    interface IKeyboardManager
    {
        public event EventHandler<string>? GetWord;

        public string LastWord();
    }

    internal interface IMishmareManager
    {
        public Task<ZBmishmare?> GetOneWordData(string name);

        public DateTime GetNextFlushTime();

        public Task RemoveAll();

        public Task<List<ZBmishmare>> GetAllWords(ETypeOfResult typeOfResult, int numberOfRsl = 0);

        public event EventHandler? DataUpdated;
    }

    internal interface IGoshBeZangManager
    {
        public Task Remove(string name);

        public Task removeAll();

        public Task AddOrUpdate(string name, AlertLevel level);
        public Task<List<ZBgoshBeZang>> GetAllData();
    }



    internal interface IZibaNeveshtManager
    {

        public void Start(EnumNameChakameNevisan nameChameSora);

        public void Stop();

        public bool GetIsStart();
    }
    internal class KeyboardManager : IKeyboardManager
    {
        private string CurrentWord;
        private string PreviousWord = string.Empty;
        private string PreviousTwoWord = string.Empty;
        private IKeyboardEvents _globalEvents;

        public event EventHandler<string> GetWord;

        public KeyboardManager()
        {
            _globalEvents = Hook.GlobalEvents();
            _globalEvents.KeyPress += GlobalEventsOnKeyPress;

            Application.Current.Exit += (sender, args) =>
            {
                _globalEvents.KeyPress -= GlobalEventsOnKeyPress;
                ((IDisposable)_globalEvents).Dispose();
            };
        }

        private void GlobalEventsOnKeyPress(object? sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == ' ')
            {
                PreviousTwoWord = PreviousWord;
                PreviousWord = CurrentWord;
                CurrentWord = string.Empty;
                GetWord?.Invoke(this, PreviousWord);
            }
            else
            {
                CurrentWord += e.KeyChar;
            }


        }

        public string LastWord()
        {
            return PreviousTwoWord;
        }
    }

    internal class ZibaNeveshtManager : IZibaNeveshtManager
    {
        private SourcePoemManager SourcePoemManager = new SourcePoemManager();
        private IKeyboardEvents _GlobalEvents;
        private int _index = 0;
        private bool _isInternalSend;
        private string getPoem = "";
        private bool IsStart { get; set; }



        public void Start(EnumNameChakameNevisan nameChameSora)
        {
            getPoem = SourcePoemManager.getPoem(nameChameSora).Result;
            IsStart = true;
            _GlobalEvents = Hook.GlobalEvents();
            _GlobalEvents.KeyDown += GlobalEventsOnKeyDown;
        }


        private void GlobalEventsOnKeyDown(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            
            if (_isInternalSend) return;
            e.Handled = true;

            char nextChar = getPoem[_index % getPoem.Length];
            _isInternalSend = true;
            SendChar(nextChar);

            _index++;
            if (_index >= getPoem.Length) _index = 0;

        }


        private void SendChar(char c)
        {

            SendKeys.SendWait(c.ToString());
            _isInternalSend = false;
        }

        public void Stop()
        {
            getPoem = String.Empty;
            IsStart = false;
            _GlobalEvents.KeyDown -= GlobalEventsOnKeyDown;
            ((IDisposable)_GlobalEvents).Dispose();
        }

        public bool GetIsStart()
        {
            return IsStart;
        }





    }


    internal static class SaveKeyboardManager
    {
        public static readonly IKeyboardManager KeyboardManager = new KeyboardManager();
    }
    internal class GoshBeZangManager : IGoshBeZangManager
    {
        private IGoshBeZang goshBeZang;
        private IKeyboardManager keyboardManager;
        private readonly InputSimulator _simulator = new InputSimulator();

        internal GoshBeZangManager()
        {
            goshBeZang = new GoshBeZang();
            keyboardManager = new KeyboardManager();

            keyboardManager.GetWord += KeyboardManagerOnGetWord;
        }

        private async void KeyboardManagerOnGetWord(object? sender, string e)
        {
            var lastWord = keyboardManager.LastWord();
            if (e != lastWord)
            {
                var getAlertLevel = await goshBeZang.GetAlertLevel(e);
                if (getAlertLevel != null)
                {
                    await ShowAlertLevelMessage(getAlertLevel.Value, e.Length, e);
                }
            }
        }


        public async Task Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            await goshBeZang.Remove(name);
        }

        public async Task removeAll()
        {
            await goshBeZang.RemoveAll();
        }

        public async Task AddOrUpdate(string name, AlertLevel level)
        {
            await goshBeZang.AddOrUpdate(name, level);
        }

        public async Task<List<ZBgoshBeZang>> GetAllData()
        {
            return await goshBeZang.GetAllGoshBeZangNames();
        }

        private async Task ShowAlertLevelMessage(AlertLevel level, int lengthWord, string nameword)
        {
            switch (level)
            {
                case AlertLevel.White:
                {
                    MessageBoxAP.APMessageBox(AlertLevel.White,
                        $"شما رده سپید را زیرپا گذاشتید",
                        $"شما واژه {nameword} را وارد کرده‌اید که در رده سپید است - این پیام برای یادآوری شما آمده است");
                    break;
                }
                case AlertLevel.Orange:
                {
                    var input = MessageBoxAP.APMessageBox(AlertLevel.Orange,
                        $"شما رده نارنگی را زیرپا گذاشتید",
                        $"شما واژه {nameword} را وارد کرده‌اید که در رده نارنگی است - این پیام برای یادآوری شما آمده است");

                    if (input == MessageBoxAP.MessageBoxAPResult.Deleted)
                    {
                        SendBackspaces(lengthWord + 1);
                    }
                    break;
                }
                case AlertLevel.Red:
                {
                    MessageBoxAP.APMessageBox(AlertLevel.Red,
                        $"شما رده سرخ را زیرپا گذاشتید",
                        $"شما واژه {nameword} را وارد کرده‌اید که در رده سرخ است - این پیام برای یادآوری شما آمده است | نوشتار شما پاکسازی میشود");

                    SendBackspaces(lengthWord + 1);
                    break;
                }
            }
        }

        private void SendBackspaces(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _simulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
            }
        }
    }


    

    internal class MishmareManager : IMishmareManager
    {
        private IMishmareDBmanager mishmaredb;
        private IKeyboardManager keyboardManager;
        private ConcurrentDictionary<string, int> SavedForEndTime = new ConcurrentDictionary<string, int>();

        System.Timers.Timer timer = new System.Timers.Timer(30000);

        public DateTime NextFlushTime { get; private set; }


        public event EventHandler? DataUpdated;

        public MishmareManager()
        {
            mishmaredb = new MishmareDBmanager();
            keyboardManager = new KeyboardManager();

            keyboardManager.GetWord += KeyboardManagerOnGetWord;
            TimerOfFlush();
            Application.Current.Exit += async (sender, args) =>
            {
                await RemoveForExitProgram();
            };

            

        }

        private void KeyboardManagerOnGetWord(object? sender, string e)
        {
            string lastWord = keyboardManager.LastWord();

            if (e != lastWord)
            {
                SavedForEndTime.AddOrUpdate(e, 1, (key, oldValue) => oldValue + 1);
            }
        }

        public enum ETypeOfResult
        {
            AllData,
            FilterData
        }

        public async Task RemoveAll()
        {
            await mishmaredb.RemoveAll();
        }

        public async Task<List<ZBmishmare>> GetAllWords(ETypeOfResult typeOfResult, int numberOfRsl = 0)
        {
            var rsl = await mishmaredb.GetAllMishmare();
            if (rsl.Count == 0)
            {
                LoggerManager.Log("No words found.");
            }

            if (typeOfResult == ETypeOfResult.FilterData && numberOfRsl > 0)
            {
                if (rsl.Count > numberOfRsl)
                {
                    rsl = rsl
                        .OrderByDescending(x => x.NumberOfUsedWord)
                        .Take(numberOfRsl)
                        .ToList();
                }

            }
            return rsl;
        }

        public async Task<ZBmishmare?> GetOneWordData(string name)
        {
            return await mishmaredb.GetOneMishmare(name);
        }

        private async Task Flush()
        {
            foreach (var item in SavedForEndTime)
            {
                if (SavedForEndTime.TryRemove(item.Key, out var value))
                {
                    await mishmaredb.AddOrUpdate(item.Key, value);
                }
            }
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void TimerOfFlush()
        {
            NextFlushTime = DateTime.Now.AddSeconds(30);
            timer.Start();
            timer.Elapsed += async (sender, args) =>
            {
                timer.Stop();
                try
                {
                    await Flush();
                }
                finally
                {
                    NextFlushTime = DateTime.Now.AddSeconds(30);
                    timer.Start();
                }
            };

        }

        public DateTime GetNextFlushTime()
        {
            return NextFlushTime;
        }

        private async Task RemoveForExitProgram()
        {
            await Flush();
            timer.Dispose();
            await mishmaredb.RemoveAllExceptForTop10();
        }



    }



    internal class SourcePoemManager
    {
        public async Task<string> getPoem(EnumNameChakameNevisan nameChameNevis)
        {
            List<string> data;
            string name;
            switch (nameChameNevis)
            {
                case EnumNameChakameNevisan.Khayyam:
                    {
                        name = "خیام";
                        break;
                    }
                case EnumNameChakameNevisan.Rodakki:
                    {
                        name = "رودکی";
                        break;
                    }
                case EnumNameChakameNevisan.BabaTaher:
                    {
                        name = "باباطاهر";
                        break;
                    }
                case EnumNameChakameNevisan.Saadi:
                    {
                        name = "سعدی";
                        break;
                    }
                case EnumNameChakameNevisan.Hafez:
                    {
                        name = "حافظ";
                        break;
                    }
                default:
                    {
                        name = "خیام";
                        break;
                    }
            }
            try
            {
                data = await ZibaNeveshtDBmanager.GettingRandomTextOfPoemsOfAPoet(name);

                string zibaText = "";
                foreach (var chame in data)
                {
                    zibaText += Environment.NewLine + chame;
                }
                return zibaText;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
