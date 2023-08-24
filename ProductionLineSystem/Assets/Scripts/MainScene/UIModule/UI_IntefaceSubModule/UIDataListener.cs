using System;

namespace LabProductLine.UIModule
{
    public class UIDataListener<T>
    {
        public event Action<T> OnValueChange;

        private T m_value;
        public T Value
        {
            get => m_value;
            set
            {
                if (m_value.Equals(value)) return;
                m_value = value;
                OnValueChange?.Invoke(m_value);
            }

        }
    }
}
