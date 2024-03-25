using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Helpers
{
    /// <summary>
    /// 用于将Action从后台线程转发到UI线程执行。
    /// </summary>
    internal class UIThreadPoster
    {

        private Action action;
        public UIThreadPoster(Action _action)
        {
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action();
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }
            
        }
    }

    /// <summary>
    /// 用于将Action从后台线程转发到UI线程执行。
    /// </summary>
    internal class UIThreadPoster<TArg>
    {
        private TArg arg;
        private Action<TArg> action;
        public UIThreadPoster(Action<TArg> _action, TArg _arg)
        {
            this.arg = _arg;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    /// <summary>
    /// 用于将Action从后台线程转发到UI线程执行。
    /// </summary>
    internal class UIThreadPoster<TArg, TArg1>
    {
        private TArg arg;
        private TArg1 arg1;
        private Action<TArg, TArg1> action;
        public UIThreadPoster(Action<TArg, TArg1> _action, TArg _arg, TArg1 _arg1)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    /// <summary>
    /// 用于将Action从后台线程转发到UI线程执行。
    /// </summary>
    internal class UIThreadPoster<TArg, TArg1, TArg2>
    {
        private TArg arg;
        private TArg1 arg1;
        private TArg2 arg2;
        private Action<TArg, TArg1, TArg2> action;
        public UIThreadPoster(Action<TArg, TArg1, TArg2> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1, this.arg2);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    /// <summary>
    /// 用于将Action从后台线程转发到UI线程执行。
    /// </summary>
    internal class UIThreadPoster<TArg, TArg1, TArg2, TArg3>
    {
        private TArg arg;
        private TArg1 arg1;
        private TArg2 arg2;
        private TArg3 arg3;
        private Action<TArg, TArg1, TArg2, TArg3> action;
        public UIThreadPoster(Action<TArg, TArg1, TArg2, TArg3> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.arg3 = _arg3;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1, this.arg2, this.arg3);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    internal class UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4>
    {
        private TArg arg;
        private TArg1 arg1;
        private TArg2 arg2;
        private TArg3 arg3;
        private TArg4 arg4;
        private Action<TArg, TArg1, TArg2, TArg3, TArg4> action;
        public UIThreadPoster(Action<TArg, TArg1, TArg2, TArg3, TArg4> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.arg3 = _arg3;
            this.arg4 = _arg4;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1, this.arg2, this.arg3, this.arg4);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    internal class UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4,TArg5>
    {
        private TArg arg;
        private TArg1 arg1;
        private TArg2 arg2;
        private TArg3 arg3;
        private TArg4 arg4;
        private TArg5 arg5;
        private Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5> action;
        public UIThreadPoster(Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4, TArg5 _arg5)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.arg3 = _arg3;
            this.arg4 = _arg4;
            this.arg5 = _arg5;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1, this.arg2, this.arg3, this.arg4, this.arg5);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    internal class UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
    {
        private TArg arg;
        private TArg1 arg1;
        private TArg2 arg2;
        private TArg3 arg3;
        private TArg4 arg4;
        private TArg5 arg5;
        private TArg6 arg6;
        private Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action;
        public UIThreadPoster(Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4, TArg5 _arg5, TArg6 _arg6)
        {
            this.arg = _arg;
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.arg3 = _arg3;
            this.arg4 = _arg4;
            this.arg5 = _arg5;
            this.arg6 = _arg6;
            this.action = _action;
        }

        public void Post()
        {
            CPF.Threading.Dispatcher.MainThread.BeginInvoke(this.Action);
        }

        private void Action()
        {
            try
            {
                this.action(this.arg, this.arg1, this.arg2, this.arg3, this.arg4, this.arg5, this.arg6);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "UIThreadPoster", ESBasic.Loggers.ErrorLevel.Standard);
            }

        }
    }

    internal static class UiSafeInvoker
    {
        public static void ActionOnUI(Action _action)
        {
            UIThreadPoster poster=  new UIThreadPoster(_action);
            poster.Post();
        }

        public static void ActionOnUI<TArg>(Action<TArg> _action, TArg _arg)
        {
            UIThreadPoster<TArg> poster = new UIThreadPoster<TArg>(_action,_arg);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1>(Action<TArg, TArg1> _action, TArg _arg, TArg1 _arg1)
        {
            UIThreadPoster<TArg, TArg1> poster = new UIThreadPoster<TArg, TArg1>(_action,_arg,_arg1);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1, TArg2>(Action<TArg, TArg1, TArg2> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2)
        {
            UIThreadPoster<TArg, TArg1, TArg2> poster = new UIThreadPoster<TArg, TArg1, TArg2>(_action,_arg,_arg1,_arg2);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1, TArg2, TArg3>(Action<TArg, TArg1, TArg2, TArg3> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3)
        {
            UIThreadPoster<TArg, TArg1, TArg2, TArg3> poster = new UIThreadPoster<TArg, TArg1, TArg2, TArg3>(_action, _arg, _arg1, _arg2,_arg3);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1, TArg2, TArg3, TArg4>(Action<TArg, TArg1, TArg2, TArg3, TArg4> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4)
        {
            UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4> poster = new UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4>(_action, _arg, _arg1, _arg2, _arg3, _arg4);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1, TArg2, TArg3, TArg4, TArg5>(Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4, TArg5 _arg5)
        {
            UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4, TArg5> poster = new UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4, TArg5>(_action, _arg, _arg1, _arg2, _arg3, _arg4,_arg5);
            poster.Post();
        }

        public static void ActionOnUI<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Action<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> _action, TArg _arg, TArg1 _arg1, TArg2 _arg2, TArg3 _arg3, TArg4 _arg4, TArg5 _arg5, TArg6 _arg6)
        {
            UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> poster = new UIThreadPoster<TArg, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(_action, _arg, _arg1, _arg2, _arg3, _arg4, _arg5, _arg6);
            poster.Post();
        }
    }
}