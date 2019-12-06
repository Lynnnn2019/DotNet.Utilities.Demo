using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlParseDemo.ViewModel
{
    /// <summary>
    /// ViewModel绑定基类
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        //INotifyPropertyChanged属性
        public event PropertyChangedEventHandler PropertyChanged;

        // 属性更改方法
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
