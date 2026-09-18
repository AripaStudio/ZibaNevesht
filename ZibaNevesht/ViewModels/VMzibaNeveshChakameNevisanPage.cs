using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows;
using ZibaNevesht.Services;

namespace ZibaNevesht.ViewModels
{
    public partial class VMzibaNeveshChakameNevisanPage : BaseViewModels
    {
        private IZibaNeveshtManager zibaNeveshtManager;
        private EnumNameChakameNevisan currentAvitveChakameNevis = EnumNameChakameNevisan.Khayyam;

        public VMzibaNeveshChakameNevisanPage()
        {
            zibaNeveshtManager = new ZibaNeveshtManager();
            if (zibaNeveshtManager.GetIsStart())
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }

        public enum EnumNameChakameNevisan
        {
            Khayyam,
            Rodakki,
            BabaTaher,
            Saadi,
            Hafez
        }

        [ObservableProperty]
        private bool _isKhayyamSelected = true;

        partial void OnIsKhayyamSelectedChanged(bool value)
        {
            if (value)
            {
                currentAvitveChakameNevis = EnumNameChakameNevisan.Khayyam;
            }
        }

        [ObservableProperty]
        private bool _isRoudakiSelected;

        partial void OnIsRoudakiSelectedChanged(bool value)
        {
            if (value)
            {
                currentAvitveChakameNevis = EnumNameChakameNevisan.Rodakki;
            }
        }

        [ObservableProperty]
        private bool _isBabaTaherSelected;

        partial void OnIsBabaTaherSelectedChanged(bool value)
        {
            if (value)
            {
                currentAvitveChakameNevis = EnumNameChakameNevisan.BabaTaher;
            }
        }

        [ObservableProperty]
        private bool _isSaadiSelected;

        partial void OnIsSaadiSelectedChanged(bool value)
        {
            if (value)
            {
                currentAvitveChakameNevis = EnumNameChakameNevisan.Saadi;
            }
        }

        [ObservableProperty]
        private bool _isHafizSelected;

        partial void OnIsHafizSelectedChanged(bool value)
        {
            if (value)
            {
                currentAvitveChakameNevis = EnumNameChakameNevisan.Hafez;
            }
        }

        [ObservableProperty]
        private bool _isActive;



        partial void OnIsActiveChanged(bool value)
        {
            if (value)
            {
                switch (currentAvitveChakameNevis)
                {
                    case EnumNameChakameNevisan.Khayyam:
                        {
                            if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
                            zibaNeveshtManager.Start(EnumNameChakameNevisan.Khayyam);
                            break;
                        }
                    case EnumNameChakameNevisan.Rodakki:
                        {
                            if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
                            zibaNeveshtManager.Start(EnumNameChakameNevisan.Rodakki);
                            break;
                        }
                    case EnumNameChakameNevisan.BabaTaher:
                        {
                            if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
                            zibaNeveshtManager.Start(EnumNameChakameNevisan.BabaTaher);
                            break;
                        }
                    case EnumNameChakameNevisan.Saadi:
                        {
                            if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
                            zibaNeveshtManager.Start(EnumNameChakameNevisan.Saadi);
                            break;
                        }
                    case EnumNameChakameNevisan.Hafez:
                        {
                            if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
                            zibaNeveshtManager.Start(EnumNameChakameNevisan.Hafez);
                            break;
                        }
                }
            }
            else
            {
                if (zibaNeveshtManager.GetIsStart()) zibaNeveshtManager.Stop();
            }
        }
    }

    
}
