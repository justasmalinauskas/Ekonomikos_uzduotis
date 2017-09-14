using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekonomika
{
    class GoodsName : INotifyPropertyChanged
    {
        public void RemoveLast()
        {
            Sum -= Last;
        }


        private string name;
        private double sum;
        private double last;
        public GoodsName()
        {
            Sum = 0.00;
            Last = 0.00;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public double Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
            }
        }
        public double Last
        {
            get { return last; }
            set
            {
                last = value;
                OnPropertyChanged("Last");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
