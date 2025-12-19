using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace ZibaNevesht
{
    public class KeyboardManager
    {
        private IKeyboardEvents _GlobalEvents;
        private int _index = 0;
        private bool _isInternalSend;
        private string getPoem = "";

        public KeyboardManager()
        {
            getPoem = SourcePoemManager.getPoem();
        }
        public void Start()
        {
            _GlobalEvents = Hook.GlobalEvents();
            _GlobalEvents.KeyDown += GlobalEventsOnKeyDown;
        }

        private void GlobalEventsOnKeyDown(object? sender, KeyEventArgs e)
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
            _GlobalEvents.KeyDown -= GlobalEventsOnKeyDown;
            ((IDisposable)_GlobalEvents).Dispose();
        }
    }

    public static class SourcePoemManager
    {
        public static string getPoem()
        {
            string zibaText = "آن قصر که جمشید در او جام گرفت\nآهو بچه کرد و روبَه آرام گرفت\n\nبهرام که گور می‌گرفتی همه عمر\nدیدی که چگونه گور بهرام گرفت؟\n\n" +
                              "ابر آمد و باز بر سر سبزه گریست\nبی بادهٔ گل‌رنگ نمی‌باید زیست\n\nاین سبزه که امروز تماشاگه ماست\nتا سبزهٔ خاک ما تماشاگه کیست!\n\n" +
                              "اکنون که گل سعادتت پربار است\nدست تو ز جام می چرا بیکار است؟\n\nمِی خور که زمانه دشمنی غدار است\nدریافتن روز چنین دشوار است\n\n" +
                              "یک جرعهٔ می ز ملک کاووس به است\nاز تخت قباد و ملکت طوس به است\n\nهر ناله که رندی به سحرگاه زند\nاز طاعت زاهدان سالوس به است\n\n" +
                              "هر ذره که در خاک زمینی بوده‌ست\nپیش از من و تو تاج و نگینی بوده‌ست\n\nگرد از رخ نازنین به آزرم فشان\nکآن هم رخ خوب نازنینی بوده‌ست\n\n" +
                              "نیکی و بدی که در نهاد بشر است\nشادی و غمی که در قضا و قدر است\n\nبا چرخ مکن حواله کاندر ره عقل\nچرخ از تو هزار بار بیچاره‌تر است\n\n" +
                              "می‌خوردن و شادبودن، آیینِ من است\nفارغ‌بودن ز کفر و دین، دینِ من است\n\nگفتم به عروسِ دهر: «کابینِ تو چیست؟»\nگفتا: «دلِ خُرَّمِ تو، کابینِ من است»";
            return zibaText;
        }

      
    }
}
