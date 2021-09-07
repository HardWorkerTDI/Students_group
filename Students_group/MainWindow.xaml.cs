using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;


namespace Students_group
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
		}

		private void AddButt_Click(object sender, RoutedEventArgs e)
		{
			
			//объявление переменной
			string connectionstring;
			SqlConnection cnn;
			//установка соединения
			connectionstring = @"Data Source=DESKTOP-GQ0VNNV\SQLEXPRESS; Initial Catalog=Sudents_for_Vor;User ID=sa; Password=12345";
			//назначить соединение
			cnn = new SqlConnection(connectionstring);
			//открыть соединение
			cnn.Open();
			
			//определяем переменные
			SqlCommand command;
			SqlDataReader dataReader;
			String sql, Output = "";
			//определяем запрос sql
			sql = "Select First_name, Second_name, birth_day from Student";
			//команда sql
			command = new SqlCommand(sql, cnn);
			//объявляем читателя данных
			dataReader = command.ExecuteReader();

			List<string[]> data = new List<string[]>();
			//получаем таблицу с даннными
			while (dataReader.Read())
			{
				data.Add(new string[3]);

				data[data.Count - 1][0] = dataReader[0].ToString();
				data[data.Count - 1][1] = dataReader[0].ToString();
				data[data.Count - 1][2] = dataReader[0].ToString();
				//Output = Output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + "\n";
			}

			//закрыть соединение
			dataReader.Close();
			command.Dispose();
			cnn.Close();

			foreach (string[] s in data)
				RecordsDataGrid.Items.Add(s);

		}

		private void AddStudent_Click(object sender, RoutedEventArgs e)
		{
			if (NameBox.Text == "" || SecondNameBox.Text == "" || BirthDayBox.Text == "")
			{
				MessageBox.Show("Enter the fields", "Error");
			}
				
			else
			{
				//объявление переменной
				string connectionstring;
				SqlCommand command;
				SqlDataAdapter adapter = new SqlDataAdapter();
				//установка соединения
				connectionstring = @"Data Source=DESKTOP-GQ0VNNV\SQLEXPRESS; Initial Catalog=Sudents_for_Vor;User ID=sa; Password=12345";
				//назначить соединение
				SqlConnection cnn = new SqlConnection(connectionstring);
				//открыть соединение
				cnn.Open();

				//определяем запрос sql
				string sql = "Insert into Student ( First_name, Second_name, birth_day) values ('" + NameBox.Text + "','" + SecondNameBox.Text + "','" + BirthDayBox.Text + "')";

				//команда sql
				command = new SqlCommand(sql, cnn);

				adapter.InsertCommand = new SqlCommand(sql, cnn);
				adapter.InsertCommand.ExecuteNonQuery();

				command.Dispose();
				cnn.Close();
				MessageBox.Show("Inserted sucessfully");
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			
		}
	}
}
