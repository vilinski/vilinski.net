using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace IwAG.Win.UI.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private SqlConnectionString _test;

        public MainWindow()
        {
            InitializeComponent();
        }

        public SqlConnectionString Test
        {
            get { return _test; }
            set
            {
            	_test = value;
				onPropertyChanged(this, _ => Test);
            }
        }

		private void onPropertyChanged<TModel, TPropetyType>(TModel model, Expression<Func<TModel, TPropetyType>> property)
    	{
    		var propertyName = ((MemberExpression)property.Body).Member.Name;
    		if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    	}

    	public event PropertyChangedEventHandler PropertyChanged;
    }
}
