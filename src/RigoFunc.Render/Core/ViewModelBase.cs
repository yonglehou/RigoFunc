
namespace RigoFunc.Render.Core {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a base abstraction about view model.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">The storage.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns><c>true</c> if the property changed, <c>false</c> otherwise.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null) {
            if (object.Equals(storage, value)) {
                return false;
            }

            storage = value;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Called when property changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            var handler = this.PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
