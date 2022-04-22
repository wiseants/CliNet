using System.ComponentModel;

namespace Common.Tools
{
    /// <summary>
    /// 최소, 최대값이 제한된 정수형 데이터를 확인하는 클래스.
    /// 증가나 감소가 가능한지를 확인하고 증가나 감소를 실행할 수 있습니다.
    /// </summary>
    public class FiniteInteger : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Fields

        private int _value;
        private int _min;
        private int _max;
        private object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public FiniteInteger()
        {
            Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 최소값.
        /// </summary>
        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (_min == value)
                {
                    return;
                }

                lock(_lockObject)
                {
                    _min = value;
                }

                NotifyPropertyChanged("Min");
                NotifyPropertyChanged("Description");
                NotifyPropertyChanged("IsCanDecrease");
            }
        }

        /// <summary>
        /// 최대값.
        /// </summary>
        public int Max
        {
            get
            {
                return _max;
            }
            set
            {
                if (_max == value)
                {
                    return;
                }

                lock (_lockObject)
                {
                    _max = value;
                }

                NotifyPropertyChanged("Max");
                NotifyPropertyChanged("Description");
                NotifyPropertyChanged("IsCanIncrease");
            }
        }

        /// <summary>
        /// 현재 값.
        /// </summary>
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }

                lock (_lockObject)
                {
                    _value = value;
                }

                NotifyPropertyChanged("Value");
                NotifyPropertyChanged("Description");
                NotifyPropertyChanged("IsCanIncrease");
                NotifyPropertyChanged("IsCanDecrease");
            }
        }

        /// <summary>
        /// 출력하기 위한 설명.
        /// </summary>
        public string Description
        {
            get
            {
                return ToString();
            }
        }

        /// <summary>
        /// 증가 가능 여부.
        /// </summary>
        public bool IsCanIncrease
        {
            get
            {
                return (Value + 1) < Max;
            }
        }

        /// <summary>
        /// 감소 가능 여부.
        /// </summary>
        public bool IsCanDecrease
        {
            get
            {
                return (Value - 1) >= Min;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 증가가 가능하면 증가 시킨다.
        /// </summary>
        /// <param name="value">변화한 후의 값.</param>
        /// <returns>true:증가, false:증가하지 않음.</returns>
        public bool TryIncrease(out int value)
        {
            bool result = false;
            lock (_lockObject)
            {
                result = IsCanIncrease;
                value = result == true ? ++Value : Value;
            }

            return result;
        }

        /// <summary>
        /// 감소가 가능하면 감소 시킨다.
        /// </summary>
        /// <param name="value">변화한 후의 값.</param>
        /// <returns>true:감소, false:감소하지 않음.</returns>
        public bool TryDecrease(out int value)
        {
            bool result = false;
            lock (_lockObject)
            {
                result = IsCanDecrease;
                value = result == true ? --Value : Value;
            }

            return result;
        }

        /// <summary>
        /// 값 초기화.
        /// </summary>
        public void Clear()
        {
            Value = 0;
            Min = 0;
            Max = 0;
        }

        #endregion;

        #region Private methods

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Override methods

        public override string ToString()
        {
            return string.Format("{0} / {1}", Max > 0 ? Value + 1 : 0, Max);
        }

        #endregion
    }
}
