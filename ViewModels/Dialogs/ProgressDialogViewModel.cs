using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.ViewModels.Dialogs
{
    class ProgressDialogViewModel:Screen
    {
        private int _progress = 50;
        public int Progress { 
            get {
                return _progress;
            }
            set {
                _progress = value;
                NotifyOfPropertyChange(() => Progress);
            }
        }
        private int _min = 0;
        public int MinProgress
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                NotifyOfPropertyChange(() => MinProgress);
            }
        }
        private int _max = 100;
        public int MaxProgress
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                NotifyOfPropertyChange(() => MaxProgress);
            }
        }

        private string _titleText;
        public string TitleText
        {
            get
            {
                return _titleText;
            }
            set
            {
                _titleText = value;
                NotifyOfPropertyChange(() => TitleText);
            }
        }

        private string _text = "WIP";
        public string ProgressText
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyOfPropertyChange(() => ProgressText);
            }
        }
        public Action<object> OnCloseAction;

        public ProgressDialogViewModel(int max, int min, int start, string progressText, string titleText)
        {
            Progress = start;
            MaxProgress = max;
            MinProgress = min;
            ProgressText = progressText;
            TitleText = titleText;
        }

        public void Close()
        {
            if (OnCloseAction != null)
            {
                OnCloseAction.Invoke(null);
            }
        }
    }
}
